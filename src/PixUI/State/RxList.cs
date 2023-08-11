using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PixUI;

// @ts-ignore for IList implements
public sealed class RxList<T> : State
#if !__WEB__
    , IList<T>
#endif
{
    public override bool Readonly { get; } = true;

    private readonly IList<T> _source;

    public RxList(IList<T> source)
    {
        _source = source;
    }

    public static implicit operator RxList<T>(Collection<T> value) => new RxList<T>(value);

    public static implicit operator RxList<T>(List<T> value) => new RxList<T>(value);

    public static implicit operator RxList<T>(T[] value) => new RxList<T>(value);

    #region ====IList====

#if !__WEB__
    public IEnumerator<T> GetEnumerator() => _source.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _source.GetEnumerator();

    public int Count => _source.Count;

    public bool IsReadOnly => _source.IsReadOnly;
        
    public void CopyTo(T[] array, int arrayIndex) => _source.CopyTo(array, arrayIndex);
#endif

    public void Add(T item)
    {
        _source.Add(item);
        NotifyValueChanged();
    }

    public bool Remove(T item)
    {
        var res = _source.Remove(item);
        if (res)
            NotifyValueChanged();
        return res;
    }

    public void Clear()
    {
        _source.Clear();
        NotifyValueChanged();
    }

    public bool Contains(T item) => _source.Contains(item);

    public int IndexOf(T item) => _source.IndexOf(item);

    public void Insert(int index, T item)
    {
        _source.Insert(index, item);
        NotifyValueChanged();
    }

    public void RemoveAt(int index)
    {
        _source.RemoveAt(index);
        NotifyValueChanged();
    }

    public T this[int index]
    {
        get => _source[index];
        set
        {
            _source[index] = value;
            NotifyValueChanged();
        }
    }

    #endregion
}