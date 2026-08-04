public class PlayerIdleState : PlayerState
{
    // 생성자 : 부모(PlayerStste)의 생성자로 리모컨 2개를 전달해줌
    public PlayerIdleState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine){}
    // 이렇게 base가 쓰이면 전달받은 재료 player랑 statemachine을 부모생성자에게 넘겨주어 저장시킴
        // 기본적으로 중괄호는 넣어야함.
    public override void Enter()
    {
        base.Enter(); // 부모 클래스에 있는 Enter() 코드를 먼저 실행하라 라는 뜻인데 위에처럼 쓰이면 다른 의미임
        player.SetXVelocity(0f); // 내가 새로 만들 커스텀 함수
    }

    public override void Update()
    {
        base.Update();

        if (player.JumpBufferCounter > 0f)
        {
            stateMachine.ChangeState(player.JumpState);
        }

        else if (player.Xinput != 0f)
        {
            stateMachine.ChangeState(player.MoveState);
        }

        else if (!player.IsGrounded)
        {
            stateMachine.ChangeState(player.InAirState);
        }
    }

}
