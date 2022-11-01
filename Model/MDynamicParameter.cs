using System.Data;

namespace OrangeCloud.Core
{
    public class MDynamicParameter
    {
        public string name { get; set; }

        public object value { get; set; }

        public DbType? dbType { get; set; }

        public ParameterDirection? direction { get; set; }

        public int? size { get; set; }

        public byte? precision { get; set; }

        public byte? scyle { get; set; }
    }
}
