using UnityEngine;
using UnityEngine.SceneManagement; // �� ������

public class SubMenuManager : MonoBehaviour
{
    public GameObject submenu; // ����޴� UI �г�
    private bool isPaused = false; // ���� �Ͻ����� ���¸� ����

    void Start()
    {
        // ����޴��� ����
        if (submenu != null)
        {
            submenu.SetActive(false);
        }
        else
        {
            Debug.LogWarning("submenu�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    void Update()
    {
        // ESC Ű �Է� �� ����޴� ����/�ݱ�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                CloseSubMenu();
            }
            else
            {
                OpenSubMenu();
            }
        }
    }

    public void OpenSubMenu()
    {
        if (submenu != null)
        {
            // ����޴� ����
            submenu.SetActive(true);
            Time.timeScale = 0; // ���� �Ͻ�����
            isPaused = true; // ���� ������Ʈ
            Debug.Log("����޴��� ���Ƚ��ϴ�."); // ����� �޽��� �߰�
        }
        else
        {
            Debug.LogWarning("submenu�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    public void CloseSubMenu()
    {
        if (submenu != null)
        {
            // ����޴� �ݱ�
            submenu.SetActive(false);
            Time.timeScale = 1; // ���� �簳
            isPaused = false; // ���� ������Ʈ
            Debug.Log("����޴��� �������ϴ�."); // ����� �޽��� �߰�
        }
        else
        {
            Debug.LogWarning("submenu�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    public void Setting() { }

    public void ResumeGame()
    {
        CloseSubMenu();
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // ������ ��忡�� ���� ����
#endif
    }

    // ���� �� ���� UI�� ���ư��� �޼���
    public void SaveAndExitToMainUI()
    {
        // ���� SceneSaveManager �ν��Ͻ��� ã��
        SceneSaveManager sceneSaveManager = FindObjectOfType<SceneSaveManager>();

        // SceneSaveManager�� ������ ��� �����͸� ����
        if (sceneSaveManager != null)
        {
            // �� �����͸� ���� (�÷��̾� ��ġ ����)
            sceneSaveManager.SaveSceneData();
        }
        else
        {
            Debug.LogWarning("SceneSaveManager�� ã�� �� �����ϴ�.");
        }

        // ���� UI�� �� ��ȯ
        UnityEngine.SceneManagement.SceneManager.LoadScene("����UI");
    }

}
