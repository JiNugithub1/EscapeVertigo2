using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public Transform player;  // ���ΰ��� Transform�� ����
    public float detectionRange = 20.0f;  // ���� �Ÿ� ����
    public float attackAnimationSpeed = 1.5f; // ���� �ִϸ��̼� �ӵ� ����
    private bool isChasing = false;  // ���ΰ� ���� ���� üũ
    private bool hasLineOfSight = false; // �þ� �ȿ� �ִ��� üũ
    private bool isAttacking = false; // ���� ������ üũ
    public int damage = 1;

    [SerializeField] private AudioClip enemySound; // �� �Ҹ� Ŭ��
    private AudioSource audioSource;             // ����� �ҽ�

    [Header("Game Over Settings")]
    public Image gameOverImg; // GameOver �̹���
    public string gameOverSceneName = "GameOver"; // �̵��� GameOver �� �̸�
    public int blinkCount = 3; // ������ Ƚ��
    public float blinkDuration = 0.5f; // ������ ����
    private bool isBlinking = false; // ������ ������ Ȯ��

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // AudioSource �ʱ�ȭ
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource ������Ʈ�� �ʿ��մϴ�. ������Ʈ�� �߰��ϼ���!");
        }

        if (gameOverImg != null)
        {
            gameOverImg.gameObject.SetActive(false); // GameOver �̹����� �ʱ� ��Ȱ��ȭ
        }

        Invoke("Think", 5); // �ʱ� �ൿ ���� ����
    }

    void Update()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRange, LayerMask.GetMask("Player"));

        Debug.DrawRay(transform.position, directionToPlayer.normalized * detectionRange, Color.red); // �ð�ȭ

        if (rayHit.collider != null && rayHit.collider.CompareTag("Player"))
        {
            Debug.Log("Player detected!");
            isChasing = true;
            hasLineOfSight = true;
        }
        else
        {
            hasLineOfSight = false;
        }

        if (isChasing && hasLineOfSight && !isAttacking) // �÷��̾� �ν��ϰ� ���󰡴� �ڵ�
        {
            Vector2 direction = directionToPlayer.normalized;
            rigid.velocity = new Vector2(direction.x * 7, rigid.velocity.y); // �÷��̾� ���󰡴� �ӵ�
            anim.SetInteger("WalkSpeed", 1);
            spriteRenderer.flipX = direction.x < 0;
        }
        else if (!isAttacking)
        {
            rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            // �÷��̾�� ������ ���� ���߰� ���� ���·� ��ȯ
            isChasing = false;
            isAttacking = true;
            rigid.velocity = Vector2.zero; // ���߱�
            anim.SetInteger("WalkSpeed", 0); // �ȱ� �ִϸ��̼� ����
            anim.SetTrigger("Attack"); // ���� �ִϸ��̼� Ʈ����
            anim.SetFloat("AttackSpeed", attackAnimationSpeed); // ���� �ִϸ��̼� �ӵ� ����

            // GameOver �̹��� �������� ���� ó�� (�ִϸ��̼� ����)
            if (!isBlinking)
            {
                isBlinking = true;
                StartCoroutine(BlinkGameOverImage());
            }
        }
    }


    private IEnumerator BlinkGameOverImage()
    {
        // ��� ������ ���߱�
        StopAllMovement();

        if (gameOverImg != null)
        {
            for (int i = 0; i < blinkCount; i++)
            {
                gameOverImg.gameObject.SetActive(true); // GameOver �̹��� Ȱ��ȭ
                yield return new WaitForSeconds(blinkDuration);
                gameOverImg.gameObject.SetActive(false); // GameOver �̹��� ��Ȱ��ȭ
                yield return new WaitForSeconds(blinkDuration);
            }

            // ������ �Ϸ� �� �� ��ȯ
            SceneManager.LoadScene(gameOverSceneName);
        }
        else
        {
            Debug.LogError("GameOverImg�� �������� �ʾҽ��ϴ�.");
        }
    }

    private void StopAllMovement()
    {
    

        // �÷��̾� ������ ���߱�
        if (player != null)
        {
            Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                playerRigidbody.velocity = Vector2.zero;
                playerRigidbody.isKinematic = true;
            }

            PlayerMove playerMove = player.GetComponent<PlayerMove>();
            if (playerMove != null)
            {
                playerMove.enabled = false;
            }
        }
    }

    // Animation Event���� ȣ��Ǵ� �Լ�
    public void PlayEnemySound()
    {
        if (audioSource != null && enemySound != null)
        {
            audioSource.PlayOneShot(enemySound); // �� �Ҹ� ���
            Debug.Log("Enemy sound played!");
        }
        else if (enemySound == null)
        {
            Debug.LogError("Enemy sound clip�� �������� �ʾҽ��ϴ�!");
        }
    }

    void Think()
    {
        if (!isChasing && !isAttacking)  // ���ΰ��� ���� ���� �ƴ� ���� ����
        {
            nextMove = Random.Range(-1, 2);
            Invoke("Think", 5);
            anim.SetInteger("WalkSpeed", nextMove);

            if (nextMove != 0)
                spriteRenderer.flipX = nextMove == -1;
        }
    }
}
