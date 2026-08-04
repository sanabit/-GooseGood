public class PlayerStateMachine
{
        // 현재 활성화된 상태를 보관
    public PlayerState CurrentState { get; private set;}
        // 자료형이 부모형. 부모 리모컨타입을 선언해두었기 때문에 대기 이동, 점프 등 이거에 다담기 가능
        // 뒤의 것들은 프로퍼티. 변수의 읽기, 쓰기 권한을 따로따로설정.
        // get은 읽기 권한인데 아무것도 안 붙였으니 앞의 public을 따라감. set은 쓰기 권한으로 private로 막아둠.

     // 1. 게임 시작 시 최초 상태 결정
    public void Initialize(PlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter(); // 첫 상태 진입
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit(); // 이전 상태 정리
        CurrentState. newState; // 새 상태로 교체
        CurrentState.Enter(); // 새 상태 시작
    }
}
