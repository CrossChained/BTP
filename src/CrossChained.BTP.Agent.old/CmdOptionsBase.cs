using System;

namespace CrossChained.BTP.Agent
{
    public abstract class CmdOptionsBase
    {
        public abstract int Run(IServiceProvider sp);
    }
}