using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrpcService.Services
{
    public class LuPigService : LuPig.LuPigBase
    {
        private static readonly List<string> Cats = new List<string>() { "傻猪", "黑猪" };
        private static readonly Random Rand = new Random(DateTime.Now.Millisecond);

        public override Task<SuckingPigResult> SuckingPig(SuckingPigRequest request, ServerCallContext context)
        {
            return Task.FromResult(new SuckingPigResult()
            {
                Message = $"{request.Name}吸了一只{Cats[Rand.Next(0, Cats.Count)]}"
            });
        }
    }
}