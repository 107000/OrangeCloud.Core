using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Linq;
using System.Threading;

namespace OrangeCloud.Core
{
    public static class RegisterToConsulExtension
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IApplicationLifetime lifetime, MConsulOption consulOption)
        {
            string globalguid = Guid.NewGuid().ToString("N");

            #region 自动注册服务

            lifetime.ApplicationStarted.Register(() =>

                {

                    using (var consulClient = new ConsulClient(c =>
                    {
                        c.Address = new Uri(consulOption.ConsulAddress);

                        c.Token = consulOption.Token;
                    }))
                    {
                        int times = 0;

                        while (times++ < 10)
                        {
                            try
                            {
                                var ipaddress = consulOption.ServiceIP;
                                if(string.IsNullOrWhiteSpace(ipaddress))
                                {
                                    ipaddress = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                                    .Select(p => p.GetIPProperties())
                                    .SelectMany(p => p.UnicastAddresses)
                                    .Where(p => p.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !System.Net.IPAddress.IsLoopback(p.Address))
                                    .Select(p => p.Address.ToString()).ToArray().First();
                                }
                                AgentServiceRegistration asr = new AgentServiceRegistration
                                {
                                    Address = ipaddress,
                                    Port = consulOption.ServicePort,
                                    ID = globalguid,
                                    Name = consulOption.ServiceName,
                                    Check = new AgentServiceCheck
                                    {
                                        DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(120),
                                        HTTP = $"http://{ipaddress}:{consulOption.ServicePort}/Health",
                                        Interval = TimeSpan.FromSeconds(30),
                                        Timeout = TimeSpan.FromSeconds(10),
                                    },
                                };

                                var task = consulClient.Agent.ServiceRegister(asr);

                                task.Wait();

                                Console.WriteLine($"服务注册结果：{task.Result.StatusCode.ToString()}");

                                if (task.Result.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"服务注册失败:{ex.ToString()}");
                            }

                            Thread.Sleep(times * 500);
                        }
                    }
                });

            //程序退出注销服务
            lifetime.ApplicationStopping.Register(() =>
            {
                using (var consulClient = new ConsulClient(c =>
                {
                    c.Address = new Uri(consulOption.ConsulAddress);

                    c.Token = consulOption.Token;
                }))
                {
                    Console.WriteLine("应用退出，开始从consul注销");

                    consulClient.Agent.ServiceDeregister(globalguid).Wait();
                }
            });

            #endregion

            //var consulClient = new ConsulClient(x =>
            //{
            //    // consul 服务地址
            //    x.Address = new Uri(consulOption.Address);
            //});

            //var registration = new AgentServiceRegistration()
            //{
            //    ID = Guid.NewGuid().ToString(),
            //    Name = consulOption.ServiceName,// 服务名
            //    Address = consulOption.ServiceIP, // 服务绑定IP
            //    Port = consulOption.ServicePort, // 服务绑定端口
            //    Check = new AgentServiceCheck()
            //    {
            //        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
            //        Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔
            //        HTTP = consulOption.ServiceHealthCheck,//健康检查地址
            //        Timeout = TimeSpan.FromSeconds(5)
            //    }
            //};

            //// 服务注册
            //consulClient.Agent.ServiceRegister(registration).Wait();

            //// 应用程序终止时，服务取消注册
            //lifetime.ApplicationStopping.Register(() =>
            //{
            //    consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            //});
            return app;
        }
    }
}
