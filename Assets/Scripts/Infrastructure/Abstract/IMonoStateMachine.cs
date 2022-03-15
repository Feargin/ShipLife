namespace DarkPower.Infrastructure
{
    public interface IMonoStateMachine
    {
        void SetState<TState>() where TState : MonoStateBase;
    }
}