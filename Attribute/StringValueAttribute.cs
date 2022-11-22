using System;

namespace OrangeCloud.Core
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class StringValueAttribute : Attribute
    {
        private readonly bool _IsStringValue = true;

        public StringValueAttribute(bool IsStringValue = true)
        {
            this._IsStringValue = IsStringValue;
        }

        public bool IsStringValue
        {
            get { return this._IsStringValue; }
        }
    }
}
