namespace PixUI.Dynamic.Design;

public abstract class ValueEditorBase : SingleChildWidget
{
    protected ValueEditorBase(DesignElement element)
    {
        Element = element;
    }

    /// <summary>
    /// 当前编辑的属性所属的DesignElement
    /// </summary>
    public readonly DesignElement Element;
}