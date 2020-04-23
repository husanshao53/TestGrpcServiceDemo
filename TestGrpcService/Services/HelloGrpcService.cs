
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using HelloGrpcService;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace TestGrpcService.Services
{

    public class HelloGrpcService: LuCat.LuCatBase
    {
        private static readonly List<string> Cats = new List<string>() { "英短银渐层", "英短金渐层", "美短", "蓝猫", "狸花猫", "橘猫" };
        private static readonly Random Rand = new Random(DateTime.Now.Millisecond);
        private readonly ILogger<HelloGrpcService> _logger;
        public HelloGrpcService(ILogger<HelloGrpcService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 双向流式grpc处理
        /// </summary>
        /// <param name="requestStream"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task BathTheCat(
            IAsyncStreamReader<BathTheCatReq> requestStream, 
            IServerStreamWriter<BathTheCatResp> responseStream,
            ServerCallContext context)
        {
            var bathQueue = new Queue<int>();
            while (await requestStream.MoveNext())
            {
                //将要洗澡的猫加入队列
                bathQueue.Enqueue(requestStream.Current.Id);

                _logger.LogInformation($"Cat {requestStream.Current.Id} Enqueue.");
            }

            //遍历队列开始洗澡
            while (bathQueue.TryDequeue(out var catId))
            {
                await responseStream.WriteAsync(new BathTheCatResp() { Message = $"铲屎的成功给一只{Cats[catId]}洗了澡！" });

                await Task.Delay(500);    //此处主要是为了方便客户端能看出流调用的效果
            }
        }

        public override Task<CountCatResult> Count(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new CountCatResult()
            {
                Count = Cats.Count
            });
        }
    }
}
