using System.Collections;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public Transform player; // �÷��̾� ������Ʈ
    public TypingEffect typingEffect; // TypingEffect ��ũ��Ʈ
    public PlayerMove playerMove; // �÷��̾� ������ ��ũ��Ʈ
    public SoldierMove soldierMove; // SoldierMove ��ũ��Ʈ �߰�
    public TypingEffect soldierTypingEffect; // Soldier�� ��ǳ�� ȿ�� �߰�
    public Animator playerAnimator; // �÷��̾� �ִϸ�����

    private bool isTypingStarted = false; // ��� Ÿ���� ���� ����
    private bool isSoldierMoving = false; // Soldier�� �̹� �̵� ������ ����
    private bool soldierisTypingStarted = false; // Soldier ��� Ÿ���� ���� ����
    private int questID;

    void Start()
    {
        GameLoad();
        // QUSTID ���� �ε� (�⺻���� 0)
        questID = PlayerPrefs.GetInt("QUSTID", 0);

        if (questID == 1)
        {
            Debug.Log("QUSTID�� 1�̹Ƿ� GameDirector�� ������� ����.");
            gameObject.SetActive(false); // GameDirector�� ��Ȱ��ȭ

            return; // �̺�Ʈ ���� ���� ����
        }

        // QUSTID�� 0�̸� �̺�Ʈ�� ����
        StartEvent();
    }

    void StartEvent()
    {
        // Ÿ���� �̺�Ʈ ����
        typingEffect.StartTyping();
    }

    void Update()
    {
        if (player != null && typingEffect != null && playerMove != null && soldierTypingEffect != null)
        {
            // �÷��̾ x >= 3�� �� �̵� ��Ȱ��ȭ �� ���� �̵� ����
            if (player.position.x >= 3 && !isSoldierMoving)
            {
                player.position = new Vector3(3f, player.position.y, player.position.z);
                playerMove.enabled = false; // �÷��̾� �̵� ��Ȱ��ȭ
                player.GetComponent<Animator>().SetBool("isWalk", false);
                StartSoldierMovement();
            }
            else
            {
                // Ÿ���� ���� ���� (�÷��̾ x >= 0 �� ��)
                if (!isTypingStarted && typingEffect.currentDialogueIndex < typingEffect.dialogues.Count)
                {
                    isTypingStarted = true; // Ÿ���� ���� ���� �÷��� ����
                    typingEffect.StartTyping(); // �÷��̾� ��� Ÿ���� ����
                    playerMove.enabled = false; // Ÿ���� ���� �� �÷��̾� �̵� ��Ȱ��ȭ
                }
            }

            // ��� �÷��̾� ��簡 �������� Ȯ��
            if (typingEffect.currentDialogueIndex >= typingEffect.dialogues.Count && typingEffect.TypingComplete)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    typingEffect.SetChildrenActive(false); // ��ǳ�� ��Ȱ��ȭ
                    playerMove.enabled = true; // ��� ��簡 ����Ǹ� �÷��̾� �̵� Ȱ��ȭ
                    Debug.Log("��� ��簡 ����Ǿ����ϴ�. Space �Է� �� �÷��̾� �̵� Ȱ��ȭ");
                    typingEffect.TypingComplete = false; // TypingComplete �ʱ�ȭ
                    isTypingStarted = false; // ���� ��縦 ���� Ÿ���� ���� �÷��� �ʱ�ȭ
                }
            }

            // ������ ��� ���� ����
            if (!soldierisTypingStarted && soldierTypingEffect.currentDialogueIndex < soldierTypingEffect.dialogues.Count)
            {
                soldierisTypingStarted = true;
                soldierTypingEffect.StartTyping(); // ���� ��� Ÿ���� ����
            }

            // ��� ���� ��簡 �������� Ȯ��
            if (soldierTypingEffect.currentDialogueIndex >= soldierTypingEffect.dialogues.Count && soldierTypingEffect.TypingComplete)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    soldierTypingEffect.SetChildrenActive(false); // ���� ��ǳ�� ��Ȱ��ȭ
                    soldierTypingEffect.TypingComplete = false; // ���� Ÿ���� �Ϸ� �÷��� �ʱ�ȭ
                    soldierisTypingStarted = false; // ���� ��縦 ���� �÷��� �ʱ�ȭ
                    StartCoroutine(WaitForSoldierDialogue());
                }
            }
        }
    }

    public void StartSoldierMovement()
    {
        if (!isSoldierMoving)
        {
            isSoldierMoving = true; // Soldier�� �̵��� ���������� ���
            soldierMove.StartMoving(); // Soldier�� ������ ����
        }
    }

    private IEnumerator WaitForSoldierDialogue()
    {
        while (soldierTypingEffect.isTyping) // ��簡 ���� ���̸� ��ٸ�
        {
            yield return null; // ���� ������ ���
        }

        // Soldier�� �̵���Ű�� ������� �ϴ� ����
        yield return StartCoroutine(soldierMove.MoveAndDisappear()); // Soldier �̵� �� ����� ó��
        playerMove.enabled = true; // ��� ��簡 ����Ǹ� �÷��̾� �̵� Ȱ��ȭ

        // �̺�Ʈ�� ������ QUSTID�� 1�� �����Ͽ� ���� ���� �ݺ����� �ʵ��� ��
        questID = 1;
        PlayerPrefs.SetInt("QUSTID", questID);
        PlayerPrefs.Save();

        Debug.Log("��ȭ�� ������, QUSTID�� 1�� �����Ǿ����ϴ�. �÷��̾� �̵��� Ȱ��ȭ�Ǿ����ϴ�.");
    }

    // �÷��̾�� Soldier�� �浹 ó��
    public void OnPlayerSoldierCollision()
    {
        Debug.Log("Player�� Soldier �浹!");
        soldierTypingEffect.SetChildrenActive(true); // Soldier�� ��ǳ�� Ȱ��ȭ
        soldierisTypingStarted = false; // ��� Ÿ���� ���� �÷��� �ʱ�ȭ (��� ���� ����)
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
