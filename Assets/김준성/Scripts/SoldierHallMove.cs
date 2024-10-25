using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoldierHallMove : MonoBehaviour
{
    public PlayerMove playerMove; // �÷��̾� ������ ��ũ��Ʈ
    public float speed = 2.0f; // Soldier�� �̵� �ӵ�
    public float warningDistance = 2.0f; // �÷��̾�� Soldier ���� ��� �Ÿ�
    public Transform player; // �÷��̾� ������Ʈ
    public TypingEffect soldierTypingEffect; // Soldier�� Ÿ���� ����Ʈ
    public Image gameOverImg; // Canvas�� �ִ� GameOver �̹��� ������Ʈ
    public float blinkDuration = 0.5f; // �����̴� ���� (��)
    public int blinkCount = 3; // �����̴� Ƚ��
    public string gameOverSceneName = "GameOver"; // �̵��� GameOver �� �̸�
    public LayerMask playerLayer; // �÷��̾� ���̾�
    public AudioSource audioSource; // AudioSource ������Ʈ
    public AudioClip blinkSound; // ��ũ �� ����� ����� Ŭ��
    public Animator playerAnimator; // �÷��̾� �ִϸ�����
    public int qustID;

    private bool isWarningTriggered = false; // ��� �̺�Ʈ �߻� ����
    private bool isTypingStarted = false; // Ÿ������ ���۵Ǿ����� Ȯ���ϴ� �÷���
    private bool dialoguesFinished = false; // ��簡 ��� �������� ����
    private bool isBlinking = false; // �������� �� ���� ����ǵ��� �����ϴ� �÷���
    private Rigidbody2D soldierRigidbody;
    private PlayerMove playerMoveScript; // �÷��̾��� �������� ������ ��ũ��Ʈ
    private SpriteRenderer spriteRenderer; // Soldier�� ��������Ʈ ������
    void Start()
    {
        // PlayerPrefs���� QUSTID ���� �ε�
        qustID = PlayerPrefs.GetInt("QUSTID", 0);

        // QUSTID�� 2 �̻��̸� Soldier ��Ȱ��ȭ (�̺�Ʈ ���� ����)
        if (qustID >= 2)
        {
            GameLoad();
            gameObject.SetActive(false); // Soldier ������Ʈ ��Ȱ��ȭ
            return;
        }// Soldier�� Rigidbody�� PlayerMove ��ũ��Ʈ ��������
        soldierRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMoveScript = player.GetComponent<PlayerMove>();
        if (gameOverImg != null)
        {
            gameOverImg.gameObject.SetActive(false); // ó������ �̹��� ��Ȱ��ȭ
        }

        if (soldierTypingEffect == null || player == null || audioSource == null)
        {
            Debug.LogError("�ʼ� ������Ʈ �Ǵ� AudioSource�� �������� �ʾҽ��ϴ�!");
        }
    }

    void Update()
    {
        if (!isBlinking)
        {
            MoveSoldier();
        }

        // Soldier�� x ��ǥ -90�� �����ߴ��� Ȯ��
        if (transform.position.x <= -110 && qustID != 2)
        {
            // QUSTID�� 2�� ������Ʈ
            qustID = 2;
            PlayerPrefs.SetInt("QUSTID", qustID);
            PlayerPrefs.Save();
            Debug.Log("Soldier�� -110�� ���������Ƿ� QUSTID�� 2�� �����Ǿ����ϴ�.");
        }
        // �÷��̾���� ��ȣ�ۿ� ����
        CheckPlayerProximity();
    }

    // Soldier�� �̵� ó��
    private void MoveSoldier()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    // Raycast�� ����Ͽ� �÷��̾���� �Ÿ��� ����
    private void CheckPlayerProximity()
    {
        // Raycast�� Soldier�� �̵��ϴ� �������� �߻�
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, warningDistance, playerLayer);

        Debug.DrawRay(transform.position, Vector2.right * warningDistance, Color.red); // Raycast �ð�ȭ

        // Raycast�� �÷��̾�� �浹�ߴ��� Ȯ��
        if (hit.collider != null && hit.collider.transform == player)
        {
            if (!isWarningTriggered)
            {
                OnPlayerSoldierCollision(); // �浹 �̺�Ʈ �߻�
            }
        }
        else
        {
            // �÷��̾ ��� �Ÿ����� ����� ���� �ʱ�ȭ
            isWarningTriggered = false;
        }
    }

    // �÷��̾�� Soldier�� �浹 ó��
    public void OnPlayerSoldierCollision()
    {
        Debug.Log("Player�� Soldier �浹!");

        if (!dialoguesFinished) // ��簡 ������ �ʾ��� ���
        {
            if (!isTypingStarted)
            {
                // Soldier�� ��ǳ�� Ȱ��ȭ �� Ÿ���� ����Ʈ ����
                soldierTypingEffect.SetChildrenActive(true);
                soldierTypingEffect.StartTyping(); // Ÿ���� ����

                isTypingStarted = true; // ��� ���� �÷��� ����
                StartCoroutine(WaitForTypingCompletion()); // Ÿ���� �Ϸ� ���
            }

            // ��� �߻������� ����Ͽ� �ߺ� ���� ����
            isWarningTriggered = true;
        }
        else
        {
            // ��簡 �������� GameOver �̹��� ������ (�� ���� ����)
            if (!isBlinking)
            {
                isBlinking = true;
                StartCoroutine(BlinkGameOverImage());
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            // �浹 �� �ٷ� GameOver �̹��� ������ ����
            if (!isBlinking)
            {
                isBlinking = true;
                StartCoroutine(BlinkGameOverImage());
            }
        }
    }

    // Ÿ���� ����Ʈ�� ���� ������ ����ϴ� �ڷ�ƾ
    private IEnumerator WaitForTypingCompletion()
    {
        // Ÿ������ �Ϸ�� ������ ���
        while (!soldierTypingEffect.TypingComplete)
        {
            yield return null;
        }

        // Ÿ������ �Ϸ�Ǹ� ��ǳ���� ��Ȱ��ȭ
        soldierTypingEffect.SetChildrenActive(false);
        isTypingStarted = false; // Ÿ���� �÷��� �ʱ�ȭ

        // currentDialogueIndex�� ����� ���� �Ѿ�� ��簡 ���� ������ ó��
        if (soldierTypingEffect.currentDialogueIndex >= soldierTypingEffect.dialogues.Count)
        {
            dialoguesFinished = true; // ��� ���� �÷��� ����
        }
    }

    private IEnumerator BlinkGameOverImage()
    {
        // Soldier�� �÷��̾��� ������ ���߱�
        StopAllMovement();

        // Soldier�� flip ���� ����
        spriteRenderer.flipX = false;
        player.GetComponent<Animator>().SetBool("isWalk", false);

        if (gameOverImg != null)
        {
            for (int i = 0; i < blinkCount; i++)
            {
                gameOverImg.gameObject.SetActive(true); // �̹��� Ȱ��ȭ

                // ����� ��� (��ũ ���尡 �ִٸ�)
                if (audioSource != null && blinkSound != null)
                {
                    audioSource.PlayOneShot(blinkSound);
                }

                yield return new WaitForSeconds(blinkDuration); // ���
                gameOverImg.gameObject.SetActive(false); // �̹��� ��Ȱ��ȭ
                yield return new WaitForSeconds(blinkDuration); // ���
            }

            // �������� ������ GameOver ������ ��ȯ
            SceneManager.LoadScene(gameOverSceneName);
        }
        else
        {
            Debug.LogError("GameOverImg ������Ʈ�� �������� �ʾҽ��ϴ�.");
        }
    }

    private void StopAllMovement()
    {
        // Soldier�� Rigidbody �ӵ� ���߱�
        if (soldierRigidbody != null)
        {
            soldierRigidbody.velocity = Vector2.zero; // �ӵ� 0���� ����
            soldierRigidbody.isKinematic = true; // ���� ȿ�� ��Ȱ��ȭ
        }

        /*oldier�� �ִϸ����͵� ���߱� (�ִϸ��̼ǿ� ���� �������� �ʵ���)
        if (soldierAnimator != null)
        {
            soldierAnimator.enabled = false;
        }*/

        // �÷��̾� ������ ���߱� (PlayerMove ��ũ��Ʈ�� �ִ��� Ȯ�� �� ��Ȱ��ȭ)
        if (playerMoveScript != null)
        {
            playerMoveScript.enabled = false;
        }

        // �÷��̾��� Rigidbody �ӵ� ���߱�
        Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector2.zero; // �÷��̾� �ӵ� 0���� ����
            playerRigidbody.isKinematic = true; // �÷��̾� ���� ȿ�� ��Ȱ��ȭ
        }
        else
        {
            Debug.LogError("Player�� Rigidbody2D�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
    public void GameLoad()
    {
        if (!PlayerPrefs.HasKey("PlayerPosX")) return;

        float x = PlayerPrefs.GetFloat("PlayerPosX");
        float y = PlayerPrefs.GetFloat("PlayerPosY");
        float z = PlayerPrefs.GetFloat("PlayerPosZ");

        player.transform.position = new Vector3(x, y, z);
        Debug.Log("�ҷ����� �Ϸ�: �÷��̾� ��ġ = (" + x + ", " + y + ", " + z + ")");
    }
}
