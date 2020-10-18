namespace SadPumpkin.Util.StateMachine.States
{
    public interface IState
    {
        void PerformSetup(Context.Context context, IState previousState);
        void PerformContent(Context.Context context);
        void PerformTeardown(Context.Context context, IState nextState);
    }
}