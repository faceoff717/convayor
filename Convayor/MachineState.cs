using Convayor.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Convayor
{
    public abstract class MachineState<TAction> : IMashineState<TAction>, IMachineStateQueryable<TAction>
    {        
        private IEnumerable<ITransition<TAction>> _transitions = new ITransition<TAction>[0];
        private ITransition<TAction> _defaultTransition;

        public MachineState(IEnumerable<ITransition<TAction>> transitions, ITransition<TAction> defaultTransition = null)
        {            
            _transitions = transitions;
            _defaultTransition = defaultTransition;
            CheckTransitionsValidity();
        }
        
        public MachineState(ITransition<TAction> transition, ITransition<TAction> defaultTransition = null)
        {
            _transitions = new[] {transition};
            _defaultTransition = defaultTransition;
            CheckTransitionsValidity();
        }

        public MachineState(IEnumerable<TAction> actions)
        {
            CheckTransitionsValidity();
        }

        public MachineState(TAction action)
        {
            CheckTransitionsValidity();
        }

        public MachineState()
        {
            CheckTransitionsValidity();
        }

        public IEnumerable<TAction> Actions { get; }

        public ITransition<TAction> DefaultTransition
        {
            get { return _defaultTransition; }
            set { _defaultTransition = value; }
        }

        public IMashineState<TAction> SetTransition(ITransition<TAction> transition)
        {
            _transitions = new[] {transition};
            CheckTransitionsValidity();

            return this;
        }

        public IMashineState<TAction> SetTransitions(IEnumerable<ITransition<TAction>> transitions)
        {
            _transitions = transitions;
            CheckTransitionsValidity();

            return this;
        }

        public IMashineState<TAction> AddTransition(ITransition<TAction> transition)
        {
            var tActions = new List<ITransition<TAction>>();
            tActions.Add(transition);
            tActions.AddRange(_transitions);
            _transitions = tActions;
            CheckTransitionsValidity();            

            return this;
        }

        public IMashineState<TAction> AddTransitions(IEnumerable<ITransition<TAction>> transitions)
        {
            var tActions = new List<ITransition<TAction>>();
            tActions.AddRange(transitions);
            tActions.AddRange(_transitions);
            _transitions = tActions;
            CheckTransitionsValidity();

            return this;            
        }

        public IMashineState<TAction> Do(TAction action)
        {
            var transition = _transitions.FirstOrDefault(x => x.Action.Equals(action));

            if (transition != null)
                return transition.Do();

            if (DefaultTransition != null)
                return DefaultTransition.Do();
            
            throw new TransitionDoesNotExisistException();
        }

        public bool CanTransit(TAction action)
        {
            return _transitions.Any(x => x.Action.Equals(action));
        }

        private void CheckTransitionsValidity()
        {
            if (_transitions != null && _transitions.Any())
            {
                if (_transitions.GroupBy(a => a.Action).Any(g => g.Count() > 1))
                    throw new DuplicateActionException();
            }        
        }
    }
}
