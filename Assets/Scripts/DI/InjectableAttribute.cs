using System;

namespace UJ.DI
{
    [AttributeUsage(AttributeTargets.Class)]
    public class InjectableAttribute : Attribute
    {
        public string key { get; set; }
        public bool canNull { get; set; }

        public InjectableAttribute(string key = "", bool canNull = false)
        {
            this.key = key;
            this.canNull = canNull;
        }
    }
}
