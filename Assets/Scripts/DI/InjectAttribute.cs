using System;

namespace UJ.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class InjectAttribute : Attribute
    {
        public string key { get; set; }
        public bool canNull { get; set; }

        public InjectAttribute(string key = "", bool canNull = false)
        {
            this.key = key;
            this.canNull = canNull;
        }
    }
}
