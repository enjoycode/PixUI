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
            ctx.getExtension('WEBGL_debug_renderer_info');
            //https://github.com/dotnet/runtime/issues/76077
            globalThis.GL = gl
            globalThis.GLctx = ctx; //gl.currentContext.GLctx
        } else {
            //TODO: fallback to software surface
            alert("Can't use gpu")
        }

        return handle;
    },

    OnTextInput: function (s) {
        //DotNet.invokeMethod(this._asmName, "OnTextInput", s)
    },

    SetCursor: function (name) {
        //window.document.body.style.cursor = name
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

    //以下事件处理
    onMouseDown(button, x, y, dx, dy) {
        if (!this._exports) return;
        this._exports.OnMouseDown(button, x, y, dx, dy);
    },
    onMouseUp(button, x, y, dx, dy) {
        if (!this._exports) return;
        this._exports.OnMouseUp(button, x, y, dx, dy);
    },

}