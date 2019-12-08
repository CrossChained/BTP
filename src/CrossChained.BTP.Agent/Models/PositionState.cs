using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrossChained.BTP.Agent.Models
{
    public enum PositionState
    {
        proposed,
        opened,
        closed,
        history,
        canceled
    }
}
