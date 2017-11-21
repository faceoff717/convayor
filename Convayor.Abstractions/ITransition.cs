using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convayor.Abstractions
{    
    public interface ITransition<TAction>
    {
        TAction Action { get; }                
        IMashineState<TAction> Do();
    }
}
