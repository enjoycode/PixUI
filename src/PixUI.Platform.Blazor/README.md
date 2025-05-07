修改项目文件.csproj
参考本示例

https://github.com/dotnet/runtime/blob/main/src/mono/wasm/build/WasmApp.Common.targets

https://github.com/dotnet/runtime/blob/main/src/mono/wasm/features.md

https://learn.microsoft.com/en-us/dotnet/core/deploying/trimming/trimming-options#trimming-framework-library-features

https://learn.microsoft.com/en-us/aspnet/core/blazor/webassembly-native-dependencies?view=aspnetcore-8.0

关于Exception: 
acknowledge that there is not enough documentations on how Wasm EH and SjLj work together. I'm planning on improving that too.

But in the meantime, to explain the status quo, there are two EH methods (Emscripten (old) and Wasm (new)), and also there are two SjLj methods (Emscripten (old) and Wasm (new)). The Emscripten EH/SjLj use JavaScript to do emulate things. They've been around longer so they are more stable, but they are slow and they increase code size more. Wasm EH/SjLj uses the new Wasm EH proposal. This has not been standardized yet. This needs browser support, but most major browsers already support it, including V8, Firefox, and Safari. I regard Wasm EH stable enough to be tried in bigger applications at this point. Wasm SjLj is little more experimental; even though it uses the same browser support, the toolchain support has been around for shorter amount of time than that of Wasm EH. But you can certainly try.

And there are restrictions depending on which combinations you want to use.

Emscripten EH + Emscripten SjLj: Works fine. No restrictions. Slow. Stable.
Emscripten EH + Wasm SjLj: We don't support this.
Wasm EH + Emscripten SjLj: We support this as an interim measure, but there is a restriction that you can't use setjmp call with exceptions within the same function. This is what you encounter here. Even though you don't use try-catch in that function, stack-allocated objects can create landingpads, so this can happen. You can modify source code to work around it but if it is third party code as in you case that might not be easy.
Wasm EH + Wasm SjLj: What we eventually want to recommend to everyone when things are stable enough. But you can try. You can add -sSUPPORT_LONGJMP=wasm to both compile and link time flags. This combination has a restriction too but a less bothering one: You can't call setjmp within catch clause in try-catch.