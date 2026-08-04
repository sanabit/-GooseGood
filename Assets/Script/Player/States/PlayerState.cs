/* abstract는 추상클래스. 단독으로 사용할 수 없는 미완성 설계도 라는 뜻
유니티 게임 오브젝트에 직접 붙일수도 없고, new처럼 새로 만들 수 없음
 오직 다른 상태 클래스들이 상속받기 위한 용도로만 쓰임
 실수 방지 및 안정장치.  */
public abstract class PlayerState
{
    // 1. 자식 상태들이 가져다 쓸 리모컨들
    
    //유니티 C#에서는 내가 만든 스크립트 이름(클래스이름)이 곧 하나의 자료형이됨
   // palyerController 라는 스크립트를 담는 변수임
   // 이거를 player라는 이름의 리모컨으로 들고 있겠다.
    protected PlayerController player; // using과는 다른 진짜 리모컨을 하나 만든다.
    // protected 는 private처럼 외부에서 볼 수는 없지만 이 클래스를 물려받은
    // 자식상태들은 마음대로 쓸 수 있게 허용하는 접근 제한자. 자식전용공개. private는 자신만 가능.
    protected PlayerStateMachine stateMachine;


    // 2. 생성자 (연결선)
    public PlayerState(PlayerController player, PlayerStateMachine stateMachine)
    {
        this.player = player; // 리모컨 보관
        // 여기서 앞의 player는 이 클래스의 player이고, 뒤에 오는 player는 매개변수의 player
        this.stateMachine = stateMachine; // 스위치 보관
        // 여기에 있는 this의  . 은 함수나 그런게 아니라 구분을 위하여 붙인것.
    }


// virtual은 가상함수. 일단 세부내용 작성하지 않고 빈 상자로 비워둠.
// 대신 자식들이 필요할 대 override로 재정의하여 자기에 맞는 내용으로 넣을 수 있는 권한 부여
//  virtual이나 abstarct없는 자식쪽에서 override 못함. 미리만들어놔야 작성가능.
    public virtual void Enter() {}
    public virtual void Update() {}
    public virtual void FixedUpdate(){}
    public virtual void  Exit() {}
}
