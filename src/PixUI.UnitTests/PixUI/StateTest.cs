using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace PixUI.UnitTests;

public class StateTest
{
    [Test]
    public void MakeEmptyTest()
    {
        State<int?> s = State<int?>.Default();
        Assert.True(!s.Value.HasValue);

        State<object?> s2 = State<int>.Default();
        Console.WriteLine(s2.Value);
    }

    [Test]
    public void RxProxyForINotifyPropertyChangedTest()
    {
        var person = new Person { Name = "Rick" };
        //var proxy = new RxProxy<string>(person, "Name");
        var proxy = person.Observe<string>("Name");
        var hasChanged = false;
        proxy.AddListener(s => hasChanged = true);
        
        //改变目标属性值
        person.Name = "Eric";
        Assert.True(hasChanged);
        Assert.True(proxy.Value == person.Name);

        //改变代理状态的值
        hasChanged = false;
        proxy.Value = "Rick";
        Assert.True(hasChanged);
        Assert.True(proxy.Value == person.Name);
    }
    
    public class Person: INotifyPropertyChanged
    {
        private string _name = string.Empty;

        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}