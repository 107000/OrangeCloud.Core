using System.Runtime.Serialization;

namespace OrangeCloud.Core
{
    [DataContract]
    public enum ECacheMode
    {
        [EnumMember]
        Absolute = 1,

        [EnumMember]
        Sliding = 2
    }
}
