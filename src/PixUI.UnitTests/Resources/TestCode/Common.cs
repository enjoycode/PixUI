using System.Collections;
using System.Collections.Generic;

namespace PixUI
{
    public abstract class State<T> { }
    
    public class MultiChildWidget : IEnumerable<object>
    {
        private readonly List<object> _list = new List<object>();
        public void Add(object item) {}
        
        public IEnumerator<object> GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
    }
}