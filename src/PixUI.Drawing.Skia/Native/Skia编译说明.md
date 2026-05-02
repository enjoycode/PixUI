https://skia.org/docs/user/build/

# 修改BUILD.gn包含SkParagraph及相关C-Api
在/BUILD.gn的skia_component("skia")最后添加(参考修改过的"# ====Rick add for PixUI====")

~~在third_parth/icu/BUILD.gn添加~~
```text
third_party("dotnet") {
    public_include_dirs = [
      "../../../dotnet_icu/icu/icu/icu4c/source/common",
      "../../../dotnet_icu/icu/icu/icu4c/source/i18n",
      ".",
    ]
    public_defines = [
      "U_USING_ICU_NAMESPACE=0",
      "U_DISABLE_RENAMING",
      "SK_USING_THIRD_PARTY_ICU",
    ]
    defines = [
      "U_COMMON_IMPLEMENTATION",
      "U_STATIC_IMPLEMENTATION",
      "U_ENABLE_DYLOAD=0",
      "U_I18N_IMPLEMENTATION",
      "_XOPEN_SOURCE=0",
      "__i386__",
    ]
  }
```

bin/gn args --list out/wasm 列出参数

# 编译Skia
## WebAssembly

### ~~先编译dotnet版本的icu~~ (不再需要)

~~1. clone dotnet icu~~
```bash
git clone -b dotnet/release/8.0 https://github.com/dotnet/icu.git
```

~~2. 修改icu-filters/icudt_wasm.json~~
TODO:其他如CJK等同样修改，暂只改默认的
```json
{
    "collationUCAData": "implicithan",
    "localeFilter": {
        "filterType": "locale",
        "includeScripts": false,  
        "includeChildren": false,
        "whitelist": [
            "en_US",
            "zh_CN",
            "zh_Hans_HK",
            "zh_SG",
            "zh_HK",
            "zh_TW"
        ]
    },
    "featureFilters": {
        "conversion_mappings": "exclude",
        "confusables": "exclude",
        "stringprep": "exclude",
        "zone_tree": "exclude",
        "zone_supplemental": "exclude",
        "translit": "exclude",
        "unames": "exclude",
        "ulayout": "exclude",
        "unit_tree": "exclude",
        "rbnf_tree": "exclude",
        "cnvalias": "exclude",
        "lang_tree": "exclude",
        "region_tree": "exclude",
        "normalization": {
            "blacklist": [
                "nfkc_cf",
                "nfkc"
            ]
        },
        "misc": {
            "whitelist": [
                "currencyNumericCodes",
                "numberingSystems",
                "icuver",
                "supplementalData",
                "keyTypeData",
                "icustd",
                "likelySubtags"
            ]
        },
        "curr_tree": {
            "whitelist": ["root"]
        },
        "brkitr_dictionaries": "exclude"
    },
    "resourceFilters": [
        {
            "categories": ["locales_tree"],
            "rules": [
                "-/characterLabel",
                "-/measurementSystemNames",
                "-/listPattern",
                "-/fields",
                "-/delimiters",
                "-/Ellipsis",
                "-/NumberElements/latn/miscPatterns",
                "-/NumberElements/latn/patternsLong",
                "-/NumberElements/latn/patternsShort",
                "-/NumberElements/*/patternsLong",
                "-/NumberElements/*/patternsShort",
                "-/NumberElements/minimalPairs",
                "-/parse",
                "-/AuxExemplarCharacters",
                "-/ExemplarCharacters",
                "-/ExemplarCharactersIndex",
                "-/ExemplarCharactersNumbers",
                "-/ExemplarCharactersPunctuation",
                "-/MoreInformation"
            ]
        },
        {
            "categories": ["locales_tree"],
            "files": {
                "blacklist": ["root"]
            },
            "rules": [
                "-/calendar/*",
                "+/calendar/default",
                "+/calendar/gregorian",
                "+/calendar/generic"
            ]
        },
        {
            "categories": ["locales_tree"],
            "files": {
                "whitelist": ["ja"]
            },
            "rules": [
                "-/calendar/*",
                "+/calendar/default",
                "+/calendar/gregorian",
                "+/calendar/generic",
                "+/calendar/japanese"
            ]
        },
        {
            "categories": ["coll_tree"],
            "rules": [
                "-/*/*",
                "+/collations/default",
                "+/collations/standard",
                "+/collations/private-kana",
                "-/UCARules"
            ]
        },
        {
            "categories": ["misc"],
            "files": {
                "whitelist": ["supplementalData"]
            },
            "rules": [
                "-/*",
                "+/calendarData",
                "+/calendarPreferenceData",
                "+/cldrVersion",
                "+/measurementData",
                "+/codeMappings",
                "+/idValidity",
                "+/timeData",
                "+/weekData"
            ]
        },
        {
            "categories": [ "brkitr_tree" ],
            "rules": [ "-/Version" ]
        }
    ]
}
```

