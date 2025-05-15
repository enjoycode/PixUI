// index.ts
import { PixUI } from '../../dotnet/pixui.js'
import { BrotliDecode } from '../../dotnet/decode.min.js';
import { dotnet } from '../../dotnet/loader/index1'
import { WebAssemblyBootResourceType, AssetBehaviors } from '../../dotnet/types/index1'
import { fetch_like } from '../../dotnet/loader/polyfills.js';
import { AbortController } from '../../dotnet/abortController.js';

// 获取应用实例
// const app = getApp<IAppOption>()

Component({
    data: {
        canvasWidth: '100%',
        canvasHeight: '100%',
    },
    methods: {
        async onLoad() {
            //加载分包资源
            let mods = await Promise.all([
                require.async("../../dotnet/pkgs/pkg1/index.js"),
                require.async("../../dotnet/pkgs/pkg2/index.js"),
                require.async("../../dotnet/pkgs/pkg3/index.js"),
                require.async("../../dotnet/pkgs/pkg4/index.js"),
                require.async("../../dotnet/pkgs/pkg5/index.js")
            ]);
            let assetMap = new Map<string, number>(); //key=name, value=pkg
            for (let i = 0; i < mods.length; i++) {
                for (const asm of mods[i].assets) {
                    assetMap.set(asm, i + 1);
                }
            }

            //设置全局变量
            (<any>globalThis).bootUrl = "http://localhost:5000";
            (<any>globalThis).WebAssembly = (<any>globalThis).WXWebAssembly;
            // (<any>globalThis).fs = wx.getFileSystemManager();
            (<any>globalThis).PixUI = PixUI;
            //TODO: 参考node-fetch and node-abort-controller
            (<any>globalThis).fetch = fetch_like;
            //(<any>globalThis).Headers = 
            (<any>globalThis).AbortController = AbortController;

            //查询视图元素
            const query = this.createSelectorQuery();
            let getCanvas = new Promise<any>(resole => {
                query.select("#canvas").fields({ node: true, size: true }).exec(res => resole(res[0].node));
            });
            let getInput = new Promise<any>(resole => {
                query.select("#input").fields({ node: true }).exec(res => resole(res[0].node));
            });
            let canvas = await getCanvas;
            let input = await getInput;
            let width = canvas.width;
            let height = canvas.height;
            let pixelRatio = 2; //wx.getWindowInfo().pixelRatio;
            canvas.width = width * pixelRatio;
            canvas.height = height * pixelRatio;
            console.log(canvas.width, canvas.height)
            this.setData({
                canvasWidth: width  + 'px',
                canvasHeight: height + 'px'
            })
            
            //开始启动dotnet
            const { setModuleImports, getAssemblyExports, getConfig, runMain } = await dotnet
                .withDiagnosticTracing(true)
                .withConfig({
                    environmentVariables: {
                        "MONO_LOG_LEVEL": "debug", //enable Mono VM detailed logging by
                        "MONO_LOG_MASK": "all", // categories, could be also gc,aot,type,...
                    },
                })
                // .withApplicationArguments("start")
                //.withConfig({ maxParallelDownloads: 3, enableDownloadRetry: true }) //测试时从Github下载资源有并发限制
                .withConfigSrc("blazor.boot.json")
                .withResourceLoader((type, name, defaultUri, integrity, behavior) => this.loadResource(assetMap, type, name, defaultUri, integrity, behavior))
                .withRuntimeOptions(["--no-jiterpreter-traces-enabled","--no-jiterpreter-jit-call-enabled", "--no-jiterpreter-interp-entry-enabled"])
                .create();

            setModuleImports('main.mjs', { PixUI: PixUI });

            console.log("准备执行C# Main", canvas.width, canvas.height, wx.getWindowInfo());
            const config: any = getConfig();
            const exports = await getAssemblyExports("PixUI.Platform.Wasm.dll");
            const jsExports = exports.PixUI.Platform.Wasm.JSExports;
            PixUI.Init(config.mainAssemblyName, jsExports, canvas, input);
            jsExports.InitAppArgs(PixUI.GetGLContext((<any>dotnet).instance.Module.GL), width, height, pixelRatio);
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

        //事件处理
        onTouchStart(e) {
            //console.log("onTouchStart", e, e.touches[0].clientX, e.touches[0].clientY);
            PixUI.onMouseDown(0, e.touches[0].clientX, e.touches[0].clientY, 0, 0);
        },
        onTouchEnd(e) {
            // console.log("onTouchEnd", e);
            PixUI.onMouseUp(0, e.changedTouches[0].clientX, e.changedTouches[0].clientY, 0, 0);
        },

    },
})
