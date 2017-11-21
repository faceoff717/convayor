using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convayor.Abstractions
{
    public interface IMachineStateQueryable<in TAction>
    {
        bool CanTransit(TAction action);
    }
}
