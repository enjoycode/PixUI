export let PixUI = {
    _htmlCanvas: null,
    _htmlInput: null,
    _asmName: "PixUI",
    _useGraphite: false,
    _baseHref: (document.getElementsByTagName('base')[0] || {href: document.location.origin + '/'}).href,
    _api: null,

    Init() {
        this.CreateCanvas()
        this.CreateInput()
    },

    CreateCanvas() {
        this._htmlCanvas = document.createElement("canvas")
        this._htmlCanvas.style.position = "absolute"
        this._htmlCanvas.style.zIndex = "1"
        this.UpdateCanvasSize()
        document.body.append(this._htmlCanvas)
    },

    CreateInput() {
        let input = document.createElement('input')
        input.id = '_i'
        input.style.position = 'absolute'
        input.style.width = input.style.height = input.style.padding = '0'
        input.type = 'text'
        input.style.border = 'none'
        input.style.zIndex = '3'

        document.body.appendChild(input);

        input.addEventListener('input', ev => {
            if (ev.data && !ev.isComposing) { //非IME输入
                this.OnTextInput(ev.data);
            }
        });
        input.addEventListener('compositionend', ev => {
            // this._input.value = '';
            if (ev.data) { //IME输入
                this.OnTextInput(ev.data);
            }
        });

        this._htmlInput = input;
    },

    UpdateCanvasSize() {
        const width = window.innerWidth;
        const height = window.innerHeight;
        const ratio = window.devicePixelRatio;
        //set physical size
        this._htmlCanvas.width = width * ratio;
        this._htmlCanvas.height = height * ratio;
        //set logical size
        this._htmlCanvas.style.width = width + "px";
        this._htmlCanvas.style.height = height + "px";
    },

    BindEvents() {
        window.onresize = ev => {
            this.UpdateCanvasSize()
            if (!this._useGraphite) {
                this._api.OnResize(window.innerWidth, window.innerHeight, window.devicePixelRatio)
            } else {
                //TODO:
            }
        }

        window.onmousemove = ev => {
            ev.preventDefault();
            ev.stopPropagation();
            this._api.OnMouseMove(ev.buttons, ev.x, ev.y, ev.movementX, ev.movementY)
        }
        window.onmouseout = ev => {
            this._api.OnMouseMoveOutWindow()
        }
        window.onmousedown = ev => {
            ev.preventDefault();
            ev.stopPropagation();
            this._api.OnMouseDown(ev.button, ev.x, ev.y, ev.movementX, ev.movementY)
        }
        window.onmouseup = ev => {
            ev.preventDefault();
            ev.stopPropagation();
            this._api.OnMouseUp(ev.button, ev.x, ev.y, ev.movementX, ev.movementY)
        }
        window.oncontextmenu = ev => {
            ev.preventDefault();
            ev.stopPropagation();
        }
        window.ondragover = ev => {
            ev.preventDefault();
        }
        window.ondrop = async (ev) => {
            ev.preventDefault();
            for (const file of ev.dataTransfer.files) {
                await DotNet.invokeMethodAsync(this._asmName, "OnDropFile", ev.x, ev.y,
                    file.name, file.size, file.type, DotNet.createJSStreamReference(file))
            }
        }
        window.onkeydown = ev => {
            this._api.OnKeyDown(ev.key, ev.code, ev.altKey, ev.ctrlKey, ev.shiftKey, ev.metaKey)
            if (ev.code === 'Tab') {
                ev.preventDefault();
            }
        }
        window.onkeyup = ev => {
            this._api.OnKeyUp(ev.key, ev.code, ev.altKey, ev.ctrlKey, ev.shiftKey, ev.metaKey)
            if (ev.code === 'Tab') {
                ev.preventDefault();
            }
        }

        window.onpopstate = ev => {
            //console.log("location: " + document.location + ", state: " + JSON.stringify(ev.state));

            if (typeof ev.state === 'number') {
                //浏览器前进或后退跳转的
                DotNet.invokeMethod(this._asmName, "RouteGoto", ev.state)
            } else {
                //直接在浏览器地址栏输入的
                let path = "/"
                if (document.location.hash.length > 0) {
                    path = document.location.hash.substring(1)
                }
                //同步替换浏览器的历史记录
                let url = this._baseHref + '#' + path
                let id = DotNet.invokeMethod(this._asmName, "NewRouteId")
                history.replaceState(id, "", url)
                DotNet.invokeMethod(this._asmName, "RoutePush", path)
            }
        }

        //注意onwheel事件附加在画布元素上
        this._htmlCanvas.onwheel = ev => {
            ev.preventDefault();
            ev.stopPropagation();
            this._api.OnScroll(ev.x, ev.y, ev.deltaX, ev.deltaY)
        }
    },

    OnTextInput(s) {
        this._api.OnTextInput(s)
    },

    SetCursor(name) {
        window.document.body.style.cursor = name
    },

    StartTextInput() {
        setTimeout(() => {
            this._htmlInput.focus({preventScroll: true});
        }, 0);
    },

    SetInputRect(x, y, w, h) {
        this._htmlInput.style.left = x.toString() + 'px'
        this._htmlInput.style.top = (y + h).toString() + 'px'
        this._htmlInput.style.width = w.toString() + 'px'
    },

    StopTextInput() {
        this._htmlInput.blur();
        this._htmlInput.value = '';
    },

    PushWebHistory(path, index) {
        let url = this._baseHref + '#' + path;
        history.pushState(index, '', url);
    },

    ReplaceWebHistory(path, index) {
        let url = this._baseHref;
        if (path !== '/')
            url += '#' + path;
        history.replaceState(index, '', url);
    },

    async ClipboardWriteText(text) {
        await navigator.clipboard.writeText(text)
    },

    async ClipboardReadText() {
        return await navigator.clipboard.readText()
    },

    async OpenFile(multiple, accept) {
        const input = document.createElement('input')
        input.type = 'file'
        input.multiple = multiple
        input.accept = accept

        // See https://stackoverflow.com/questions/47664777/javascript-file-input-onchange-not-working-ios-safari-only
        Object.assign(input.style, {
            position: 'fixed',
            top: '-100000px',
            left: '-100000px'
        })

        document.body.appendChild(input)

        await new Promise(resolve => {
            input.addEventListener('change', resolve, {once: true})
            input.click()
        })
        input.remove()

        let results = []
        if (input.files) {
            for (let i = 0; i < input.files.length; i++) {
                results.push({
                    FileName: input.files[i].name,
                    FileSize: input.files[i].size,
                    FileStream: DotNet.createJSStreamReference(input.files[i])
                })
            }
        }
        return results
    },

    async SaveFile(fileName, streamRef) {
        //https://github.com/jimmywarting/native-file-system-adapter/blob/master/src/adapters/downloader.js
        //https://stackoverflow.com/questions/77427123/javascript-open-save-as-dialog-box-and-store-content
        const data = await streamRef.arrayBuffer()
        const blob = new Blob([data], {type: 'application/octet-stream; charset=utf-8'})

        const link = document.createElement('a')
        link.download = fileName
        link.href = URL.createObjectURL(blob)
        link.click()
        setTimeout(() => URL.revokeObjectURL(link.href), 10000)
    },

    PostInvalidateEvent() {
        requestAnimationFrame(() => {
            if (!this._useGraphite) {
                this._api.OnInvalidate()
            } else {
                this._api.OnInvalidate(this.WebGPU.getOnScreenTextureId())
            }
        });
    },

    async BeforeRunApp(useGraphite) {
        this._useGraphite = useGraphite
        let runtime = globalThis.Blazor.runtime
        this._asmName = runtime.getConfig().mainAssemblyName
        let exports = await runtime.getAssemblyExports(this._asmName)
        this._api = this._asmName.split('.')
            .reduce((obj, key) => obj[key], exports)
            .WebBrowser

        if (useGraphite) {
            await this.WebGPU.init(this._htmlCanvas, useGraphite)
        } else {
            this.WebGL.init(this._htmlCanvas)
        }

        return {
            GpuInstanceId: this.WebGPU.instanceId,
            GpuDeviceId: this.WebGPU.deviceId,
            GpuQueueId: this.WebGPU.queueId,
            GpuOnScreenTextureId: this.WebGPU.onScreenTextureId,
            GpuOffScreenTextureId: this.WebGPU.offScreenTextureId,
            Width: window.innerWidth,
            Height: window.innerHeight,
            PixelRatio: window.devicePixelRatio,
            RoutePath: document.location.hash.length > 0 ? document.location.hash.substring(1) : null,
            IsMacOS: navigator.userAgent.includes("Mac")
        }
    },

    WebGL: {
        glHandle: null,
        
        init(htmlCanvas) {
            let contextAttributes = {
                'alpha': 1,
                'depth': 1,
                'stencil': 8,
                'antialias': 0,
                'premultipliedAlpha': 1,
                'preserveDrawingBuffer': 0,
                'preferLowPowerToHighPerformance': 0,
                'failIfMajorPerformanceCaveat': 0,
                'enableExtensionsByDefault': 1,
                'explicitSwapControl': 0,
                'renderViaOffscreenBackBuffer': 0,
            }
            contextAttributes['majorVersion'] = (typeof WebGL2RenderingContext !== 'undefined') ? 2 : 1
            let gl = globalThis.Blazor.runtime.Module.GL;
            this.glHandle = gl.createContext(htmlCanvas, contextAttributes)
            if (this.glHandle) {
                gl.makeContextCurrent(this.glHandle)
                gl.currentContext.GLctx.getExtension('WEBGL_debug_renderer_info')
                //https://github.com/dotnet/runtime/issues/76077
                globalThis.GL = gl
                globalThis.GLctx = gl.currentContext.GLctx
            } else {
                alert("Can't use webgl")
            }
        }
    },

    WebGPU: {
        canvasCtx: null,
        instanceId: 0,
        device: null,
        deviceId: 0,
        queueId: 0,
        onScreenTexture: null,
        onScreenTextureId: 0,
        offScreenTexture: null,
        offScreenTextureId: 0,

        async init(htmlCanvas, useGraphite) {
            let adapter = await this.requestAdapter()
            this.device = await adapter.requestDevice()
            this.canvasCtx = htmlCanvas.getContext("webgpu")
            this.canvasCtx.configure({
                device: this.device,
                format: navigator.gpu.getPreferredCanvasFormat(),
                alphaMode: "premultiplied",
                usage: 0x04 | 0x10
            });

            if (useGraphite) {
                this.instanceId = this.createInstance()
                if (this.instanceId === 0) throw 'Cannot obtain a real WGPUInstance'

                this.queueId = this.registerQueue(this.device.queue, this.instanceId)
                this.deviceId = this.registerDevice(this.device, this.instanceId)
            }

            this.onScreenTexture = this.canvasCtx.getCurrentTexture()
            this.onScreenTextureId = this.registerTexture(this.onScreenTexture)
            this.offScreenTexture = this.createTexture(this.device, this.onScreenTexture.width, this.onScreenTexture.height)
            this.offScreenTextureId = this.registerTexture(this.offScreenTexture)
        },

        getOnScreenTextureId() {
            if (this.onScreenTextureId !== 0) {
                this.releaseTexture(this.onScreenTextureId) //TODO:check
            }

            this.onScreenTexture = this.canvasCtx.getCurrentTexture()
            this.onScreenTextureId = this.registerTexture(this.onScreenTexture)
            return this.onScreenTextureId
        },

        requestAdapter: () => navigator.gpu && navigator.gpu.requestAdapter({powerPreference: 'low-power'}),
        createInstance: () => (typeof Blazor.runtime.Module.wasmExports.wgpuCreateInstance === 'function')
            ? Blazor.runtime.Module.wasmExports.wgpuCreateInstance(0) : 0,
        // Port-agnostic handle registration. emdawnwebgpu ships importJs* on Module.WebGPU; the legacy -sUSE_WEBGPU=1
        // port shipped mgr* HandleAllocator tables with .create. emdawnwebgpu tags each imported object's events with
        // the parent EventSource's InstanceID; leaving parent=0 makes WaitAny assert(event->mInstanceId == instance)
        // fire on the first async wait. Pass the current instance handle so device/queue events resolve against it.
        registerDevice: (d, parent) => Blazor.runtime.Module.WebGPU.importJsDevice
            ? Blazor.runtime.Module.WebGPU.importJsDevice(d, parent)
            : Blazor.runtime.Module.WebGPU.mgrDevice.create(d),
        registerQueue: (q, parent) => Blazor.runtime.Module.WebGPU.importJsQueue
            ? Blazor.runtime.Module.WebGPU.importJsQueue(q, parent)
            : Blazor.runtime.Module.WebGPU.mgrQueue.create(q),
        registerTexture: (t) => Blazor.runtime.Module.WebGPU.importJsTexture
            ? Blazor.runtime.Module.WebGPU.importJsTexture(t)
            : Blazor.runtime.Module.WebGPU.mgrTexture.create(t),
        // Under emdawnwebgpu, released handles hold real refcounted C-side WGPUTexture objects — call the C ABI
        // via the exported symbol. Under the legacy port they were HandleAllocator table entries with a JS-side
        // .release. Try the C ABI first (it's the mandatory path under emdawnwebgpu), fall back to the JS table.
        releaseTexture: (id) => {
            if (typeof Blazor.runtime.Module.wasmExports.wgpuTextureRelease === 'function') {
                Blazor.runtime.Module.wasmExports.wgpuTextureRelease(id);
            } else if (Blazor.runtime.Module.WebGPU.mgrTexture) {
                Blazor.runtime.Module.WebGPU.mgrTexture.release(id);
            }
        },
        requestDevice: (adapter) => adapter.requestDevice(),
        createTexture: (d, w, h) => d.createTexture({
            size: {width: w, height: h, depthOrArrayLayers: 1},
            format: navigator.gpu.getPreferredCanvasFormat(),
            alphaMode: "premultiplied",
            usage: 0x01 | 0x04 | 0x10,
        }),
        createBuffer: (d, sz) => d.createBuffer({size: sz, usage: 0x09}),
        createCommandEncoder: (d) => d.createCommandEncoder(),
        copyTextureToBuffer: (e, tex, buf, bpr, w, h) => e.copyTextureToBuffer(
            {texture: tex},
            {buffer: buf, bytesPerRow: bpr, rowsPerImage: h},
            {width: w, height: h, depthOrArrayLayers: 1}),
        submitEncoder: (d, e) => d.queue.submit([e.finish()]),
        mapBufferRead: (b) => b.mapAsync(0x01),
        getMappedBase64: (b, bpr, w, h) => {
            const mapped = new Uint8Array(b.getMappedRange());
            const widthBytes = w * 4;
            const packed = new Uint8Array(widthBytes * h);
            for (let r = 0; r < h; r++)
                packed.set(mapped.subarray(r * bpr, r * bpr + widthBytes), r * widthBytes);
            b.unmap();
            b.destroy();
            let s = '';
            const CHUNK = 0x8000;
            for (let i = 0; i < packed.length; i += CHUNK)
                s += String.fromCharCode.apply(null, packed.subarray(i, i + CHUNK));
            return btoa(s);
        },
    },
}