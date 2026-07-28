
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour // 뒤에것을 상속받는 것 사실상 필수
// 모노비해비어를 상속받아야 오브젝트에 넣을 수 있고, 어웨이크, 스타트 같은 함수 사용 가능
{
// public : 유니티 엔진과 다른 스크립트가 이 클래스를 열어볼 수 있도록 공개
    // 설정 및 보관용 변수 선언
    [Header("이동 및 점프 설정")] // 인스펙터 창에 속성 이름표
    [SerializeField] private float moveSpeed = 5f; // 거위의 이동속도. (인스펙터에서 조정 가능)
    [SerializeField] private float jumpForce =10f;
    // private 지만 유니티 인스펙터에서 조정가능하도록 
    private Rigidbody2D rb; //리지드바디 컴포넌트 (물리엔진조작)
    private float xInput; // onmove에서 넘어온 좌우 입력값 임시로 담아두는 곳

    // 초기화 
    private void Awake() // 이 void는 이 함수 실행 끝난이후 아무런 값 리턴 x라는 뜻
    // awake 는 엔진이 시작 될 때 제일처음 실행되는 함수
    {
        rb = GetComponent<Rigidbody2D>();
        //거위 오브젝트에 붙어있는 리지드바디를 미리 rb에 넣어둠
    }

    // 입력 이벤트 수신 영역
    
    //player input 컴포넌트가 'move' 신호를 줄 때마다 이 함수가 실행됨
    // 즉 키보드 입력이 들어오거나 바뀔 때 실행
    private void OnMove(InputValue value) //파라미터로 받는것 
    {
        //value 안에는 wasd나 방향키 입력을 나타내는 벡터xy값이 들어있음
        // a누르면 (-1,0) 과 같은 느낌
        Vector2 moveVector = value.Get<Vector2>(); // 기본적으로 단위벡터임

        // 대각선 입력 속도 감소 방지
        if (moveVector.x > 0.1f)
        {
            xInput =1f; //
        }

        else if (moveVector.x < -0.1f)
        {
            xInput = -1f;
        }

        else
        {
            xInput = 0f; // 입력 없을 시 정지!
        }

        // 상하y 이동은 플랫포머에서 안쓰므로 좌우(x) 데이터만 빼와서 xinput에 덮어쓰기
        // 2D 플랫포머에서는 점프와 상하 이동은 구별해서 사용함

    }

    private void OnJump(InputValue value)
    {
        if (value.isPressed) // 버튼이 눌린순간 true됨
        {// x 축 속도 유지, y축은 점프 힘만큼 점프
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
        // 물리 연산 영역
    private void FixedUpdate() //일정 시간 간격
    {
        //x 축 속도 : )방향값 -1,1,0 * movespeed . 키를 떼는 즉시 0이되어 미끄러지지 않고 멈추게
        //y 축 속도는 기존 리지드 바디가 받고 있던 y축 속도 유지 > 중력낙하 방해 x
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
    }
    
}
