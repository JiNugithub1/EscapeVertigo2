using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroEvent : MonoBehaviour
{
    public GameObject dialogueObject; // Dialogue ������Ʈ (SC)
    private Dialogue dialogueScript;  // Dialogue ��ũ��Ʈ ����


    void Start()
    {
       
        StartCoroutine(EventSequence());

    }

    private IEnumerator EventSequence()
    {
        // Dialogue ������Ʈ Ȱ��ȭ
        dialogueObject.SetActive(true);

        // ��ȭ�� ���� ������ ���
        yield return new WaitUntil(() => dialogueObject.activeSelf == false);

        SceneManager.LoadScene("PersonalCell"); // �̵��� �� ������ ��ȯ

    }
}
