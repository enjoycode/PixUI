using System;

namespace PixUI
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method |
                    AttributeTargets.Delegate | AttributeTargets.Class | AttributeTargets.Struct |
                    AttributeTargets.Interface)]
    public sealed class TSRenameAttribute : Attribute
    {
        public TSRenameAttribute(string newName) { }
    }
}