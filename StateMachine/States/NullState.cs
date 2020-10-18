namespace SadPumpkin.Util.StateMachine.States
{
    public class NullState : IState
    {
        public static readonly NullState INSTANCE = new NullState();

        private NullState()
        {
        }

        public void PerformSetup(Context.Context context, IState previousState)
        {
        }

        public void PerformContent(Context.Context context)
        {
        }

        public void PerformTeardown(Context.Context context, IState nextState)
        {
        }
    }
}