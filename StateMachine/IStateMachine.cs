using SadPumpkin.Util.StateMachine.States;

namespace SadPumpkin.Util.StateMachine
{
    public interface IStateMachine
    {
        IState CurrentState { get; }
        void ChangeState<T>() where T : IState, new();
        void ChangeState<T>(T constructedState) where T : IState;
    }
}