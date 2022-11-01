using System;
using System.Collections.Generic;

namespace OrangeCloud.Core
{
    [Serializable]
    public class MPageData<T>
    {
        /// <summary>
		/// 数据总数
		/// </summary>
		public int TotalNumber { get; set; }

        /// <summary>
        /// 分页后的数据列表
        /// </summary>
        public IList<T> Data { get; set; }
    }
}
