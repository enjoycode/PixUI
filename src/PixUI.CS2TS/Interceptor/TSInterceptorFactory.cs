using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace PixUI.CS2TS
{
    public static class TSInterceptorFactory
    {
        private static readonly Dictionary<string, ITSInterceptor> CustomInterceptors
            = new Dictionary<string, ITSInterceptor>();

        public static void RegisterCustomInterceptor(string name, ITSInterceptor interceptor)
        {
            CustomInterceptors.Add(name, interceptor);
        }

        internal static ITSInterceptor Make(AttributeData attribute)
        {
            var fullName = attribute.AttributeClass!.ToString();
            switch (fullName)
            {
                case Emitter.TSTemplateAttributeFullName:
                    var template = attribute.ConstructorArguments[0].Value!.ToString();
                    return new TSTemplateInterceptor(template);
                case Emitter.TSPropertyToGetSetAttributeFullName:
                    return TSPropertyToGetSetInterceptor.Default;
                case Emitter.TSIgnoreMethodInvokeAttributeFullName:
                    return TSIngnoreMethodInvokeInterceptor.Default;
                case Emitter.TSCustomInterceptorAttributeFullName:
                    var name = attribute.ConstructorArguments[0].Value!.ToString();
                    if (CustomInterceptors.TryGetValue(name, out var interceptor))
                        return interceptor;
                    throw new Exception("Can't find custom interceptor: " + name);
                default:
                    throw new NotImplementedException(fullName);
            }
        }
    }
}