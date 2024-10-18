using System.Collections;
using UnityEngine;

public class HallwayManager : MonoBehaviour
{
    public TypingEffect soldierTypingEffect; // Soldier�� Ÿ���� ����Ʈ
    public Transform player; // �÷��̾� ������Ʈ
    public SoldierHallMove soldierMove; // Soldier�� �̵� ��ũ��Ʈ (���� �̵�)

    private bool isTypingStarted = false; // Ÿ������ ���۵Ǿ����� Ȯ���ϴ� �÷���

    void Start()
    {
        // �� �ʱ�ȭ
        InitializeScene();
    }

    // �� �ʱ�ȭ
    private void InitializeScene()
    {
        // Soldier�� �̵� ����
        soldierMove.enabled = true;

        // Ÿ���� ����Ʈ�� ��Ȱ��ȭ
        if (soldierTypingEffect != null)
        {
            soldierTypingEffect.SetChildrenActive(false);
        }
    }

    // �÷��̾�� Soldier�� �浹 �̺�Ʈ ó��
    public void OnPlayerSoldierCollision()
    {
        Debug.Log("Player�� Soldier �浹!");

        if (!isTypingStarted && soldierTypingEffect.dialogues.Count > 0) // TypingEffect�� ��� ����� ����
        {
            // Soldier�� ��ǳ�� Ȱ��ȭ �� Ÿ���� ����Ʈ ����
            soldierTypingEffect.SetChildrenActive(true);

            // Ÿ���� ����Ʈ�� ���� ��� ���
            soldierTypingEffect.currentDialogueIndex = 0; // ��� �ε��� �ʱ�ȭ
            soldierTypingEffect.StartTyping(); // Ÿ���� ����

            isTypingStarted = true; // ��� ���� �÷��� ����

            // ��簡 ���� �� Ÿ���� ����Ʈ ����
            StartCoroutine(WaitForTypingCompletion());
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

        // Ÿ������ �Ϸ�Ǹ� ��ǳ�� ��Ȱ��ȭ
        soldierTypingEffect.SetChildrenActive(false);
        Debug.Log("Ÿ���� �Ϸ�, ��ǳ�� ��Ȱ��ȭ.");
        isTypingStarted = false; // ��� Ÿ���� �÷��� �ʱ�ȭ
    }
}
