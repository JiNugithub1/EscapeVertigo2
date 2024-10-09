using System.Collections;
using System.Collections.Generic;
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


    void Start()
    {
        // ������Ʈ Ȯ��
        if (player == null)
        {
            Debug.LogError("Player Transform�� �Ҵ���� �ʾҽ��ϴ�!");
        }

        if (typingEffect == null)
        {
            Debug.LogError("TypingEffect ��ũ��Ʈ�� �Ҵ���� �ʾҽ��ϴ�!");
        }

        if (playerMove == null)
        {
            Debug.LogError("PlayerMovement ��ũ��Ʈ�� �Ҵ���� �ʾҽ��ϴ�!");
        }

        if (soldierTypingEffect == null)
        {
            Debug.LogError("Soldier TypingEffect ��ũ��Ʈ�� �Ҵ���� �ʾҽ��ϴ�!");
        }

        // ���� ���� �� �÷��̾� ��ġ ����
        player.position = new Vector3(0f, player.position.y, player.position.z);
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
                // Idle �ִϸ��̼����� ��ȯ

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
                // Space Ű�� ������ �� ��ȭâ�� �ݰ� �÷��̾� �̵� Ȱ��ȭ
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    typingEffect.SetChildrenActive(false); // ��ǳ�� ��Ȱ��ȭ
                    playerMove.enabled = true; // ��� ��簡 ����Ǹ� �÷��̾� �̵� Ȱ��ȭ
                    Debug.Log("��� ��簡 ����Ǿ����ϴ�. Space �Է� �� �÷��̾� �̵� Ȱ��ȭ");

                    // �ʱ�ȭ
                    typingEffect.TypingComplete = false; // TypingComplete �ʱ�ȭ
                    isTypingStarted = false; // ���� ��縦 ���� Ÿ���� ���� �÷��� �ʱ�ȭ
                }
            }

            // ������ ��� ���� ����
            if (!soldierisTypingStarted && soldierTypingEffect.currentDialogueIndex < soldierTypingEffect.dialogues.Count)
            {
                // ���� ��� Ÿ���� ����
                soldierisTypingStarted = true;
                soldierTypingEffect.StartTyping(); // ���� ��� Ÿ���� ����
            }

            // ��� ���� ��簡 �������� Ȯ��
            if (soldierTypingEffect.currentDialogueIndex >= soldierTypingEffect.dialogues.Count && soldierTypingEffect.TypingComplete)
            {
                // Space Ű�� ������ �� ���� ��ȭâ�� �ݰ� ���� ���� �̵�
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    soldierTypingEffect.SetChildrenActive(false); // ���� ��ǳ�� ��Ȱ��ȭ
                    soldierTypingEffect.TypingComplete = false; // ���� Ÿ���� �Ϸ� �÷��� �ʱ�ȭ
                    soldierisTypingStarted = false; // ���� ��縦 ���� �÷��� �ʱ�ȭ

                    // ������ �̵��� �����ϴ� ���� �߰�
                    StartCoroutine(WaitForSoldierDialogue());
                   

                }
            }
        }
    }


    // Soldier�� �����̱� ������ ���� (Player�� x=3 �̻��� ��)
    public void StartSoldierMovement()
    {
        if (!isSoldierMoving)
        {
            isSoldierMoving = true; // Soldier�� �̵��� ���������� ���
            soldierMove.StartMoving(); // Soldier�� ������ ����
        }
    }

    // �÷��̾�� Soldier�� �浹 ó��
    public void OnPlayerSoldierCollision()
    {
        Debug.Log("Player�� Soldier �浹!");
        soldierTypingEffect.SetChildrenActive(true); // Soldier�� ��ǳ�� Ȱ��ȭ
        soldierisTypingStarted = false; // ��� Ÿ���� ���� �÷��� �ʱ�ȭ (��� ���� ����)
    }

    // Soldier ��簡 ���� ������ ���
    private IEnumerator WaitForSoldierDialogue()
    {
        while (soldierTypingEffect.isTyping) // ��簡 ���� ���̸� ��ٸ�
        {
            yield return null; // ���� ������ ���
        }

        // Soldier�� �̵���Ű�� ������� �ϴ� ����
        yield return StartCoroutine(soldierMove.MoveAndDisappear()); // Soldier �̵� �� ����� ó��
        playerMove.enabled = true; // ��� ��簡 ����Ǹ� �÷��̾� �̵� Ȱ��ȭ

        // ��ȭ�� ���� �� �÷��̾� �̵� Ȱ��ȭ
        Debug.Log("��ȭ�� ������, �÷��̾� �̵��� Ȱ��ȭ�Ǿ����ϴ�.");
    }
}
