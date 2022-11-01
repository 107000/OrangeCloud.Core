using System.Runtime.Serialization;

namespace OrangeCloud.Core
{
    [DataContract]
    public enum ETimeMode
    {
        [EnumMember]
        Minutes = 1,

        [EnumMember]
        Second = 2
    }
}
