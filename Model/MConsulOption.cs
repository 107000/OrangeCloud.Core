namespace OrangeCloud.Core
{
    public class MConsulOption
    {
        /// <summary>
        /// consul 服务地址
        /// </summary>
        public string ConsulAddress { get; set; }

        /// <summary>
        /// consul鉴权token,暂未有
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 服务名
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务IP地址（为空则自动获取）
        /// </summary>
        public string ServiceIP { get; set; }

        /// <summary>
        /// 服务绑定端口
        /// </summary>
        public int ServicePort { get; set; }

        /// <summary>
        /// 健康检查地址
        /// </summary>
        public string ServiceHealthCheck { get; set; }
    }
}
