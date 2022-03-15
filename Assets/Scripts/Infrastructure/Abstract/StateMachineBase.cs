using System;
using System.Collections.Generic;

namespace DarkPower.Infrastructure
{
    public abstract class StateMachineBase : IStateMachine
    {
        protected IState _currentState;
        protected readonly Dictionary<Type, IState> _states = new Dictionary<Type, IState>();

        protected abstract void RegisterStates();
        
        public StateMachineBase()
        {
            RegisterStates();
        }


        public void SetState<TState>() where TState : class, IState
        {
            if (_states.TryGetValue(typeof(TState), out IState state))
            {
                _currentState?.Exit();
                _currentState = state;
                _currentState.Enter();
            }
        }
    }
}