~~3. 设置编译环境~~
```bash
mkdir artifacts
cd artifacts
git clone https://github.com/emscripten-core/emsdk.git
cd emsdk
./emsdk install 3.1.34
./emsdk activate 3.1.34
source emsdk_env.sh
export EMSDK_PATH=/Users/rick/Projects/dotnet_icu/icu/artifacts/emsdk(实际目录)
```

~~4. 回到icu目录开始编译~~
```bash
./build.sh /p:TargetOS=Browser /p:TargetArchitecture=wasm /p:IcuTracing=true
```

### 编译skia

参考modules/canvaskit/compile.sh or jetbrains/skia-pack/script/build.py
注意用于链接入dotnet wasm时: 
1. skia_use_system_zlib=true，并且在macos需要"-I/opt/homebrew/opt/zlib/include",及定义宏FT_CONFIG_OPTION_SYSTEM_ZLIB
2. skia_use_system_icu=false但修改BUILD.gn使用修改过的icu::dotnet
3. 暂skia_enable_pdf=false skia_pdf_subset_harfbuzz=false
4. std::isalnum(name[i], std::locale::classic())指定第二个参数行为不确定，所以修改SkMesh.cpp check_name()去掉第二个参数
```c++
static bool check_name(const SkString& name) {
    if (name.isEmpty()) {
        return false;
    }
    for (size_t i = 0; i < name.size(); ++i) {
        // if (name[i] != '_' && !std::isalnum(name[i], std::locale::classic())) {
        //     return false;
        // }
        // Rick for WebAssembly
        if (name[i] != '_' && !std::isalnum(name[i])) {
            return false;
        }
    }
    return true;
} 
```

skia_enable_fontmgr_custom_empty=true skia_enable_fontmgr_custom_embedded=false
skia_use_libgrapheme=true
1.先正常生成libgrapheme头文件,再修改third_party/libgrapheme/BUILD.gn不再生成,
2.bin/gn gen时的错误可忽略 "ERROR Input to target not generated by a dependency."
3.将gen目录复制到当前目录下，再执行ninja -C out/wasm skia

```bash
bin/gn gen out/wasm --args='target_cpu="wasm" is_official_build=true is_component_build=false skia_enable_optimize_size=true skia_enable_tools=false skia_enable_gpu=true skia_use_webgl=true skia_use_webgpu=false skia_gl_standard="webgl" skia_enable_ganesh=true skia_use_angle=false skia_use_vulkan=false skia_use_metal=false skia_enable_skottie=false skia_use_dng_sdk=false skia_use_lua=false skia_enable_pdf=false skia_pdf_subset_harfbuzz=false skia_use_xps=false skia_enable_svg=false skia_use_freetype_zlib=false skia_use_libjpeg_turbo_decode=false skia_use_libjpeg_turbo_encode=false skia_use_no_jpeg_encode=true skia_use_libpng_decode=false skia_use_libpng_encode=false skia_use_no_png_encode=true skia_use_no_webp_encode=true skia_use_wuffs=false skia_use_zlib=true skia_use_icu=false skia_use_libgrapheme=true skia_use_harfbuzz=true skia_use_piex=false skia_use_system_icu=false skia_use_system_harfbuzz=false skia_use_system_libpng=false skia_use_system_libwebp=false skia_use_system_zlib=true skia_enable_fontmgr_custom_directory=false skia_enable_fontmgr_custom_empty=true skia_enable_fontmgr_custom_embedded=false skia_enable_fontmgr_empty=false extra_cflags=[ "-msimd128", "-sSUPPORT_LONGJMP=wasm", "-fwasm-exceptions", "-I/opt/homebrew/opt/zlib/include", "-DSKIA_C_DLL", "-DSK_SUPPORT_GPU=1", "-DSK_GL", "-DCK_ENABLE_WEBGL", "-DSK_GANESH", "-DSK_DISABLE_LEGACY_SHADERCONTEXT", "-DSK_DISABLE_EFFECT_DESERIALIZATION", "-DXML_POOR_ENTROPY", "-DSK_FORCE_8_BYTE_ALIGNMENT", "-DEMSCRIPTEN_HAS_UNBOUND_TYPE_NAMES=0", "-DSK_DISABLE_AAA", "-DGR_GL_CHECK_ALLOC_WITH_GET_ERROR=0", "-DFT_CONFIG_OPTION_SYSTEM_ZLIB" ] extra_cflags_cc=[ "-msimd128", "-fwasm-exceptions" ] extra_ldflags=[ "-Oz", "-fno-rtti", "-fwasm-exceptions", "-sSUPPORT_LONGJMP=wasm", "-sDISABLE_EXCEPTION_CATCHING", "-std=c++17", "--bind", "--no-entry", "-sMODULARIZE", "-sNO_EXIT_RUNTIME=1", "-sWASM", "-sSTRICT=1", "-sUSE_WEBGL2=1", "-sMAX_WEBGL_VERSION=2", "-sALLOW_MEMORY_GROWTH" ] cc="/usr/local/share/dotnet/packs/Microsoft.NET.Runtime.Emscripten.3.1.56.Sdk.osx-arm64/9.0.4/tools/emscripten/emcc" cxx="/usr/local/share/dotnet/packs/Microsoft.NET.Runtime.Emscripten.3.1.56.Sdk.osx-arm64/9.0.4/tools/emscripten/em++" ar="/usr/local/share/dotnet/packs/Microsoft.NET.Runtime.Emscripten.3.1.56.Sdk.osx-arm64/9.0.4/tools/emscripten/emar"'

ninja -C out/wasm skia
```

