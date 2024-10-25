using UnityEngine;
using UnityEngine.SceneManagement;

public class SubMenuManager : MonoBehaviour
{
    public GameObject submenu; // ����޴� UI �г�
    private bool isPaused = false; // ���� �Ͻ����� ���¸� ����
    public Transform player; // �÷��̾� Transform

    void Start()
    {
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
            submenu.SetActive(true);
            Time.timeScale = 0; // ���� �Ͻ�����
            isPaused = true;
            Debug.Log("����޴��� ���Ƚ��ϴ�.");
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
            submenu.SetActive(false);
            Time.timeScale = 1; // ���� �簳
            isPaused = false;
            Debug.Log("����޴��� �������ϴ�.");
        }
        else
        {
            Debug.LogWarning("submenu�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    public void SaveAndExitToMainUI()
    {
        // QUSTID�� �÷��̾� ��ġ, ���� �� �̸��� ����
        PlayerPrefs.SetInt("QUSTID", PlayerPrefs.GetInt("QUSTID", 0)); // ���� QUSTID ���� ����
        PlayerPrefs.SetFloat("PlayerPosX", player.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", player.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", player.position.z);

        // ���� �� �̸� ����
        string currentSceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("LastScene", currentSceneName);

        PlayerPrefs.Save();

        Debug.Log("QUSTID, �÷��̾� ��ġ, ���� ��(" + currentSceneName + ")�� ����Ǿ����ϴ�.");

        // ���� UI�� �� ��ȯ
        SceneManager.LoadScene("����UI");
    }
    public void Exit()
    {
        Application.Quit();

    }
    public void Setting() { }

}