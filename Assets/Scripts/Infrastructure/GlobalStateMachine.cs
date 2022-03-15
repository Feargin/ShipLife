namespace DarkPower.Infrastructure
{
    public class GlobalStateMachine : StateMachineBase
    {
        protected override void RegisterStates()
        {
            _states.Add(typeof(BootstrapState), new BootstrapState());
            _states.Add(typeof(GameLoopState), new GameLoopState());
        }
    }
}