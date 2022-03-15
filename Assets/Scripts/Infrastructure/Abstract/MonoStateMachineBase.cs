using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkPower.Infrastructure
{
    public abstract class MonoStateMachineBase : MonoBehaviour, IMonoStateMachine
    {
        public MonoStateBase CurrentState { get; private set; }
        private readonly Dictionary<Type, MonoStateBase> _registeredStates = new Dictionary<Type, MonoStateBase>();

        protected abstract void RegisterStates();
        
        protected virtual void Awake()
        {
            RegisterStates();
        }

        public void SetState<TState>() where TState : MonoStateBase
        {
            if (_registeredStates.TryGetValue(typeof(TState), out MonoStateBase state))
            {
                SetState(state);
            }
        }

        public void SetState(MonoStateBase state)
        {
            if(CurrentState == state)
                return;
            
            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();
        }

        protected void RegisterState(MonoStateBase state)
        {
            _registeredStates.Add(state.GetType(), state);
        }
    }
}