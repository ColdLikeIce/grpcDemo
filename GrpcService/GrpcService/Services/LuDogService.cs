using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrpcService.Services
{
    public class LuDogService : LuDog.LuDogBase
    {
        private static readonly List<string> Cats = new List<string>() { "二哈", "傻狗" };
        private static readonly Random Rand = new Random(DateTime.Now.Millisecond);

        public override Task<SuckingDogResult> SuckingDog(SuckingDogRequest request, ServerCallContext context)
        {
            return Task.FromResult(new SuckingDogResult()
            {
                Message = $"{request.Name}吸了一只{Cats[Rand.Next(0, Cats.Count)]}"
            });
        }
    }
}