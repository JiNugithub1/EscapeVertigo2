using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public Transform player; // �÷��̾� ������Ʈ
    public TypingEffect typingEffect; // TypingEffect ��ũ��Ʈ
    public PlayerMove playerMove; // �÷��̾� ������ ��ũ��Ʈ

    private bool isTypingStarted = false;

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

        // ���� ���� �� �÷��̾� ��ġ ����
        player.position = new Vector3(0f, player.position.y, player.position.z);
        typingEffect.StartTyping();
    }

    void Update()
    {
        if (player != null && typingEffect != null && playerMove != null)
        {
            // Ÿ���� ���� ���� �÷��̾� �̵� ��Ȱ��ȭ
            if (typingEffect.isTyping)
            {
                playerMove.enabled = false; // Ÿ���� �߿��� �÷��̾� �̵� ��Ȱ��ȭ
                return; // Ÿ���� ���� ��� Update ����
            }

            // Ÿ���� ���� ���� (�÷��̾ x >= 0 �� ��)
            if (player.position.x >= 0 && !isTypingStarted && typingEffect.currentDialogueIndex < typingEffect.dialogues.Count)
            {
                isTypingStarted = true; // Ÿ���� ���� ���� �÷��� ����
                typingEffect.StartTyping(); // Ÿ���� ����
                playerMove.enabled = false; // Ÿ���� ���� �� �÷��̾� �̵� ��Ȱ��ȭ
            }

            // ��� ��簡 �������� Ȯ��
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
        }
    }





}
