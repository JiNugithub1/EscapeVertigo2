using UnityEngine;

public class QuizGimmick : MonoBehaviour
{
    [SerializeField] private GameObject questionUI;      // ���� ���� UI
    [SerializeField] private GameObject bloodyBackground; // �Ƿ� ���� ���
    [SerializeField] private GameObject safe;            // �ݰ� ������Ʈ
    [SerializeField] private GameObject[] portraits;     // �ʻ�ȭ ������Ʈ �迭
    [SerializeField] private float detectionRange = 5f;  // �÷��̾� ���� �Ÿ�
    [SerializeField] private LayerMask playerLayer;      // �÷��̾� ���̾�
    public int correctAnswerIndex = 1;                   // ���� �ʻ�ȭ �ε��� (0���� ����)

    private bool quizStarted = false; // ���� ���� ����

    void Start()
    {
        // �ʱ� ���¿��� UI, ���, �ݰ� ��Ȱ��ȭ
        questionUI.SetActive(false);
        bloodyBackground.SetActive(false);
        safe.SetActive(false);
    }

    void Update()
    {
        Vector2 direction = (Vector2.right); // �߻� ���� (�ʿ� �� ����)

        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, direction, detectionRange, LayerMask.GetMask("Player"));

        if (rayHit.collider != null)
        {
            Debug.Log($"Raycast hit: {rayHit.collider.gameObject.name}");
        }

        if (rayHit.collider != null && !quizStarted)
        {
            Debug.Log("Player detected, starting the quiz!");
            quizStarted = true;
            StartQuiz();
        }
    }


    void StartQuiz()
    {
        questionUI.SetActive(true); // ���� UI Ȱ��ȭ

        // �ʻ�ȭ�� ��ȣ�ۿ� �����ϵ��� ����
        foreach (var portrait in portraits)
        {
            var trigger = portrait.GetComponent<PortraitTrigger>();
            if (trigger != null)
            {
                trigger.EnableInteraction();
            }
        }

        Debug.Log("Quiz started!");
    }

    public void HandleAnswer(int selectedIndex)
    {
        if (selectedIndex == correctAnswerIndex) // ������ ���
        {
            Debug.Log("����!");
            safe.SetActive(true); // �ݰ� Ȱ��ȭ
        }
        else // ������ ���
        {
            Debug.Log("����!");
            bloodyBackground.SetActive(true); // ��游 �Ƿ� ����
        }

        EndQuiz();
    }

    void EndQuiz()
    {
        // ���� ���� �� UI ��Ȱ��ȭ �� ��ȣ�ۿ� ��Ȱ��ȭ
        questionUI.SetActive(false);
        foreach (var portrait in portraits)
        {
            var trigger = portrait.GetComponent<PortraitTrigger>();
            if (trigger != null)
            {
                trigger.DisableInteraction();
            }
        }
        Debug.Log("Quiz ended.");
    }

    // Gizmos�� ����ĳ��Ʈ �ð�ȭ
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * detectionRange);
    }
}
