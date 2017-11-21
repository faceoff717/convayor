using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Convayor.Abstractions;

namespace Convayor
{    
    public abstract class Transition<TAction> : ITransition<TAction>
    {
        private IEnumerable<Action> _actions;
        private TAction _action;        
        private MachineState<TAction> _toState;

        public Transition(TAction action, MachineState<TAction> toState, IEnumerable<Action> jobs)
        {
            _actions = jobs;
            _action = action;            
            _toState = toState;
        }

        public Transition(TAction action, MachineState<TAction> toState, Action job)
        {
            if (action != null)
                _actions = new[] { job };

            _action = action;            
            _toState = toState;
        }

        public Transition(TAction action, MachineState<TAction> toState)
        {
            if (action != null)
                _actions = new List<Action>();

            _action = action;            
            _toState = toState;
        }
        
        TAction ITransition<TAction>.Action
        {
            get { return _action; }
        }
        
        IMashineState<TAction> ITransition<TAction>.Do()
        {
            if (_actions != null)
            {
                foreach (var action in _actions)
                {
                    action?.Invoke();
                }
            }

            return _toState;
        }
    }
}
