namespace DarkPower.Infrastructure
{
    public interface IStateMachine
    {
        void SetState<TState>() where TState : class, IState;
    }
}