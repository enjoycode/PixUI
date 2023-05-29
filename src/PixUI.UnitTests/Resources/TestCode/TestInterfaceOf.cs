using System;

namespace PixUI
{
    [TSInterfaceOf]
    interface INameable
    {
        public string Name { get; }
    }

    class Person: INameable
    {
        private string _name = "Rick";
        public string Name => _name;

        public static void Test()
        {
            var obj = new Person();
            if (obj is INameable nameable)
            {
                Console.Write(nameable.Name);
            }
        }
    }
}