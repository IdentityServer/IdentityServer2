using System;

namespace SelfHostConsoleHost
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var server = new SelfHostServer();
            server.Start("https://renovator:7779/idsrv/");

            Console.WriteLine("thinktecture IdentityServer v2 running...");
            Console.ReadKey();

            server.Stop();
        }
    }
}
