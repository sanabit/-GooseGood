
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour // 뒤에것을 상속받는 것 사실상 필수
// 모노비해비어를 상속받아야 오브젝트에 넣을 수 있고, 어웨이크, 스타트 같은 함수 사용 가능
{
// public : 유니티 엔진과 다른 스크립트가 이 클래스를 열어볼 수 있도록 공개
    // 설정 및 보관용 변수 선언
    [Header("이동 및 점프 설정")] // 인스펙터 창에 속성 이름표
    [SerializeField] private float moveSpeed = 5f; // 거위의 이동속도. (인스펙터에서 조정 가능)
    // 유니티 인스펙터에 이 변수를 보여달라고 하는 것 private여도 조정 가능하게 여기서
    [SerializeField] private float jumpForce =12f;
    // private 지만 유니티 인스펙터에서 조정가능하도록 


    [Header("지면 감지 설정")]
    [SerializeField] private Transform groundCheck; // 발 밑 위치
    // transform 은 모든 오브젝트가 갖고 있는 위치, 회전, 크기정보를 담는 컴포넌트
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f) ; // 감지할 사각형의 크기
    [SerializeField] private LayerMask gruondLayer; //바닥으로 인식할 레이어 지정

    [Header("코요테 타임 설정")]
    [SerializeField] private float coyoteTime = 0.15f; // 코요테타임 시간
    private float coyoteTimeCounter; // 카운트다운 타이머. 얘는 인스펙터에서 안보임

    [Header("중력 및 가변 점프 설정")]
    [SerializeField] private float defaultGravity = 1f; // 기본중력 배율
    [SerializeField] private float fallMultiplier = 3f; // 떨어질 때 적용할 중력
    [SerializeField] private float lowJumpMultiplier = 2.5f; // 살짝 점프 할 때의 중력

    [Header("점프 버퍼링 설정")]
    [SerializeField] private float jumpBufferTime = 0.1f; // 착지 직전 선제입력기억시간
    private float jumpBufferCounter; // 버퍼 타이머


    private Rigidbody2D rb; //리지드바디 컴포넌트 (물리엔진조작)
    private float xInput; // onmove에서 넘어온 좌우 입력값 임시로 담아두는 곳
    private bool isGrounded; // 현재 땅에 있니?
    private bool isJumpPressed; // 점프키 누르고 있니?

    // 초기화 
    private void Awake() // 이 void는 이 함수 실행 끝난이후 아무런 값 리턴 x라는 뜻
    // awake 는 엔진이 시작 될 때 제일처음 실행되는 함수
    {
        rb = GetComponent<Rigidbody2D>();
        //거위 오브젝트에 붙어있는 리지드바디를 미리 rb에 넣어둠
    }

    // 지면 감지 영역
    //groundcheck 위치에서  설정한 반지름 크기의 원을 그려서 gorundlayer와 닿아있으면 true로 반환
    // 이거 아마 레이어 그룹으로 가능할 듯? 
    private void Update()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, gruondLayer);
        
        // 코요테 타임 
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime; // 바닥에 있으면 타이머 충전
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; // 공중에 있으면 시간차감
        } // deltaTime은 이전 프레임에서 다음 프레임으로 넘어가는데 걸린 실제 시간(초단위)

        if (jumpBufferCounter > 0f) // 점프버퍼링 시간 차감
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        { // 점프를 누를 때 마다 점프 버퍼링 카운터를 충전하므로 이 땅에 닿아 코요테 타임이 충전되었을 때 점프
            ExecuteJump();
        }



        ApplyGravity();
    } // 위치, 사각형 크기, 회전각도(세타), 감지할 레이어

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
        if (value.isPressed)
        {
            isJumpPressed = true; // 점프 누른 상태 저장


            jumpBufferCounter = jumpBufferTime;
            // 점프를 하면 점프 버퍼링 타이머를 충전하여 예약
            
        } // 버튼이 눌린순간 true됨 + 땅에 있을 때만
        else
        {// onjump 는 점프키를 뗀 순간에도 한번 더 실행된다. 상태가 변화할 때마다 실행되는 것
            isJumpPressed = false; // 점프키 뗀 상태 저장

            if (rb.linearVelocity.y > 0)
            { // 상승중 손 떼면 반토막
                rb.linearVelocity = new Vector2(rb.linearVelocity.x , rb.linearVelocity.y * 0.5f);
            }
        }
    }

    // 점프 버퍼링 추가
    private void ExecuteJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        if (!isJumpPressed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        coyoteTimeCounter = 0f; // 점프 했을 땐 0으로 만들어 이단점프 막기
        jumpBufferCounter = 0f;
    }
        // 물리 연산 영역
    private void FixedUpdate() //일정 시간 간격으로 실행
    {
        //x 축 속도 : )방향값 -1,1,0 * movespeed . 키를 떼는 즉시 0이되어 미끄러지지 않고 멈추게
        //y 축 속도는 기존 리지드 바디가 받고 있던 y축 속도 유지 > 중력낙하 방해 x
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
    }
    
    // 중력조절 영역
    private void ApplyGravity()
    {// 1. 공중에서 하강할 때 강한 중력 작용
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = fallMultiplier;
        }
        
        else if (rb.linearVelocity.y > 0 && !isJumpPressed)
        {// 2. 상승하는 도중 점프키를 뗏을 때
            rb.gravityScale = lowJumpMultiplier;
        }

        else
        {
            rb.gravityScale = defaultGravity;
        }
    }

    // 감지범위 확인
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }
}



