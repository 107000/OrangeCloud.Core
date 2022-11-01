using System;
using System.Collections.Generic;
using System.Text;

namespace OrangeCloud.Core
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class KeyAttribute : Attribute
    {
        private readonly bool _IsIncrement = true;

        public KeyAttribute(bool IsIncrement)
        {
            this._IsIncrement = IsIncrement;
        }

        public bool IsIncrement
        {
            get { return this._IsIncrement; }
        }
    }
}
