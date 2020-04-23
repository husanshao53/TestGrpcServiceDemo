using GreetGrpcService;
using Grpc.Net.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestGrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {   

            //简单grpc调用
            {

                var httpClientHandler = new HttpClientHandler();
                // Return `true` to allow certificates that are untrusted/invalid
                httpClientHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                var httpClient = new HttpClient(httpClientHandler);


                var channel =  GrpcChannel.ForAddress("http://127.0.0.1:50051", new GrpcChannelOptions {HttpClient= httpClient });


                var client = new Greeter.GreeterClient(channel);
                var reply =await client.SayHelloAsync(
                    new HelloRequest { Name = "晓晨" });
                Console.WriteLine("Greeter 服务返回数据: " + reply.Message);
            }


            //双向流式grpc调用
            //{
            //    var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //    var catClient = new LuCat.LuCatClient(channel);
            //    //获取猫总数
            //    var catCount = await catClient.CountAsync(new Empty());
            //    Console.WriteLine($"一共{catCount.Count}只猫。");

            //    var rand = new Random(DateTime.Now.Millisecond);

            //    var bathCat = catClient.BathTheCat();

            //    //定义接收吸猫响应逻辑
            //    var bathCatRespTask = Task.Run(async () =>
            //    {
            //        await foreach (var resp in bathCat.ResponseStream.ReadAllAsync())
            //        {
            //            Console.WriteLine(resp.Message);
            //        }
            //    });


            //    //随机给10个猫洗澡
            //    for (int i = 0; i < 10; i++)
            //    {
            //        await bathCat.RequestStream.WriteAsync(new BathTheCatReq() { Id = rand.Next(0, catCount.Count) });
            //    }


            //    //发送完毕
            //    await bathCat.RequestStream.CompleteAsync();


            //    Console.WriteLine("客户端已发送完10个需要洗澡的猫id");
            //    Console.WriteLine("接收洗澡结果：");
            //    //开始接收响应
            //    await bathCatRespTask;

            //    Console.WriteLine("洗澡完毕");

            //}

            Console.ReadKey();
        }
    }
}