小程序用的wasm编译方案，暂不使用wasm-exceptions
4."-DSK_DISABLE_EFFECT_DESERIALIZATION"
5. skia_use_libpng_decode = true skia_use_libwebp_decode=false skia_use_libwebp_encode=false
6. 修改third_paty/harfbuzz/Build.gn的defines,加入"HB_TINY", https://skia-review.googlesource.com/c/skia/+/586313
7. extra_ldfalgs "-Oz"优化大小
```bash
bin/gn gen out/wasm-wxmp-135 --args='target_cpu="wasm" is_official_build=true is_component_build=false skia_enable_optimize_size=true skia_enable_tools=false skia_enable_gpu=true skia_use_webgl=true skia_use_webgpu=false skia_gl_standard="webgl" skia_enable_ganesh=true skia_use_angle=false skia_use_vulkan=false skia_use_metal=false skia_enable_skottie=false skia_use_dng_sdk=false skia_use_lua=false skia_enable_pdf=false skia_pdf_subset_harfbuzz=false skia_use_xps=false skia_enable_svg=false skia_use_freetype_zlib=false skia_use_libjpeg_turbo_decode=false skia_use_libjpeg_turbo_encode=false skia_use_no_jpeg_encode=true skia_use_libpng_decode=true skia_use_libpng_encode=false skia_use_no_png_encode=true skia_use_no_webp_encode=true skia_use_libwebp_decode=false skia_use_libwebp_encode=false skia_use_wuffs=false skia_use_zlib=true skia_use_icu=false skia_use_libgrapheme=true skia_use_harfbuzz=true skia_use_piex=false skia_use_system_icu=false skia_use_system_harfbuzz=false skia_use_system_libpng=false skia_use_system_libwebp=false skia_use_system_zlib=true skia_enable_fontmgr_custom_directory=false skia_enable_fontmgr_custom_empty=true skia_enable_fontmgr_custom_embedded=false skia_enable_fontmgr_empty=false extra_cflags=[ "-msimd128", "-I/opt/homebrew/opt/zlib/include", "-DSKIA_C_DLL", "-DSK_SUPPORT_GPU=1", "-DSK_GL", "-DCK_ENABLE_WEBGL", "-DSK_GANESH", "-DSK_DISABLE_LEGACY_SHADERCONTEXT", "-DSK_DISABLE_EFFECT_DESERIALIZATION", "-DXML_POOR_ENTROPY", "-DSK_FORCE_8_BYTE_ALIGNMENT", "-DEMSCRIPTEN_HAS_UNBOUND_TYPE_NAMES=0", "-DSK_DISABLE_AAA", "-DGR_GL_CHECK_ALLOC_WITH_GET_ERROR=0", "-DFT_CONFIG_OPTION_SYSTEM_ZLIB" ] extra_cflags_cc=[ "-msimd128" ] extra_ldflags=[ "-Oz", "-fno-rtti", "-sDISABLE_EXCEPTION_CATCHING", "-std=c++17", "--bind", "--no-entry", "-sMODULARIZE", "-sNO_EXIT_RUNTIME=1", "-sWASM", "-sSTRICT=1", "-sUSE_WEBGL2=1", "-sMAX_WEBGL_VERSION=2", "-sALLOW_MEMORY_GROWTH" ] cc="/usr/local/share/dotnet/packs/Microsoft.NET.Runtime.Emscripten.3.1.56.Sdk.osx-arm64/9.0.4/tools/emscripten/emcc" cxx="/usr/local/share/dotnet/packs/Microsoft.NET.Runtime.Emscripten.3.1.56.Sdk.osx-arm64/9.0.4/tools/emscripten/em++" ar="/usr/local/share/dotnet/packs/Microsoft.NET.Runtime.Emscripten.3.1.56.Sdk.osx-arm64/9.0.4/tools/emscripten/emar"'
```

