export let PixUI = {
    _htmlCanvas: null,
    _htmlInput: null,
    _canvasCtx: null,
    _asmName: "PixUI",
    _baseHref: (document.getElementsByTagName('base')[0] || {href: document.location.origin + '/'}).href,

    Init: function () {
        this.CreateCanvas()
        this.CreateInput()
    },

    CreateCanvas: function () {
        this._htmlCanvas = document.createElement("canvas")
        this._htmlCanvas.style.position = "absolute"
        this._htmlCanvas.style.zIndex = "1"
        this.UpdateCanvasSize()
        document.body.append(this._htmlCanvas)
    },

    CreateInput: function () {
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
    
    GetCanvasTexture: function(device) {
        this._canvasCtx = this._htmlCanvas.getContext("webgpu");
        this._canvasCtx.configure({
            device: device,
            format: navigator.gpu.getPreferredCanvasFormat(),
        });
        return this._canvasCtx.getCurrentTexture();
    },

    UpdateCanvasSize: function () {
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

    BindEvents: function () {
        window.onresize = ev => {
            this.UpdateCanvasSize()
            DotNet.invokeMethod(this._asmName, "OnResize", window.innerWidth, window.innerHeight, window.devicePixelRatio)
        }

        window.onmousemove = ev => {
            ev.preventDefault();
            ev.stopPropagation();
            DotNet.invokeMethod(this._asmName, "OnMouseMove", ev.buttons, ev.x, ev.y, ev.movementX, ev.movementY)
        }
        window.onmouseout = ev => {
            DotNet.invokeMethod(this._asmName, "OnMouseMoveOutWindow")
        }
        window.onmousedown = ev => {
            ev.preventDefault();
            ev.stopPropagation();
            DotNet.invokeMethod(this._asmName, "OnMouseDown", ev.button, ev.x, ev.y, ev.movementX, ev.movementY)
        }
        window.onmouseup = ev => {
            ev.preventDefault();
            ev.stopPropagation();
            DotNet.invokeMethod(this._asmName, "OnMouseUp", ev.button, ev.x, ev.y, ev.movementX, ev.movementY)
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
            DotNet.invokeMethod(this._asmName, "OnKeyDown", ev.key, ev.code, ev.altKey, ev.ctrlKey, ev.shiftKey, ev.metaKey)
            if (ev.code === 'Tab') {
                ev.preventDefault();
            }
        }
        window.onkeyup = ev => {
            DotNet.invokeMethod(this._asmName, "OnKeyUp", ev.key, ev.code, ev.altKey, ev.ctrlKey, ev.shiftKey, ev.metaKey)
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
            DotNet.invokeMethod(this._asmName, "OnScroll", ev.x, ev.y, ev.deltaX, ev.deltaY)
        }
    },

    OnTextInput: function (s) {
        DotNet.invokeMethod(this._asmName, "OnTextInput", s)
    },

    SetCursor: function (name) {
        window.document.body.style.cursor = name
    },

    StartTextInput: function () {
        setTimeout(() => {
            this._htmlInput.focus({preventScroll: true});
        }, 0);
    },

    SetInputRect: function (x, y, w, h) {
        this._htmlInput.style.left = x.toString() + 'px'
        this._htmlInput.style.top = (y + h).toString() + 'px'
        this._htmlInput.style.width = w.toString() + 'px'
    },

    StopTextInput: function () {
        this._htmlInput.blur();
        this._htmlInput.value = '';
    },

    PushWebHistory: function (path, index) {
        let url = this._baseHref + '#' + path;
        history.pushState(index, '', url);
    },

    ReplaceWebHistory: function (path, index) {
        let url = this._baseHref;
        if (path !== '/')
            url += '#' + path;
        history.replaceState(index, '', url);
    },

    PostInvalidateEvent: function () {
        requestAnimationFrame(() => {
            DotNet.invokeMethod(this._asmName, "OnInvalidate")
        });
    },

    ClipboardWriteText: async function (text) {
        await navigator.clipboard.writeText(text)
    },

    ClipboardReadText: async function () {
        return await navigator.clipboard.readText()
    },

    OpenFile: async function (multiple, accept) {
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

    SaveFile: async function (fileName, streamRef) {
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

    BeforeRunApp: function () {
        this._asmName = Blazor.runtime.getConfig().mainAssemblyName

        let routePath = document.location.hash.length > 0 ? document.location.hash.substring(1) : null
        let isMacOS = navigator.userAgent.includes("Mac")
        return {
            Width: window.innerWidth,
            Height: window.innerHeight,
            PixelRatio: window.devicePixelRatio,
            RoutePath: routePath,
            IsMacOS: isMacOS
        }
    },

    WebGPU: {
        requestAdapter: () => navigator.gpu && navigator.gpu.requestAdapter({powerPreference: 'low-power'}),
        createInstance: () => (typeof Blazor.runtime.Module.wasmExports.wgpuCreateInstance === 'function')
            ? Blazor.runtime.Module.wasmExports.wgpuCreateInstance(0) : 0,
        // Port-agnostic handle registration. emdawnwebgpu ships
        // importJs* on Module.WebGPU; the legacy -sUSE_WEBGPU=1
        // port shipped mgr* HandleAllocator tables with .create.
        // emdawnwebgpu tags each imported object's events with the
        // parent EventSource's InstanceID; leaving parent=0 makes
        // WaitAny assert(event->mInstanceId == instance) fire on
        // the first async wait. Pass the current instance handle
        // so device/queue events resolve against it.
        registerDevice: (d, parent) => Blazor.runtime.Module.WebGPU.importJsDevice
            ? Blazor.runtime.Module.WebGPU.importJsDevice(d, parent)
            : Blazor.runtime.Module.WebGPU.mgrDevice.create(d),
        registerQueue: (q, parent) => Blazor.runtime.Module.WebGPU.importJsQueue
            ? Blazor.runtime.Module.WebGPU.importJsQueue(q, parent)
            : Blazor.runtime.Module.WebGPU.mgrQueue.create(q),
        registerTexture: (t) => Blazor.runtime.Module.WebGPU.importJsTexture
            ? Blazor.runtime.Module.WebGPU.importJsTexture(t)
            : Blazor.runtime.Module.WebGPU.mgrTexture.create(t),
        // Under emdawnwebgpu, released handles hold real
        // refcounted C-side WGPUTexture objects — call the C ABI
        // via the exported symbol. Under the legacy port they
        // were HandleAllocator table entries with a JS-side
        // .release. Try the C ABI first (it's the mandatory
        // path under emdawnwebgpu), fall back to the JS table.
        releaseTexture: (id) => {
            if (typeof Blazor.runtime.Module.wasmExports.wgpuTextureRelease === 'function') {
                Blazor.runtime.Module.wasmExports.wgpuTextureRelease(id);
            } else if (Blazor.runtime.Module.WebGPU.mgrTexture) {
                Blazor.runtime.Module.WebGPU.mgrTexture.release(id);
            }
        },
        requestDevice: (adapter) => adapter.requestDevice(),
        deviceQueue: (d) => d.queue,
        createTexture: (d, w, h) => d.createTexture({
            size: {width: w, height: h, depthOrArrayLayers: 1},
            format: 'rgba8unorm',
            usage: 0x10 | 0x01,
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
    }

}