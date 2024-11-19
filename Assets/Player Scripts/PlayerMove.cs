using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed; // �ִ� �ӵ�
    public float acceleration; // ���ӵ�
    public float runMultiplier; // �޸��� �ӵ� ����
    public string targetTag = "Obstacle";
    public ObjectManager objectManager; // ObjectManager ���� �߰�
    private bool lastDirectionFlipX; // ������ �̵� ���� ����

    public float transitionThresholdLeft = -30f; // ���� �� ��ȯ �Ӱ谪
    public float transitionThresholdRight = 30f; // ������ �� ��ȯ �Ӱ谪
    public float stamina = 100f; // ���� ���¹̳�
    public float maxStamina = 100f; // �ִ� ���¹̳�
    public float staminaDrainRate = 10f; // ���¹̳� �Ҹ� �ӵ� (�ʴ�)
    public float staminaRegenRate = 5f; // ���¹̳� ȸ�� �ӵ� (�ʴ�)

    private bool isRunning = false; // ���� �޸��� �ִ��� ����
    public Slider staminaBar; // ����Ƽ �����Ϳ��� ������ �����̴�
    public Vector3 staminaBarOffset = new Vector3(0, 10f, 0); // �÷��̾� �Ӹ� �� ������
    public float staminaRecoveryCooldown = 10f; // ���¹̳� ȸ�� ���� �ð� (��)
    private float staminaRecoveryTimer = 0f; // ���� ȸ�� ���� Ÿ�̸�

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
            anim.SetBool("isIdle", true); // Idle ���·� ��ȯ
        }
        else
        {
            anim.SetBool("isIdle", false);
        }

        // ��ȣ�ۿ� Ű(E)�� ������ �� ��ȣ�ۿ� ����
        if (Input.GetKeyDown(KeyCode.E))
        {
            Scan();
            if (scanObject != null)
            {
                objectManager.Action(scanObject); // ��ȣ�ۿ� ����
            }
        }

        // ���¹̳� �� ǥ�� �� ��ġ ������Ʈ
        if (staminaBar != null)
        {
            if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
            {
                staminaBar.gameObject.SetActive(true); // ���¹̳� �� Ȱ��ȭ
                staminaBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + staminaBarOffset);
                staminaBar.value = stamina; // �����̴� �� ����
            }
            else
            {
                staminaBar.gameObject.SetActive(false); // ���¹̳� �� ��Ȱ��ȭ
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
        Debug.Log($"CanAttack Result: {result}"); // ���� ����� ���
        return result;
    }


    void FixedUpdate()
    {
        // ��ȣ�ۿ� �߿��� �÷��̾� �̵� ����
        if (objectManager != null && objectManager.isAction)
        {
            rigid.velocity = Vector2.zero;
            return;
        }

        // Ű �Է¿� ���� ������
        float h = Input.GetAxisRaw("Horizontal");
        float speed = maxSpeed;

        // �޸��� ó��
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
        {
            speed *= runMultiplier;
            isRunning = true;
            stamina -= staminaDrainRate * Time.deltaTime; // ���¹̳� �Ҹ�
            staminaRecoveryTimer = 0f; // �޸��� ���� ȸ�� ���� Ÿ�̸� �ʱ�ȭ
        }
        else
        {
            isRunning = false;
        }

        // ���¹̳� ȸ�� ���� ó��
        if (stamina <= 10f)
        {
            staminaRecoveryTimer += Time.deltaTime; // Ÿ�̸� ����
        }

        // ���¹̳� ȸ��
        if (!isRunning && staminaRecoveryTimer >= staminaRecoveryCooldown && stamina < maxStamina)
        {
            stamina += staminaRegenRate * Time.deltaTime; // ���¹̳� ȸ��
        }

        // ���¹̳ʰ� ������ �������� �ʵ��� ����
        stamina = Mathf.Clamp(stamina, 0, maxStamina);

        // ������ ���� ����� �̵� ó��
        rigid.AddForce(new Vector2(h, 0) * acceleration, ForceMode2D.Impulse);

        // �ִ� �ӵ� ����
        if (rigid.velocity.x > speed)
            rigid.velocity = new Vector2(speed, rigid.velocity.y);
        else if (rigid.velocity.x < -speed)
            rigid.velocity = new Vector2(-speed, rigid.velocity.y);

        // ���� ���� ����
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