## MacOS
暂手工加 extra_cflags_cc=[ "-DSK_CODEC_ENCODES_JPEG", "-DSK_CODEC_DECODES_JPEG" ]

### arm64
```bash
bin/gn gen out/macos-arm64 --args='target_os="mac" target_cpu="arm64" is_official_build=true is_component_build=true skia_use_gl=false skia_use_vulkan=false skia_enable_tools=false skia_enable_pdf=true skia_pdf_subset_harfbuzz=true skia_use_zlib=true skia_use_icu=true skia_use_harfbuzz=true skia_use_metal=true skia_use_piex=true skia_use_system_icu=false skia_use_system_harfbuzz=false skia_use_system_expat=false skia_use_system_libjpeg_turbo=false skia_use_system_libpng=false skia_use_system_libwebp=false skia_use_system_zlib=false extra_cflags=[ "-DSKIA_C_DLL", "-DHAVE_ARC4RANDOM_BUF" ] extra_cflags_cc=[ "-DSK_CODEC_ENCODES_JPEG", "-DSK_CODEC_DECODES_JPEG" ] extra_ldflags=[ ] '

ninja -C out/macos-arm64 skia
```

//--target=arm64-apple-macos11

### x86_64
```bash
bin/gn gen out/macos-x86_64 --args='target_os="mac" target_cpu="x86_64" is_official_build=true is_component_build=true skia_use_gl=false skia_use_vulkan=false skia_enable_tools=false skia_enable_pdf=true skia_pdf_subset_harfbuzz=true skia_use_zlib=true skia_use_icu=true skia_use_harfbuzz=true skia_use_metal=true skia_use_piex=true skia_use_system_icu=false skia_use_system_harfbuzz=false skia_use_system_expat=false skia_use_system_libjpeg_turbo=false skia_use_system_libpng=false skia_use_system_libwebp=false skia_use_system_zlib=false extra_cflags=[ "-DSKIA_C_DLL", "-DHAVE_ARC4RANDOM_BUF", "--target=x86_64-apple-macos10.15" ] extra_ldflags=[ "--target=x86_64-apple-macos10.15" ] '
```

~~注意M1编译x86_64使用以下方式：
将上述命令建一个shell脚本~~

```bash
arch -x86_64 ./build.sh
arch -x86_64 ninja -C out/macos-x86_64 skia
```

## Windows

**注意: 命令行操作用Command不要用PowerShell**

* 禁用系统Python:

  Settings > Manage App Execution Aliases 关闭python installer

* 安装 depot_tools:

  https://commondatastorage.googleapis.com/chrome-infra-docs/flat/depot_tools/docs/html/depot_tools_tutorial.html#_setting_up
  安装完执行一下

```shell
gclient
```

* 安装 VisualStudio:

  arm需要2022 preview 2之后版本，且需要单独安装Visual C++ compilers and libraries for ARM64

* 编译

先同步skia第三方依赖:
```shell
python3 tools/git-sync-deps
```

找不到ninja
```shell
python3 bin\fetch-ninja
```

```bash
bin\gn gen out/win-arm64 --args="target_cpu=\"arm64\" is_official_build=true is_component_build=true skia_use_gl=false skia_use_metal=false skia_use_vulkan=false skia_use_angle=false skia_use_direct3d=true skia_enable_tools=false skia_use_xps=false skia_enable_pdf=true skia_pdf_subset_harfbuzz=true skia_use_zlib=true skia_use_libgrapheme=false skia_use_icu=true skia_use_harfbuzz=true skia_use_piex=false skia_use_dng_sdk=false skia_use_system_icu=false skia_use_system_harfbuzz=false skia_use_system_expat=false skia_use_system_libjpeg_turbo=false skia_use_system_libpng=false skia_use_system_libwebp=false skia_use_system_zlib=false skia_enable_fontmgr_win=true skia_enable_fontmgr_win_gdi=true extra_cflags=[\"-DSKIA_C_DLL\"]"

ninja -C out/win-arm64 skia
```

注意: skia_use_gl=true skia_use_angle=true