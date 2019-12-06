using System;
using CommandLine;
using CrossChained.RAFT;
using CrossChained.RAFT.WebSockets;
using Microsoft.Extensions.DependencyInjection;

namespace CrossChained.BTP.Agent
{
    public class Program
    {
        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<StartServiceOptions>(args)
                .MapResult(
                  (StartServiceOptions opts) => RunAddAndReturnExitCode(opts),
                  errs => 1);
        }
        public static int RunAddAndReturnExitCode(StartServiceOptions opts)
        {
            return RunWithDI<StartServiceOptions>(opts);
        }

        private static int RunWithDI<T>(T opts)
            where T : CmdOptionsBase
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            using (var sp = services.BuildServiceProvider())
            {
                Configure(sp);

                return opts.Run(sp);
            }
        }
        public static void ConfigureServices(ServiceCollection services)
        {
            services.UseRaft();
            services.UseRaftWebSockets();
        }

        public static void Configure(IServiceProvider sp)
        {
        }
    }
}
