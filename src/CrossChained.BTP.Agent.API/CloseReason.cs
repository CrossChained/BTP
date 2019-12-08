using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrossChained.BTP.Agent.API
{
    public enum CloseReason
    {
        manual,
        liquidation,
        take_profit,
        stop_loss
    }
}
