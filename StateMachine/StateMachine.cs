using SadPumpkin.Util.Context;
using SadPumpkin.Util.StateMachine.Signals;
using SadPumpkin.Util.StateMachine.States;

namespace SadPumpkin.Util.StateMachine
{
    public class StateMachine : IStateMachine
    {
        public IContext SharedContext { get; }
        public IState CurrentState { get; private set; } = NullState.INSTANCE;

        public StateMachine(IContext sharedContext)
        {
            SharedContext = sharedContext;
            SharedContext.Set(this);
            SharedContext.Set(new StateChanged(), overwrite: false);
        }

        public void ChangeState<T>() where T : IState, new() => ChangeState(new T());

        public void ChangeState<T>(T constructedState) where T : IState
        {
            IState nextState = constructedState;

            CurrentState.PerformTeardown(SharedContext, nextState);

            SharedContext.Set(nextState);

            nextState.PerformSetup(SharedContext, CurrentState);

            CurrentState = nextState;

            SharedContext.Get<StateChanged>().Fire(nextState);

            CurrentState.PerformContent(SharedContext);
        }
    }
}