using UnityEngine;

public class PortraitTrigger : MonoBehaviour
{
    [SerializeField] private int portraitIndex;       // �ʻ�ȭ �ε���
    [SerializeField] private float detectionRange = 2f; // �÷��̾� ���� �Ÿ�
    [SerializeField] private LayerMask playerLayer;   // �÷��̾� ���̾�

    private bool interactionEnabled = false;          // ��ȣ�ۿ� Ȱ��ȭ ����

    public void EnableInteraction()
    {
        interactionEnabled = true;
    }

    public void DisableInteraction()
    {
        interactionEnabled = false;
    }

    void Update()
    {
        // ����ĳ��Ʈ�� ����Ͽ� �÷��̾ �ʻ�ȭ �Ʒ��� �ִ��� Ȯ��
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, detectionRange, LayerMask.GetMask("Player"));

        if (hit.collider != null && interactionEnabled && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log($"Player interacted with portrait {portraitIndex}");

            // QuizGimmick ��ũ��Ʈ�� ����
            QuizGimmick quizGimmick = FindObjectOfType<QuizGimmick>();
            if (quizGimmick != null)
            {
                quizGimmick.HandleAnswer(portraitIndex);
            }
        }
    }

    void OnDrawGizmos()
    {
        // ����ĳ��Ʈ �ð�ȭ (������)
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * detectionRange);
    }
}
