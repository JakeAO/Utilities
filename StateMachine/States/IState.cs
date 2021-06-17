using SadPumpkin.Util.Context;

namespace SadPumpkin.Util.StateMachine.States
{
    public interface IState
    {
        void PerformSetup(IContext context, IState previousState);
        void PerformContent();
        void PerformTeardown(IState nextState);
    }
}