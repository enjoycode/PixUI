using System;
using System.Collections.Generic;

namespace PixUI.CS2TS
{
    /// <summary>
    /// 用于转换AppBox时的一些回调
    /// </summary>
    public sealed class AppBoxContext
    {
        public AppBoxContext(Func<string, string?> findModelId, Func<string, string, short> findEntityMemberId,
            bool forPreview, int sessionId
#if DEBUG
            , bool forViteDev = false
#endif
        )
        {
            FindModelId = findModelId;
            FindModel = fullName => FindModelId(fullName) != null;
            FindEntityMemberId = findEntityMemberId;
            ForPreview = forPreview;
            SessionId = sessionId;
#if DEBUG
            ForViteDev = forViteDev;
#endif
        }

        /// <summary>
        /// 用于跟踪使用到的模型
        /// </summary>
        internal readonly Func<string, bool> FindModel;

        internal readonly Func<string, string?> FindModelId;

        /// <summary>
        /// 用于查找实体成员的标识
        /// </summary>
        internal readonly Func<string, string, short> FindEntityMemberId;

        internal readonly bool ForPreview;

        internal readonly int SessionId;

#if DEBUG
        internal readonly bool ForViteDev;
#endif
        
        internal Action<string>? AddUsedModelInterceptor;
        
        // 使用到的模型，用于生成文件头import
        public readonly HashSet<string> UsedModels = new ();
        
        /// <summary>
        /// 添加使用到的模型
        /// </summary>
        internal void AddUsedModel(string modelFullName)
        {
            //先判断是否new Route()时被拦截
            if (AddUsedModelInterceptor != null)
            {
                AddUsedModelInterceptor(modelFullName);
                return;
            }
            
            if (!UsedModels.Contains(modelFullName))
                UsedModels.Add(modelFullName);
        }
    }
}