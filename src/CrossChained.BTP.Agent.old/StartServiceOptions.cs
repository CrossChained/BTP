using CommandLine;
using System;

namespace CrossChained.BTP.Agent
{
    [Verb("start", HelpText = "Start agent.")]
    public class StartServiceOptions : CmdOptionsBase
    {
        [Option('l', "login", Required = true, HelpText = "User Login")]
        public string login { get; set; }

        [Option('p', "password", Required = true, HelpText = "User Password")]
        public string password { get; set; }

        public override int Run(IServiceProvider sp)
        {
            throw new NotImplementedException();
        }
    }
}