using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrpcService.Services
{
    public class LuCatService : LuCat.LuCatBase
    {
        private static readonly List<string> Cats = new List<string>() { "英短银渐层", "英短金渐层", "美短", "蓝猫", "狸花猫", "橘猫" };
        private static readonly Random Rand = new Random(DateTime.Now.Millisecond);

        public override Task<SuckingCatResult> SuckingCat(SuckingCatRequest request, ServerCallContext context)
        {
            return Task.FromResult(new SuckingCatResult()
            {
                Message = $"{request.Name}吸了一只{Cats[Rand.Next(0, Cats.Count)]}"
            });
        }
    }
}