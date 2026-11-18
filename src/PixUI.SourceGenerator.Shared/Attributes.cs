namespace PixUI;

public static class Attributes
{
    public const string StylableAttribute =
        """
        using System;
        namespace PixUI;
        
        [AttributeUsage(AttributeTargets.Class)]
        public sealed class StylableAttribute<TStyle> : Attribute where TStyle : Style
        {
            public StylableAttribute() {}
        }
        """;
    
    public const string StyleObjectAttribute =
        """
        using System;
        namespace PixUI;

        [AttributeUsage(AttributeTargets.Class)]
        public sealed class StyleObjectAttribute : Attribute
        {
            public StyleObjectAttribute() {}
        }
        """;
    
    public const string StylePropertyAttribute = 
        """
        using System;
        namespace PixUI;

        /// <summary>
        /// 用于样式属性的代码生成
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)]
        public sealed class StylePropertyAttribute : Attribute
        {
            /// <summary>
            /// 样式属性
            /// </summary>
            /// <param name="defaultValue">默认值表达式, eg: "18f" or "Theme.FontSize"</param>
            /// <param name="defaultValueProxy">默认值是否指向其他位置, eg: "Theme.FontSize"</param>
            /// <param name="validator">值验证表达式, eg: "{v} &amp;gt; 0 &amp;&amp; {v} &amp;lt; 100"</param>
            /// <param name="listener">值变更监听表达式, eg: "Repaint" or "Relayout" or "Button.OnFontSizeChanged"</param>
            public StylePropertyAttribute(string? defaultValue = null, bool defaultValueProxy = false, string? validator = null,
                string? listener = null)
            {
                DefaultValue = defaultValue;
                Validator = validator;
                DefaultValueProxy = defaultValueProxy;
                Listener = listener;
            }
            
            public string? DefaultValue { get; set; }
            
            public bool DefaultValueProxy { get; set; }
            
            public string? Validator { get; set; }
            
            public string? Listener { get; set; }
        }
        """;
}