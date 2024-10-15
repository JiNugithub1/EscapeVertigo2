using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIController : MonoBehaviour
{
    private Animator anim;
    public GameObject playerPrefab; // �÷��̾� �������� �巡���Ͽ� �Ҵ�
    private Vector2 playerPosition; // �÷��̾� ��ġ ���� ����

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // �ʿ��ϴٸ� ������Ʈ ������ �߰��� �� �ֽ��ϴ�.
    }

    public void GameStart()
    {
        // �� ��ȯ ���� �÷��̾��� ��ġ�� �����մϴ�.
        GameLoad(); // ���� ���� �� ��ġ �ҷ�����
        SceneManager.LoadScene("���ΰ���"); // ���ΰ��� ������ ��ȯ
        Debug.Log("Play");
    }

    public void GameExit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // ������ ��忡�� ���� ����
#endif
    }

    public void GameLoad()
    {
        // SaveManager �ν��Ͻ��� ã��
        MainSaveManager saveManager = FindObjectOfType<MainSaveManager>();

        // PlayerPrefs���� ����� �÷��̾� ��ġ�� �ҷ�����
        Vector2 playerPosition = saveManager.LoadPlayerPosition();

        // ����� �� �̸� ��������
        string lastSceneName = PlayerPrefs.GetString("LastSceneName", "");

        if (!string.IsNullOrEmpty(lastSceneName))
        {
            // �ش� ������ ��ȯ
            saveManager.LoadScene(lastSceneName);

            // �÷��̾� ������Ʈ ���� (�ش� ������ ����)
            GameObject player = Instantiate(playerPrefab, playerPosition, Quaternion.identity);
            Debug.Log("�÷��̾� ��ġ�� �ҷ��Խ��ϴ�: " + playerPosition);
        }
        else
        {
            Debug.LogWarning("����� ���� �����ϴ�.");
        }
    }

    public void GameSetting()
    {
        // ���� ���� UI�� ���� ������ ���⿡ �߰��� �� �ֽ��ϴ�.
        Debug.Log("���� ������ �������ϴ�.");
    }

    public void Close()
    {
        StartCoroutine(CloseAfterDelay());
    }

    private IEnumerator CloseAfterDelay()
    {
        anim.SetTrigger("close");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        anim.ResetTrigger("close");
    }
}
 