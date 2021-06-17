using SadPumpkin.Util.Context;

namespace SadPumpkin.Util.StateMachine.States
{
    public class NullState : IState
    {
        public static readonly NullState INSTANCE = new NullState();

        private NullState()
        {
        }

        public void PerformSetup(IContext context, IState previousState)
        {
        }

        public void PerformContent()
        {
        }

        public void PerformTeardown(IState nextState)
        {
        }
    }
}