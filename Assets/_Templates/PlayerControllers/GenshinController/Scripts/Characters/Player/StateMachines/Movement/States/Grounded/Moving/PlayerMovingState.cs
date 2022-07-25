namespace GenshinController
{
    public class PlayerMovingState : PlayerGroundedState
    {
        public PlayerMovingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            base.Enter();

            StartAnimation(StateMachine.Player.AnimationData.MovingParameterHash);
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(StateMachine.Player.AnimationData.MovingParameterHash);
        }
        #endregion
    }
}
