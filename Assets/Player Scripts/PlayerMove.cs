using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed; // 최대 속도
    public float acceleration; // 가속도
    public float runMultiplier; // 달리기 속도 배율
    public string targetTag = "Obstacle";
    public ObjectManager objectManager; // ObjectManager 참조 추가
    private bool lastDirectionFlipX; // 마지막 이동 방향 저장

    public float transitionThresholdLeft = -30f; // 왼쪽 끝 전환 임계값
    public float transitionThresholdRight = 30f; // 오른쪽 끝 전환 임계값
    public float stamina = 100f; // 현재 스태미너
    public float maxStamina = 100f; // 최대 스태미너
    public float staminaDrainRate = 10f; // 스태미너 소모 속도 (초당)
    public float staminaRegenRate = 5f; // 스태미너 회복 속도 (초당)

    private bool isRunning = false; // 현재 달리고 있는지 여부
    public Slider staminaBar; // 유니티 에디터에서 연결할 슬라이더
    public Vector3 staminaBarOffset = new Vector3(0, 10f, 0); // 플레이어 머리 위 오프셋
    public float staminaRecoveryCooldown = 10f; // 스태미너 회복 지연 시간 (초)
    private float staminaRecoveryTimer = 0f; // 현재 회복 지연 타이머

    private BoxCollider2D boxCollider;
    private float horizontalInput;

    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask groundLayer;


    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    Vector3 dirVec;
    GameObject scanObject;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

    }

    void Update()
    {
        if (Input.GetButton("Horizontal"))
        {
            lastDirectionFlipX = Input.GetAxisRaw("Horizontal") == -1;
            spriteRenderer.flipX = lastDirectionFlipX;
        }   


        if (Mathf.Abs(rigid.velocity.x) > 0.01f && Mathf.Abs(rigid.velocity.x) < 5f)
        {
            lastDirectionFlipX = Input.GetAxisRaw("Horizontal") == 1;

            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
        }
        if (Mathf.Abs(rigid.velocity.x) >= 5f)
        {
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("attack") && rigid.velocity.x == 0)
        {
            anim.SetBool("isIdle", true); // Idle 상태로 전환
        }
        else
        {
            anim.SetBool("isIdle", false);
        }

        // 상호작용 키(E)를 눌렀을 때 상호작용 실행
        if (Input.GetKeyDown(KeyCode.E))
        {
            Scan();
            if (scanObject != null)
            {
                objectManager.Action(scanObject); // 상호작용 실행
            }
        }

        // 스태미너 바 표시 및 위치 업데이트
        if (staminaBar != null)
        {
            if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
            {
                staminaBar.gameObject.SetActive(true); // 스태미너 바 활성화
                staminaBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + staminaBarOffset);
                staminaBar.value = stamina; // 슬라이더 값 갱신
            }
            else
            {
                staminaBar.gameObject.SetActive(false); // 스태미너 바 비활성화
            }
        }
    }
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        bool result = horizontalInput == 0 && isGrounded() && !onWall();
        Debug.Log($"CanAttack Result: {result}"); // 조건 결과를 출력
        return result;
    }


    void FixedUpdate()
    {
        // 상호작용 중에는 플레이어 이동 중지
        if (objectManager != null && objectManager.isAction)
        {
            rigid.velocity = Vector2.zero;
            return;
        }

        // 키 입력에 따른 움직임
        float h = Input.GetAxisRaw("Horizontal");
        float speed = maxSpeed;

        // 달리기 처리
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
        {
            speed *= runMultiplier;
            isRunning = true;
            stamina -= staminaDrainRate * Time.deltaTime; // 스태미너 소모
            staminaRecoveryTimer = 0f; // 달리는 동안 회복 지연 타이머 초기화
        }
        else
        {
            isRunning = false;
        }

        // 스태미너 회복 지연 처리
        if (stamina <= 10f)
        {
            staminaRecoveryTimer += Time.deltaTime; // 타이머 증가
        }

        // 스태미너 회복
        if (!isRunning && staminaRecoveryTimer >= staminaRecoveryCooldown && stamina < maxStamina)
        {
            stamina += staminaRegenRate * Time.deltaTime; // 스태미너 회복
        }

        // 스태미너가 음수로 내려가지 않도록 보장
        stamina = Mathf.Clamp(stamina, 0, maxStamina);

        // 물리적 힘을 사용한 이동 처리
        rigid.AddForce(new Vector2(h, 0) * acceleration, ForceMode2D.Impulse);

        // 최대 속도 제한
        if (rigid.velocity.x > speed)
            rigid.velocity = new Vector2(speed, rigid.velocity.y);
        else if (rigid.velocity.x < -speed)
            rigid.velocity = new Vector2(-speed, rigid.velocity.y);

        // 방향 벡터 설정
        if (h < 0)
        {
            dirVec = Vector3.left;
        }
        else if (h > 0)
        {
            dirVec = Vector3.right;
        }
    }

    void Scan()
    {

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 0.7f, LayerMask.GetMask("Object"));
        Debug.DrawRay(rigid.position, dirVec * 0.7f, new Color(0, 1, 0), 1.0f);

        if (rayHit.collider != null)
            scanObject = rayHit.collider.gameObject;
        else
            scanObject = null;

    }


}
