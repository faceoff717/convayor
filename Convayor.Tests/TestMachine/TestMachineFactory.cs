using Convayor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Convayor.Abstractions;

namespace Convayor.Tests.TestMachine
{
    public enum TestMachineState
    {
        State1 = 1,
        State2 = 2,
        State3 = 3
    }

    public enum TestMachineStateTransition
    {
        State_1_1 = 0,
        State_1_2 = 1,
        State_2_3 = 2,
        State_1_3 = 3,
        State_3_1 = 4
    }

    public class MachineStateTest : MachineState<TestMachineStateTransition>
    {
        //public MachineStateTest(TestMachineState state, IEnumerable<ITransition<TestMachineState, TestMachineStateTransition>> transitions, bool throwOnMissedAction = false) : base(state, transitions, throwOnMissedAction)
        //{
        //}

        //public MachineStateTest(TestMachineState state, ITransition<TestMachineState, TestMachineStateTransition> transition, bool throwOnMissedAction = false) : base(state, transition, throwOnMissedAction)
        //{
        //}

        public MachineStateTest(TestMachineState state, IEnumerable<ITransition<TestMachineStateTransition>> transitions) : base(transitions)
        {
        }

        public MachineStateTest(TestMachineState state, ITransition<TestMachineStateTransition> transition) : base(transition)
        {
        }

        public MachineStateTest(TestMachineState state, IEnumerable<TestMachineStateTransition> transitions) : base(transitions)
        {
        }

        public MachineStateTest(TestMachineState state, TestMachineStateTransition transition) : base(transition)
        {
        }

        public MachineStateTest(TestMachineState state) : base()
        {
        }
    }

    public class MachineTranslitionTest : Transition<TestMachineStateTransition>
    {
        public MachineTranslitionTest(TestMachineStateTransition action, 
            MachineState<TestMachineStateTransition> toState, 
            IEnumerable<Action> jobs = null) 
            : base(action, toState, jobs)
        {
        }

        public MachineTranslitionTest(TestMachineStateTransition action, 
            MachineState<TestMachineStateTransition> toState, 
            Action job = null)
            : base(action, toState, job)
        {
        }

        public MachineTranslitionTest(TestMachineStateTransition action,
            MachineState<TestMachineStateTransition> toState)         
            : base(action, toState)
        {
        }
    }

    public class TestMachineFactory
    {        
        public IMashineState<TestMachineStateTransition> GetInvalidTestMachine()
        {
            var state1 = new MachineStateTest(TestMachineState.State1);
            var state2 = new MachineStateTest(TestMachineState.State1);            

            state1.SetTransitions(new []
                {
                    new MachineTranslitionTest(TestMachineStateTransition.State_1_1, state1),
                    new MachineTranslitionTest(TestMachineStateTransition.State_1_1, state2)
                }
            );

            return state1;
        }

        public MachineStateTest state1;
        public MachineStateTest state2;
        public MachineStateTest state3;
             
        public IMashineState<TestMachineStateTransition> GetInvalidActionTestMachine()
        {
            state1 = new MachineStateTest(TestMachineState.State1);
            state2 = new MachineStateTest(TestMachineState.State2);

            state1.SetTransitions(new[]
                {
                    new MachineTranslitionTest(TestMachineStateTransition.State_1_1, state1),
                    new MachineTranslitionTest(TestMachineStateTransition.State_1_2, state1)
                }
            );

            return state1;
        }
        
        public IMashineState<TestMachineStateTransition> GetValidTestMachine(bool trowIfNotexists = true)
        {
            MachineTranslitionTest defaultTransition = null;
                        
            state1 = new MachineStateTest(TestMachineState.State1);
            state2 = new MachineStateTest(TestMachineState.State2);

            if (!trowIfNotexists)
                state1.DefaultTransition = new MachineTranslitionTest(TestMachineStateTransition.State_1_1, state1);

            state1.SetTransitions(new[]
                {
                    new MachineTranslitionTest(TestMachineStateTransition.State_1_1, state1),
                    new MachineTranslitionTest(TestMachineStateTransition.State_1_2, state2)
                }
            );

            return state1;
        }
        
        public IMashineState<TestMachineStateTransition> GetTestMachine( 
            Action a11, Action a12, Action a13, Action a23, Action a31)
        {
            state1 = new MachineStateTest(TestMachineState.State1);
            state2 = new MachineStateTest(TestMachineState.State2);
            state3 = new MachineStateTest(TestMachineState.State3);


            state1.SetTransitions(new[]
            {
                new MachineTranslitionTest(TestMachineStateTransition.State_1_1, state1, a11),
                new MachineTranslitionTest(TestMachineStateTransition.State_1_2, state2, a12),
                new MachineTranslitionTest(TestMachineStateTransition.State_1_3, state3, a13)
            });

            state2.SetTransition(new MachineTranslitionTest(TestMachineStateTransition.State_2_3, state3, a23));

            state3.SetTransition(new MachineTranslitionTest(TestMachineStateTransition.State_3_1, state1, a31));
            
            return state1;
        }
    }
}
