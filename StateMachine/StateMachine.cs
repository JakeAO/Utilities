using SadPumpkin.Util.Context;
using SadPumpkin.Util.Signals;
using SadPumpkin.Util.StateMachine.Signals;
using SadPumpkin.Util.StateMachine.States;

namespace SadPumpkin.Util.StateMachine
{
    public class StateMachine : IStateMachine
    {
        public IContext SharedContext { get; }
        public IState CurrentState { get; private set; } = NullState.INSTANCE;

        private readonly Signal<IState> _stateChangedSignal;

        public StateMachine(IContext sharedContext, Signal<IState> stateChangedSignal = null)
        {
            _stateChangedSignal = stateChangedSignal ?? new StateChanged();

            SharedContext = sharedContext;
            SharedContext.SetValue(this);
            SharedContext.SetValue(_stateChangedSignal);
        }

        public void ChangeState<T>() where T : IState, new() => ChangeState(new T());

        public void ChangeState<T>(T constructedState) where T : IState
        {
            IState nextState = constructedState;

            CurrentState.PerformTeardown(nextState);

            SharedContext.SetValue(nextState);

            nextState.PerformSetup(SharedContext, CurrentState);

            CurrentState = nextState;

            _stateChangedSignal.Fire(nextState);

            CurrentState.PerformContent();
        }
    }
}