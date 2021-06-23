using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace GrpcGreeterClient
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            await MainTest2();
            await MainTest();
            await MainTestPig();
        }

        private static async Task MainTest()
        {
            using (var channel = GrpcChannel.ForAddress("https://localhost:5001"))
            {
                var client = new LuDog.LuDogClient(channel);
                var reply = client.SuckingDog(new SuckingDogRequest { Name = "小明" });
                Console.WriteLine(reply);
            }
        }

        private static async Task MainTestPig()
        {
            using (var channel = GrpcChannel.ForAddress("https://localhost:5001"))
            {
                var client = new LuPig.LuPigClient(channel);
                var reply = client.SuckingPig(new SuckingPigRequest { Name = "小猪" });
                Console.WriteLine(reply);
                Console.ReadKey();
            }
        }

        private static async Task MainTest2()
        {
            using (var channel = GrpcChannel.ForAddress("https://localhost:5001"))
            {
                var client = new LuCat.LuCatClient(channel);
                var reply = client.SuckingCat(new SuckingCatRequest { Name = "小红" });
                Console.WriteLine(reply);
            }
        }
    }
}