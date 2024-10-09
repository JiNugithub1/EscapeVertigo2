using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TypingEffect : MonoBehaviour
{
    public TextMeshProUGUI speechText; // TextMeshPro �ؽ�Ʈ ������Ʈ
    public float typingSpeed = 0.05f; // �� ���ھ� ������ �ð� ����
    public List<string> dialogues; // ��� ���
    public GameObject imageObject; // Image ������Ʈ
    public GameObject textObject; // Text (TMP) ������Ʈ
    public int currentDialogueIndex = 0; // ���� ��� �ε���
    public bool TypingComplete { get; set; } // Ÿ���� �Ϸ� ���� Ȯ�� �÷��� �߰�
    public bool isTyping = false; // ���� Ÿ���� ������ Ȯ�� (public���� �����Ͽ� �ܺο��� ���� �����ϰ� ��)

    void Start()
    {
        Debug.Log("TypingEffect Start called.");
        SetChildrenActive(false); // �ʱ� ���¿��� Image�� Text (TMP) ��Ȱ��ȭ
    }

    public void StartTyping()
    {
        Debug.Log("StartTyping called.");
        if (dialogues.Count == 0)
        {
            Debug.LogError("��� ����� ��� �ֽ��ϴ�!");
            return;
        }

        // �̹� Ÿ���� ���̶�� ���ο� Ÿ������ �������� ����
        if (!isTyping && currentDialogueIndex < dialogues.Count)
        {
            SetChildrenActive(true); // Ÿ���� ���� �� Image�� Text (TMP) Ȱ��ȭ
            StartCoroutine(TypeText()); // Ÿ���� �ڷ�ƾ ����
        }
        else if (currentDialogueIndex >= dialogues.Count)
        {
            // ��簡 ������ UI�� ��Ȱ��ȭ�ϰ� ����
            SetChildrenActive(false);
            Debug.Log("��� ��簡 ����Ǿ����ϴ�.");
        }
    }

    private IEnumerator TypeText()
    {
        // Ÿ���� ����
        TypingComplete = false;

        string fullText = dialogues[currentDialogueIndex];
        speechText.text = ""; // ���ο� ��縦 ����ϱ� ���� �ʱ�ȭ

        foreach (char letter in fullText.ToCharArray())
        {
            speechText.text += letter;  // �� ���ھ� �߰�
            AdjustImageSize(); // ���ڰ� �߰��� ������ �̹��� ũ�� ����
            yield return new WaitForSeconds(typingSpeed); // ���� �ð�
        }

        // Ÿ���� �Ϸ�
        TypingComplete = true;
        Debug.Log("Ÿ���� �Ϸ�");

        // ��� �ε��� ����
        currentDialogueIndex++;

        // Ÿ���� �Ϸ� �� Space Ű �Է� ���
        while (TypingComplete)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // ���� ���� ����
                if (currentDialogueIndex < dialogues.Count)
                {
                    // ���� ��� Ÿ���� ����
                    Debug.Log("���� ��� ����: " + currentDialogueIndex);
                    StartCoroutine(TypeText()); // ���� ��縦 Ÿ����
                }
                else
                {
                    // ��ȭ�� ��� ������ ��쿡�� UI�� ��Ȱ��ȭ
                    SetChildrenActive(false);
                    Debug.Log("��� ��簡 ����Ǿ����ϴ�.");

                    FindObjectOfType<GameDirector>().playerMove.enabled = true; // GameDirector�� �����Ͽ� �÷��̾� �̵� Ȱ��ȭ

                }
                yield break; // �ڷ�ƾ ����
            }

            yield return null; // ���� ������ ���
        }
    }



    private void AdjustImageSize()
    {
        Vector2 textSize = speechText.GetPreferredValues(speechText.text);
        float widthPadding = 40f; // �ʺ� ����
        float heightPadding = 20f; // ���� ����
        imageObject.GetComponent<RectTransform>().sizeDelta = new Vector2(textSize.x + widthPadding, textSize.y + heightPadding); // ��ǳ�� ���� �߰�
        Debug.Log("��ǳ�� ũ�� ���� �Ϸ�.");
    }

    public void SetChildrenActive(bool isActive)
    {
        imageObject.SetActive(isActive);
        textObject.SetActive(isActive);
        Debug.Log($"SetChildrenActive called with value: {isActive}");
    }
}
