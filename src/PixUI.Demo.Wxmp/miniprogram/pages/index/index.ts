// index.ts
import { PixUI } from '../../dotnet/pixui.js'
import { BrotliDecode } from '../../dotnet/decode.min.js';
import { dotnet } from '../../dotnet/loader/index1'
import { WebAssemblyBootResourceType, AssetBehaviors } from '../../dotnet/types/index1'
import { fetch_like } from '../../dotnet/loader/polyfills'

// 获取应用实例
const app = getApp<IAppOption>()

Component({
  data: {
    canIUseGetUserProfile: wx.canIUse('getUserProfile'),
    canIUseNicknameComp: wx.canIUse('input.type.nickname'),
  },
  methods: {
    async onLoad() {
      (<any>globalThis).bootUrl = "https://raw.githubusercontent.com/enjoycode/WasmDemo/refs/heads/main/";
      (<any>globalThis).WebAssembly = (<any>globalThis).WXWebAssembly;
      // (<any>globalThis).fs = wx.getFileSystemManager();
      // (<any>globalThis).PixUI = PixUI;

      const query = this.createSelectorQuery();
      let getCanvas = new Promise<any>(resole => {
        query.select("#canvas").fields({ node: true }).exec(res => resole(res[0].node));
      });
      let getInput = new Promise<any>(resole => {
        query.select("#input").fields({ node: true }).exec(res => resole(res[0].node));
      });
      let canvas = await getCanvas;
      let input = await getInput;
      console.log(canvas, input);

      const { setModuleImports, getAssemblyExports, getConfig, runMain } = await dotnet
        .withDiagnosticTracing(false)
        // .withApplicationArguments("start")
        .withConfig({ maxParallelDownloads : 3, enableDownloadRetry: true }) //测试时从Github下载资源有并发限制
        .withConfigSrc("blazor.boot.json")
        .withResourceLoader((type, name, defaultUri, integrity, behavior) => this.loadResource(type, name, defaultUri, integrity, behavior))
        .create();

      setModuleImports('main.mjs', {
        node: {
          process: {
            version: () => "V1234567"
          }
        }
      });

      const config: any = getConfig();
      const exports = await getAssemblyExports(config.mainAssemblyName);
      const text = exports.MyClass.Greeting();
      console.log(text);

      console.log("准备执行C# Main")
      runMain(config.mainAssemblyName);
    },

    // 加载包内资源文件
    loadResource(type: WebAssemblyBootResourceType, name: string, defaultUri: string, integrity: string, behavior: AssetBehaviors): any {
      const userDataPath = wx.env.USER_DATA_PATH;
      const bootPath = userDataPath + '/';
      const fs = wx.getFileSystemManager();
      const uri: string = (<any>globalThis).bootUrl + name;

      //console.log("加载资源: " + name);
      if (type == "dotnetjs") {
        return uri;
      }

      if (type == "dotnetwasm") {
        return new Promise<any>((resolve, reject) => {
          fetch_like(uri + '.br', { method: "GET" }).then(res => {
            resolve(res);
            // WXWebAssemlby只能加载代码包内的，保存dotnet.native.wasm.br至本地
            // fs.writeFile({
            //   filePath: bootPath + name + ".br",
            //   data: res.arrayBuffer(),
            //   success: () => resolve(res),
            //   fail: (err) => console.error(err),
            // });
          }).catch(err => reject(err));
        });
      }

      if (type == "assembly") {
        return new Promise<any>((resolve, reject) => {
          fetch_like(uri + '.br', { method: "GET" }).then(res => {
            const originalResponseBuffer = res.arrayBuffer();
            const originalResponseArray = new Int8Array(originalResponseBuffer);
            const decompressedResponseArray = BrotliDecode(originalResponseArray);
            const contentType = 'application/wasm';
            resolve({
              ok: true,
              headers: { length: 0, get: (n: string) => n == "content-type" ? contentType : null },
              defaultUri,
              arrayBuffer: () => decompressedResponseArray,
              json: () => { throw new Error('不支持') },
              text: () => { throw new Error('不支持') },
            });
          }).catch(err => reject(err));
        });
      }

      // 其他类型的资源
      let req: any = { method: "GET" };
      if (type == "manifest" || behavior == "symbols") {
        req.responseType = "text";
      }
      return fetch_like(uri, req);
    },

  },
})
