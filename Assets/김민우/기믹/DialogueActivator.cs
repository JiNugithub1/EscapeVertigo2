using UnityEngine;

public class DialogueActivator : MonoBehaviour
{
    [SerializeField] private GameObject Bang; // ����ǥ ������Ʈ
    [SerializeField] private GameObject dialogueObject; // ��ȭ UI ������Ʈ
    [SerializeField] private Transform player;          // ���ΰ� Transform
    private bool dialogueStarted = false;              // ��ȭ ���� ���� üũ
    private bool dialogueFinished = false;             // ��ȭ ���� ���� üũ

    void Update()
    {
        if (player != null)
        {
            // ���ΰ��� x ��ǥ�� -8�� -7 ������ ��
            if (player.position.x >= -8f && player.position.x <= -7f && !dialogueStarted)
            {
                dialogueObject.SetActive(true); // ��ȭ UI Ȱ��ȭ
                dialogueStarted = true;        // ��ȭ ���� ���·� ����
                dialogueFinished = false;     // ��ȭ ���� ���� �ʱ�ȭ
                Bang.SetActive(false);
                Debug.Log("Dialogue Object Activated!");
            }

            // ��ȭ�� ������ �� ��Ȱ��ȭ
            if (dialogueStarted && dialogueFinished)
            {
                dialogueObject.SetActive(false); // ��ȭ UI ��Ȱ��ȭ
                dialogueStarted = false;        // ��ȭ ���� �ʱ�ȭ
                Debug.Log("Dialogue Object Deactivated!");
            }
        }
    }

    // ��ȭ ���Ḧ �ܺο��� ȣ���ϱ� ���� �޼���
    public void EndDialogue()
    {
        dialogueFinished = true; // ��ȭ ���� ���� ����
    }
}
