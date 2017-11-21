using System;
using System.Collections.Generic;
using Convayor.Abstractions;
using Convayor.Tests.TestMachine;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;

namespace Convayor.Tests
{    
    public class ConvayStateMachine_Tests
    {
        private TestMachineFactory _factory;
        public ConvayStateMachine_Tests()
        {
            _factory = new TestMachineFactory();
        }

        [Fact]
        public void DuplicateActionShouldTrowExection()
        {
            Action invalidAction = new Action(() => _factory.GetInvalidTestMachine());
            invalidAction.ShouldThrow<DuplicateActionException>();
        }

        [Fact]
        public void NormalActionShouldNotTrowExection()
        {
            Action validAction = new Action(() => _factory.GetValidTestMachine());
            validAction.ShouldNotThrow<DuplicateActionException>();
        }

        [Fact]
        public void UnexistedTransitionShouldThrowException()
        {
            var state = _factory.GetValidTestMachine(true);

            Action action = () => state.Do(TestMachineStateTransition.State_1_3);            
            action.ShouldThrow<TransitionDoesNotExisistException>();
        }

        [Fact]
        public void UnexistedTransitionShouldNotThrowExceptionIfDefined()
        {
            var state = _factory.GetValidTestMachine(false);

            Action action = () => state.Do(TestMachineStateTransition.State_1_3);
            action.ShouldNotThrow<TransitionDoesNotExisistException>();
        }

        [Fact]
        public void TransitionShouldReturnSameState()
        {
            var state = _factory.GetValidTestMachine(false);
            state.Do(TestMachineStateTransition.State_1_3).Should().Be(state);            
        }

        [Fact]
        public void TransitionShouldReturnSecondState()
        {
            var state = _factory.GetValidTestMachine(true);
            state.Do(TestMachineStateTransition.State_1_2).Should().Be(_factory.state2);
        }

        [Fact]
        public void TransitionShouldReturnFirstState()
        {
            var state = _factory.GetValidTestMachine(true);
            state.Do(TestMachineStateTransition.State_1_1).Should().Be(_factory.state1);
        }

        
        [Fact]
        public void StateSequecnceShouldMatch()
        {
            var state = _factory.GetTestMachine(null, null, null, null, null);
            
            state.Do(TestMachineStateTransition.State_1_1).
                Do(TestMachineStateTransition.State_1_2).
                Do(TestMachineStateTransition.State_2_3).
                Do(TestMachineStateTransition.State_3_1).
                Do(TestMachineStateTransition.State_1_3).Should().Be(_factory.state3);            
        }

        private List<string> strList = new List<string>();
        [Fact]
        public void AllActionsShouldBeExecutedOnce()
        {
            Action a11 = () => strList.Add("1_1");
            Action a12 = () => strList.Add("1_2");
            Action a13 = () => strList.Add("1_3");
            Action a23 = () => strList.Add("2_3");
            Action a31 = () => strList.Add("3_1");

            var state = _factory.GetTestMachine(a11, a12, a13, a23, a31);

            state.Do(TestMachineStateTransition.State_1_1).
                Do(TestMachineStateTransition.State_1_2).
                Do(TestMachineStateTransition.State_2_3).
                Do(TestMachineStateTransition.State_3_1).
                Do(TestMachineStateTransition.State_1_3);

            strList.Count.Should().Be(5);
            strList.Should().Contain("1_1");
            strList.Should().Contain("1_2");
            strList.Should().Contain("1_2");
            strList.Should().Contain("2_3");
            strList.Should().Contain("3_1");
        }
    }
}
