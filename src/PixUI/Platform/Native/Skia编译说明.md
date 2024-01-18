https://skia.org/docs/user/build/

# 修改BUILD.gn包含SkParagraph及相关C-Api
在/BUILD.gn的skia_component("skia")最后添加
```text
# ====Rick add for SkiaUI====
  sources += [
    "include/c/sk_bitmap.h",
    "include/c/sk_canvas.h",
    "include/c/sk_codec.h",
    "include/c/sk_colorfilter.h",
    "include/c/sk_colorspace.h",
    "include/c/sk_colortable.h",
    "include/c/sk_data.h",
    "include/c/sk_document.h",
    "include/c/sk_drawable.h",
    "include/c/sk_font.h",
    "include/c/sk_general.h",
    "include/c/sk_graphics.h",
    "include/c/sk_image.h",
    "include/c/sk_imagefilter.h",
    "include/c/sk_mask.h",
    "include/c/sk_maskfilter.h",
    "include/c/sk_matrix.h",
    "include/c/sk_paint.h",
    "include/c/sk_path.h",
    "include/c/sk_patheffect.h",
    "include/c/sk_picture.h",
    "include/c/sk_pixmap.h",
    "include/c/sk_region.h",
    "include/c/sk_rrect.h",
    "include/c/sk_runtimeeffect.h",
    "include/c/sk_shader.h",
    "include/c/sk_stream.h",
    "include/c/sk_string.h",
    "include/c/sk_surface.h",
    "include/c/sk_svg.h",
    "include/c/sk_textblob.h",
    "include/c/sk_typeface.h",
    "include/c/sk_types.h",
    "include/c/sk_vertices.h",
    "include/c/sk_xml.h",
    "include/c/gr_context.h",
    "src/c/sk_bitmap.cpp",
    "src/c/sk_canvas.cpp",
    "src/c/sk_codec.cpp",
    "src/c/sk_colorfilter.cpp",
    "src/c/sk_colorspace.cpp",
    "src/c/sk_colortable.cpp",
    "src/c/sk_data.cpp",
    "src/c/sk_document.cpp",
    "src/c/sk_drawable.cpp",
    "src/c/sk_enums.cpp",
    "src/c/sk_font.cpp",
    "src/c/sk_general.cpp",
    "src/c/sk_graphics.cpp",
    "src/c/sk_image.cpp",
    "src/c/sk_imagefilter.cpp",
    "src/c/sk_mask.cpp",
    "src/c/sk_maskfilter.cpp",
    "src/c/sk_matrix.cpp",
    "src/c/sk_paint.cpp",
    "src/c/sk_path.cpp",
    "src/c/sk_patheffect.cpp",
    "src/c/sk_picture.cpp",
    "src/c/sk_pixmap.cpp",
    "src/c/sk_region.cpp",
    "src/c/sk_rrect.cpp",
    "src/c/sk_runtimeeffect.cpp",
    "src/c/sk_shader.cpp",
    "src/c/sk_stream.cpp",
    "src/c/sk_string.cpp",
    "src/c/sk_structs.cpp",
    "src/c/sk_surface.cpp",
    "src/c/sk_svg.cpp",
    "src/c/sk_textblob.cpp",
    "src/c/sk_typeface.cpp",
    "src/c/sk_types_priv.h",
    "src/c/sk_vertices.cpp",
    "src/c/sk_xml.cpp",
    "src/c/gr_context.cpp"
  ]

  if (is_wasm) {
    deps += [
      "//third_party/icu:dotnet",
      "//third_party/harfbuzz",
    ]
  } else {
    deps += [
      "//third_party/icu",
      "//third_party/harfbuzz",
    ]
  }

  defines += [ "SKUNICODE_IMPLEMENTATION=1", "SKSHAPER_IMPLEMENTATION=1" ]
  if (skia_use_icu) {
    defines += [ "SK_UNICODE_ICU_IMPLEMENTATION" ]
  }
  if (skia_use_fonthost_mac) {
    defines += [ "SK_SHAPER_CORETEXT_AVAILABLE" ]
  }
  if (skia_use_icu && skia_use_harfbuzz) {
    defines += [ "SK_UNICODE_AVAILABLE", "SK_SHAPER_HARFBUZZ_AVAILABLE" ]
  }

  skia_unicode_public = [ "modules/skunicode/include/SkUnicode.h" ]
  skia_unicode_sources = [
    "modules/skunicode/src/SkUnicode.cpp",
    "modules/skunicode/src/SkUnicode_icu.cpp",
    "modules/skunicode/src/SkUnicode_icu.h",
    "modules/skunicode/src/SkUnicode_icu_bidi.cpp",
    "modules/skunicode/src/SkUnicode_icu_bidi.h"
  ]
  skia_unicode_builtin_icu_sources = [ "modules/skunicode/src/SkUnicode_icu_builtin.cpp" ]
  public += skia_unicode_public
  sources += skia_unicode_sources
  sources += skia_unicode_builtin_icu_sources


  skia_shaper_public = [ "modules/skshaper/include/SkShaper.h" ]
  public += skia_shaper_public
  skia_shaper_primitive_sources = [
    "modules/skshaper/src/SkShaper.cpp",
    "modules/skshaper/src/SkShaper_primitive.cpp",
  ]
  skia_shaper_harfbuzz_sources = [ "modules/skshaper/src/SkShaper_harfbuzz.cpp" ]
  
  sources += skia_shaper_primitive_sources
  if (skia_use_fonthost_mac) {
    skia_shaper_coretext_sources = [ "modules/skshaper/src/SkShaper_coretext.cpp" ]
    sources += skia_shaper_coretext_sources
  }
  if (skia_use_icu && skia_use_harfbuzz) {
    sources += skia_shaper_harfbuzz_sources
  }

  skia_paragraph_public = [
    "include/ParagraphStyle.h",
    "include/TextStyle.h",
    "include/TextShadow.h",
    "include/FontArguments.h",
    "include/FontCollection.h",
    "include/Paragraph.h",
    "include/ParagraphBuilder.h",
    "include/ParagraphCache.h",
    "include/DartTypes.h",
    "include/TypefaceFontProvider.h",
    "include/Metrics.h",
    "include/ParagraphPainter.h"
  ]

  skia_paragraph_sources = [
    "include/c/sk_paragraph.h",
    "src/c/sk_paragraph.cpp",
    "modules/skparagraph/src/Decorations.cpp",
    "modules/skparagraph/src/Decorations.h",
    "modules/skparagraph/src/FontArguments.cpp",
    "modules/skparagraph/src/FontCollection.cpp",
    "modules/skparagraph/src/Iterators.h",
    "modules/skparagraph/src/OneLineShaper.cpp",
    "modules/skparagraph/src/OneLineShaper.h",
    "modules/skparagraph/src/ParagraphBuilderImpl.cpp",
    "modules/skparagraph/src/ParagraphBuilderImpl.h",
    "modules/skparagraph/src/ParagraphCache.cpp",
    "modules/skparagraph/src/ParagraphImpl.cpp",
    "modules/skparagraph/src/ParagraphImpl.h",
    "modules/skparagraph/src/ParagraphPainterImpl.cpp",
    "modules/skparagraph/src/ParagraphPainterImpl.h",
    "modules/skparagraph/src/ParagraphStyle.cpp",
    "modules/skparagraph/src/Run.cpp",
    "modules/skparagraph/src/Run.h",
    "modules/skparagraph/src/TextLine.cpp",
    "modules/skparagraph/src/TextLine.h",
    "modules/skparagraph/src/TextShadow.cpp",
    "modules/skparagraph/src/TextStyle.cpp",
    "modules/skparagraph/src/TextWrapper.cpp",
    "modules/skparagraph/src/TextWrapper.h",
    "modules/skparagraph/src/TypefaceFontProvider.cpp",
  ]
  
  public += skia_paragraph_public
  sources += skia_paragraph_sources

  # clear the sources and add explicitly
  # set_sources_assignment_filter([])
  sources += [
    "src/xamarin/sk_compatpaint.cpp",
    "src/xamarin/sk_manageddrawable.cpp",
    "src/xamarin/sk_managedstream.cpp",
    "src/xamarin/sk_managedtracememorydump.cpp",
    "src/xamarin/sk_xamarin.cpp",
    "src/xamarin/SkiaKeeper.c",
    "src/xamarin/SkCompatPaint.cpp",
    "src/xamarin/SkManagedDrawable.cpp",
    "src/xamarin/SkManagedStream.cpp",
    "src/xamarin/SkManagedTraceMemoryDump.cpp",
    # "src/xamarin/WinRTCompat.cpp",
  ]
  # ====End add for SkiaUI====
```

