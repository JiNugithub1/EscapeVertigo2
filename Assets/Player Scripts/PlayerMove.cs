using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // �� ��ȯ�� ���� �ʿ�
using UnityEngine.UI;
public class PlayerMove : MonoBehaviour
{
    public float maxSpeed; // �ִ� �ӵ�
    public float acceleration; // ���ӵ�
    public float runMultiplier; // �޸��� �ӵ� ����
    public string targetTag = "Obstacle";
    public float transitionThresholdLeft = -30f; // ���� �� ��ȯ �Ӱ谪
    public float transitionThresholdRight = 30f; // ������ �� ��ȯ �Ӱ谪
    public float stamina = 100f; // ���� ���¹̳�
    public float maxStamina = 100f; // �ִ� ���¹̳�
    public float staminaDrainRate = 10f; // ���¹̳� �Ҹ� �ӵ� (�ʴ�)
    public float staminaRegenRate = 5f; // ���¹̳� ȸ�� �ӵ� (�ʴ�)
    private bool isRunning = false; // ���� �޸��� �ִ��� ����
    public Slider staminaBar; // ����Ƽ �����Ϳ��� ������ �����̴�
    public Vector3 staminaBarOffset = new Vector3(0, 1.5f, 0); // �÷��̾� �Ӹ� �� ������
    public float staminaRecoveryCooldown = 10f; // ���¹̳� ȸ�� ���� �ð� (��)
    private float staminaRecoveryTimer = 0f; // ���� ȸ�� ���� Ÿ�̸�


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
    }

    void Update()
    {
        // ���� ���� �� �ִϸ��̼� ����
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        if (Mathf.Abs(rigid.velocity.x) > 0.01f && Mathf.Abs(rigid.velocity.x) < 2f)
            anim.SetBool("isWalk", true);
        else
            anim.SetBool("isWalk", false);

        if (Mathf.Abs(rigid.velocity.x) >= 2f)
            anim.SetBool("isRun", true);
        else
            anim.SetBool("isRun", false);

    

        // �� ��ȯ üũ
        CheckSceneTransition();

        // ���¹̳� �� ǥ�� �� ��ġ ������Ʈ
        if (staminaBar != null)
        {
            if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
            {
                staminaBar.gameObject.SetActive(true); // ���¹̳� �� Ȱ��ȭ
                staminaBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + staminaBarOffset);
                staminaBar.value = stamina; // �����̴��� �� ����
            }
            else
            {
                staminaBar.gameObject.SetActive(false); // ���¹̳� �� ��Ȱ��ȭ
            }
        }
    }


    void FixedUpdate()
    {

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

    void CheckSceneTransition()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log("Current Scene: " + currentScene);
        Debug.Log("Player Position: " + transform.position.x);

        // ���� ������ ���� ������ ��ȯ
        if (transform.position.x <= transitionThresholdLeft)
        {
            switch (currentScene)
            {
                case "PrisonHall2 CL":
                    Debug.Log("Transitioning to PrisonHall");
                    SceneManager.LoadScene("PrisonHall CL");
                    break;
                case "PrisonHall CL":
                    Debug.Log("Transitioning to MainHall");
                    SceneManager.LoadScene("MainHall");
                    break;
                case "MainHall":
                    Debug.Log("Transitioning to MedicalHall");
                    SceneManager.LoadScene("MedicalHall");
                    break;
                case "PrisonHall2 R":
                    Debug.Log("Transitioning to PrisonHall ");
                    SceneManager.LoadScene("PrisonHall CL");
                    break;
                case "PrisonHall R":
                    Debug.Log("Transitioning to MainHall");
                    SceneManager.LoadScene("MainHall");
                    break;
                case "MainHall R":
                    Debug.Log("Transitioning to MedicalHall");
                    SceneManager.LoadScene("MedicalHall");
                    break;
            }
        }
        // ������ ������ ���� ������ ��ȯ
        else if (transform.position.x >= transitionThresholdRight)
        {
            switch (currentScene)
            {
                case "MedicalHall":
                    Debug.Log("Transitioning to MainHall");
                    SceneManager.LoadScene("MainHall R");
                    break;
                case "MainHall R":
                    Debug.Log("Transitioning to PrisonHall");
                    SceneManager.LoadScene("PrisonHall CL 1");
                    break;
                case "MainHall":
                    Debug.Log("Transitioning to PrisonHall");
                    SceneManager.LoadScene("PrisonHall CL 1");
                    break;
                case "PrisonHall CL 1":
                    Debug.Log("Transitioning to PrisonHall2");
                    SceneManager.LoadScene("PrisonHall2 CL 1");
                    break;
                case "PrisonHall CL ":
                    Debug.Log("Transitioning to PrisonHall2");
                    SceneManager.LoadScene("Prison Hall2 CL 1");
                    break;

            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        // �����۰� �浹���� �� ���¹̳� ȸ��
        if (collision.CompareTag("StaminaItem"))
        {
            // ���¹̳� ���� ȸ��
            stamina += maxStamina / 2;

            // ���¹̳ʰ� �ִ밪�� ���� �ʵ��� ����
            stamina = Mathf.Clamp(stamina, 0, maxStamina);

            // ������ ������Ʈ ����
            Destroy(collision.gameObject);
        }
    }

}
