namespace DarkPower.Infrastructure
{
    public interface IState
    {
        void Enter();
        void Exit();
    }
}