在third_parth/icu/BUILD.gn添加
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

### 先编译dotnet版本的icu

1. clone dotnet icu
```bash
git clone -b dotnet/release/8.0 https://github.com/dotnet/icu.git
```

2. 修改icu-filters/icudt_wasm.json
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

3. 设置编译环境
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

4. 回到icu目录开始编译
```bash
./build.sh /p:TargetOS=Browser /p:TargetArchitecture=wasm /p:IcuTracing=true
```

### 再编译skia

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

// skia_enable_fontmgr_custom_empty=true skia_enable_fontmgr_custom_embedded=false
```bash
bin/gn gen out/wasm --args='target_os="linux" target_cpu="wasm" is_official_build=true is_component_build=false skia_enable_tools=false skia_enable_gpu=true skia_use_webgl=true skia_use_webgpu=false skia_gl_standard="webgl" skia_enable_ganesh=true skia_use_angle=false skia_use_vulkan=false skia_use_metal=false skia_use_dng_sdk=false skia_use_lua=false skia_enable_pdf=false skia_pdf_subset_harfbuzz=false skia_use_xps=false skia_use_zlib=true skia_use_icu=true skia_use_harfbuzz=true skia_use_piex=false skia_use_sfntly=false skia_use_system_icu=false skia_use_system_harfbuzz=false skia_use_system_libjpeg_turbo=false skia_use_system_libpng=false skia_use_system_libwebp=false skia_use_system_zlib=true skia_enable_fontmgr_custom_directory=false skia_enable_fontmgr_custom_empty=true skia_enable_fontmgr_custom_embedded=false skia_enable_fontmgr_empty=false extra_cflags=[ "-msimd128", "-sSUPPORT_LONGJMP=wasm", "-fwasm-exceptions", "-I/opt/homebrew/opt/zlib/include", "-DSKIA_C_DLL", "-DSK_SUPPORT_GPU=1", "-DSK_GL", "-DCK_ENABLE_WEBGL", "-DSK_GANESH", "-DSK_DISABLE_LEGACY_SHADERCONTEXT", "-DXML_POOR_ENTROPY", "-DSK_FORCE_8_BYTE_ALIGNMENT", "-DEMSCRIPTEN_HAS_UNBOUND_TYPE_NAMES=0", "-DSK_DISABLE_AAA", "-DGR_GL_CHECK_ALLOC_WITH_GET_ERROR=0", "-DFT_CONFIG_OPTION_SYSTEM_ZLIB" ] extra_cflags_cc=[ "-msimd128", "-fwasm-exceptions" ] extra_ldflags=[ "-fno-rtti", "-fwasm-exceptions", "-sSUPPORT_LONGJMP=wasm", "-sDISABLE_EXCEPTION_CATCHING", "-std=c++17", "--bind", "--no-entry", "-sMODULARIZE", "-sNO_EXIT_RUNTIME=1", "-sWASM", "-sSTRICT=1", "-sUSE_WEBGL2=1", "-sMAX_WEBGL_VERSION=2", "-sALLOW_MEMORY_GROWTH" ] cc="/usr/local/share/dotnet/packs/Microsoft.NET.Runtime.Emscripten.3.1.34.Sdk.osx-arm64/8.0.1/tools/emscripten/emcc" cxx="/usr/local/share/dotnet/packs/Microsoft.NET.Runtime.Emscripten.3.1.34.Sdk.osx-arm64/8.0.1/tools/emscripten/em++" ar="/usr/local/share/dotnet/packs/Microsoft.NET.Runtime.Emscripten.3.1.34.Sdk.osx-arm64/8.0.1/tools/emscripten/emar"'

ninja -C out/wasm skia
```

