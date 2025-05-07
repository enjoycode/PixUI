// index.ts
import { PixUI } from '../../dotnet/pixui.js'
import { BrotliDecode } from '../../dotnet/decode.min.js';
import { dotnet } from '../../dotnet/loader/index1'
import { WebAssemblyBootResourceType, AssetBehaviors } from '../../dotnet/types/index1'

// 获取应用实例
const app = getApp<IAppOption>()

Component({
  data: {
    canIUseGetUserProfile: wx.canIUse('getUserProfile'),
    canIUseNicknameComp: wx.canIUse('input.type.nickname'),
  },
  methods: {
    async onLoad() {
      //加载分包资源
      let mods = await Promise.all([
        require.async("../../dotnet/pkgs/pkg1/index.js"),
        require.async("../../dotnet/pkgs/pkg2/index.js"),
        require.async("../../dotnet/pkgs/pkg3/index.js"),
        require.async("../../dotnet/pkgs/pkg4/index.js")
      ]);
      let assetMap = new Map<string, number>(); //key=name, value=pkg
      for (let i = 0; i < mods.length; i++) {
        for (const asm of mods[i].assets) {
          assetMap.set(asm, i + 1);
          // try {
          //   wx.getFileSystemManager().accessSync(`/dotnet/pkgs/pkg${i + 1}/${asm}.br`)
          //   console.log("资源文件存在:", asm);
          // } catch (err) {
          //   console.error("资源文件不存在: ", asm);
          // }
        }
      }

      //设置全局变量
      (<any>globalThis).bootUrl = "http://localhost:5000";
      (<any>globalThis).WebAssembly = (<any>globalThis).WXWebAssembly;
      // (<any>globalThis).fs = wx.getFileSystemManager();
      // (<any>globalThis).PixUI = PixUI;

      //查询视图元素
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

      //开始启动dotnet
      const { setModuleImports, getAssemblyExports, getConfig, runMain } = await dotnet
        .withDiagnosticTracing(false)
        // .withApplicationArguments("start")
        //.withConfig({ maxParallelDownloads: 3, enableDownloadRetry: true }) //测试时从Github下载资源有并发限制
        .withConfigSrc("blazor.boot.json")
        .withResourceLoader((type, name, defaultUri, integrity, behavior) => this.loadResource(assetMap, type, name, defaultUri, integrity, behavior))
        .create();

      setModuleImports('main.mjs', {
        node: {
          process: {
            version: () => "V1234567"
          }
        }
      });

      const config: any = getConfig();
      // const exports = await getAssemblyExports(config.mainAssemblyName);
      // const text = exports.MyClass.Greeting();
      // console.log(text);

      console.log("准备执行C# Main")
      runMain(config.mainAssemblyName);
    },

    readFile(path: string, type: "text" | "binary") {
      const fs = wx.getFileSystemManager();

      return new Promise((resolve, reject) => {
        fs.readFile({
          filePath: path,
          encoding: type == "binary" ? undefined : "utf8",
          success: buf => {
            resolve({
              ok: true,
              headers: { length: 0, get: () => null },
              path,
              arrayBuffer: () => buf.data,
              json: () => JSON.parse(<string>buf.data),
              text: () => buf.data,
            })
          },
          fail: err => {
            console.error(`读取资源文件${path}错误: `, err);
            reject({
              ok: false,
              path,
              status: 500,
              headers: { length: 0, get: () => null },
              statusText: "ERR28: " + err,
              arrayBuffer: () => { throw err; },
              json: () => { throw err; },
              text: () => { throw err; }
            })
          },
        });
      });
    },

    // 加载包内资源文件
    loadResource(map: Map<string, number>, type: WebAssemblyBootResourceType, name: string, defaultUri: string, integrity: string, behavior: AssetBehaviors): any {
      //console.log("加载资源: " + name);
      if (type == "manifest") {
        return this.readFile("/dotnet/blazor.boot.json", "text");
      }

      if (type == "dotnetjs") {
        return defaultUri;
      }

      if (type == "configuration") {
        return this.readFile("/dotnet/supportFiles/0_runtimeconfig.bin", "binary");
      }

      if (type == "dotnetwasm") {
        return Promise.resolve({
          ok: true,
          headers: { length: 0, get: (n: string) => n == "content-type" ? "application/wasm" : null },
          defaultUri,
          arrayBuffer: () => { throw new Error('不支持') },
          json: () => { throw new Error('不支持') },
          text: () => { throw new Error('不支持') },
        });
      }

      if (type == "assembly") {
        return new Promise<any>((resolve, reject) => {
          //查找在哪个分包内
          let pkgNo = map.get(name);
          if (!pkgNo) throw new Error("无法找到分包资源:" + name);
          let filePath = `/dotnet/pkgs/pkg${pkgNo}/${name}.br`;
          this.readFile(filePath, "binary").then(res => {
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
      throw new Error("不支持: " + name);
    },

  },
})
