export const PixUI = {
    _exports: null,
    _htmlCanvas: null,
    _htmlInput: null,
    _asmName: "PixUI",
    _baseHref: "http://localhost" /*(document.getElementsByTagName('base')[0] || {href: document.location.origin + '/'}).href*/,

    GetGLContext: function (gl) {
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
        contextAttributes['majorVersion'] = 1; /*(typeof WebGL2RenderingContext !== 'undefined') ? 2 : 1*/
        let ctx = this._htmlCanvas.getContext('webgl');
        let handle = gl.registerContext(ctx, contextAttributes);
        if (handle) {
            gl.makeContextCurrent(handle)
            ctx.getExtension('WEBGL_debug_renderer_info'); //gl.currentContext.GLctx.getExtension('WEBGL_debug_renderer_info')
            //https://github.com/dotnet/runtime/issues/76077
            globalThis.GL = gl
            globalThis.GLctx = ctx; //gl.currentContext.GLctx
        } else {
            //TODO: fallback to software surface
            alert("Can't use gpu")
        }

        return handle;
    },

    BindEvents: function () {
        // window.onresize = ev => {
        //     this.UpdateCanvasSize()
        //     DotNet.invokeMethod(this._asmName, "OnResize", window.innerWidth, window.innerHeight, window.devicePixelRatio)
        // }

        // window.onmousemove = ev => {
        //     ev.preventDefault();
        //     ev.stopPropagation();
        //     DotNet.invokeMethod(this._asmName, "OnMouseMove", ev.buttons, ev.x, ev.y, ev.movementX, ev.movementY)
        // }
        // window.onmouseout = ev => {
        //     DotNet.invokeMethod(this._asmName, "OnMouseMoveOutWindow")
        // }
        // window.onmousedown = ev => {
        //     ev.preventDefault();
        //     ev.stopPropagation();
        //     DotNet.invokeMethod(this._asmName, "OnMouseDown", ev.button, ev.x, ev.y, ev.movementX, ev.movementY)
        // }
        // window.onmouseup = ev => {
        //     ev.preventDefault();
        //     ev.stopPropagation();
        //     DotNet.invokeMethod(this._asmName, "OnMouseUp", ev.button, ev.x, ev.y, ev.movementX, ev.movementY)
        // }
        // window.oncontextmenu = ev => {
        //     ev.preventDefault();
        //     ev.stopPropagation();
        // }
        // window.ondragover = ev => {
        //     ev.preventDefault();
        // }
        // window.ondrop = async (ev) => {
        //     ev.preventDefault();
        //     for (const file of ev.dataTransfer.files) {
        //         await DotNet.invokeMethodAsync(this._asmName, "OnDropFile", ev.x, ev.y, file.name, file.size, file.type, DotNet.createJSStreamReference(file))
        //     }
        // }
        // window.onkeydown = ev => {
        //     DotNet.invokeMethod(this._asmName, "OnKeyDown", ev.key, ev.code, ev.altKey, ev.ctrlKey, ev.shiftKey, ev.metaKey)
        //     if (ev.code === 'Tab') {
        //         ev.preventDefault();
        //     }
        // }
        // window.onkeyup = ev => {
        //     DotNet.invokeMethod(this._asmName, "OnKeyUp", ev.key, ev.code, ev.altKey, ev.ctrlKey, ev.shiftKey, ev.metaKey)
        //     if (ev.code === 'Tab') {
        //         ev.preventDefault();
        //     }
        // }

        // window.onpopstate = ev => {
        //     //console.log("location: " + document.location + ", state: " + JSON.stringify(ev.state));

        //     if (typeof ev.state === 'number') {
        //         //浏览器前进或后退跳转的
        //         DotNet.invokeMethod(this._asmName, "RouteGoto", ev.state)
        //     } else {
        //         //直接在浏览器地址栏输入的
        //         let path = "/"
        //         if (document.location.hash.length > 0) {
        //             path = document.location.hash.substring(1)
        //         }
        //         //同步替换浏览器的历史记录
        //         let url = this._baseHref + '#' + path
        //         let id = DotNet.invokeMethod(this._asmName, "NewRouteId")
        //         history.replaceState(id, "", url)
        //         DotNet.invokeMethod(this._asmName, "RoutePush", path)
        //     }
        // }

        // //注意onwheel事件附加在画布元素上
        // this._htmlCanvas.onwheel = ev => {
        //     ev.preventDefault();
        //     ev.stopPropagation();
        //     DotNet.invokeMethod(this._asmName, "OnScroll", ev.x, ev.y, ev.deltaX, ev.deltaY)
        // }
    },

    OnTextInput: function (s) {
        DotNet.invokeMethod(this._asmName, "OnTextInput", s)
    },

    SetCursor: function (name) {
        window.document.body.style.cursor = name
    },

    StartTextInput: function () {
        setTimeout(() => {
            this._htmlInput.focus({ preventScroll: true });
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
        this._htmlCanvas.requestAnimationFrame(() => {
            this._exports.OnInvalidate();
        });
    },

    ClipboardWriteText: async function (text) {
        await navigator.clipboard.writeText(text)
    },

    ClipboardReadText: async function () {
        return await navigator.clipboard.readText()
    },

    Init: function (asmName, exports, canvas, input) {
        this._exports = exports;
        this._asmName = asmName;
        this._htmlCanvas = canvas;
        //this._htmlInput = input;
    },

}