## MacOS

### arm64
```bash
bin/gn gen out/macos-arm64 --args='target_os="mac" target_cpu="arm64" is_official_build=true is_component_build=true skia_use_gl=false skia_use_vulkan=false skia_enable_tools=false skia_enable_pdf=true skia_pdf_subset_harfbuzz=true skia_use_zlib=true skia_use_icu=true skia_use_harfbuzz=true skia_use_metal=true skia_use_piex=true skia_use_sfntly=false skia_use_system_icu=false skia_use_system_harfbuzz=false skia_use_system_expat=false skia_use_system_libjpeg_turbo=false skia_use_system_libpng=false skia_use_system_libwebp=false skia_use_system_zlib=false extra_cflags=[ "-DSKIA_C_DLL", "-DHAVE_ARC4RANDOM_BUF", "-stdlib=libc++" ] extra_ldflags=[ "-stdlib=libc++" ] '

ninja -C out/macos-arm64 skia
```

//--target=arm64-apple-macos11

### x86_64
```bash
bin/gn gen out/macos-x86_64 --args='target_os="mac" target_cpu="x86_64" is_official_build=true is_component_build=true skia_use_gl=false skia_use_vulkan=false skia_enable_tools=false skia_enable_pdf=true skia_pdf_subset_harfbuzz=true skia_use_zlib=true skia_use_icu=true skia_use_harfbuzz=true skia_use_metal=true skia_use_piex=true skia_use_sfntly=false skia_use_system_icu=false skia_use_system_harfbuzz=false skia_use_system_expat=false skia_use_system_libjpeg_turbo=false skia_use_system_libpng=false skia_use_system_libwebp=false skia_use_system_zlib=false extra_cflags=[ "-DSKIA_C_DLL", "-DHAVE_ARC4RANDOM_BUF", "-stdlib=libc++", "--target=x86_64-apple-macos10.15" ] extra_ldflags=[ "-stdlib=libc++", "--target=x86_64-apple-macos10.15" ] '
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
bin\gn gen out/win-arm64 --args="target_cpu=\"arm64\" is_official_build=true is_component_build=true skia_use_gl=true skia_use_metal=false skia_use_vulkan=false skia_use_angle=true skia_use_direct3d=true skia_enable_tools=false skia_use_xps=false skia_enable_pdf=true skia_pdf_subset_harfbuzz=true skia_use_zlib=true skia_use_icu=true skia_use_harfbuzz=true skia_use_piex=false skia_use_dng_sdk=false skia_use_sfntly=false skia_use_system_icu=false skia_use_system_harfbuzz=false skia_use_system_expat=false skia_use_system_libjpeg_turbo=false skia_use_system_libpng=false skia_use_system_libwebp=false skia_use_system_zlib=false skia_enable_fontmgr_win=true skia_enable_fontmgr_win_gdi=true extra_cflags=[\"-DSKIA_C_DLL\"]"

ninja -C out/win-arm64 skia
```

注意: skia_use_gl=true skia_use_angle=true