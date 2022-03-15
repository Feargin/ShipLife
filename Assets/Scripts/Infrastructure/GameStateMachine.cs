using GameStates;
using UnityEngine;

namespace DarkPower.Infrastructure
{
    public class GameStateMachine : MonoStateMachineBase
    {
        [SerializeReference] private MonoStateBase[] _states;

        protected override void Awake()
        {
            base.Awake();
            SetState<PlayerCaveState>();
        }

        protected override void RegisterStates()
        {
            foreach (var state in _states)
                RegisterState(state);
        }
    }
}