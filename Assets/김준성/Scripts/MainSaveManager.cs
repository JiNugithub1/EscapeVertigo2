using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSaveManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static MainSaveManager Instance; // 'instance'�� 'Instance'�� ����

    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� ������Ʈ ����
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� �ߺ��� ������Ʈ �ı�
        }
    }

    // �÷��̾� ��ġ�� �� �̸��� ������ �޼���
    public void SavePlayerData(Vector2 position, string sceneName)
    {
        PlayerPrefs.SetFloat("MainPlayerPosX", position.x); // X��ǥ ����
        PlayerPrefs.SetFloat("MainPlayerPosY", position.y); // Y��ǥ ����
        PlayerPrefs.SetString("LastSceneName", sceneName); // �� �̸� ����
        PlayerPrefs.Save(); // ���� ������ ����
        Debug.Log($"���� UI �÷��̾� ��ġ�� ����Ǿ����ϴ�: ({position.x}, {position.y}) in scene {sceneName}");
    }

    // ���� �ҷ����� �޼���
    public Vector2 LoadPlayerPosition()
    {
        float posX = PlayerPrefs.GetFloat("MainPlayerPosX", 0f); // �⺻���� 0
        float posY = PlayerPrefs.GetFloat("MainPlayerPosY", 0f); // �⺻���� 0
        Vector2 position = new Vector2(posX, posY);

        // �� �̸� �ҷ�����
        string lastSceneName = PlayerPrefs.GetString("LastSceneName", ""); // �⺻���� �� ���ڿ�

        // ��ȿ�� �˻�: ��ġ�� ���������̸� �ʱ�ȭ
        if (Mathf.Abs(position.x) > 100 || Mathf.Abs(position.y) > 100)
        {
            Debug.LogWarning("�ҷ��� �÷��̾� ��ġ�� ��ȿ���� �ʽ��ϴ�. �⺻ ��ġ�� �ʱ�ȭ�մϴ�.");
            position = Vector2.zero; // �⺻ ��ġ�� �ʱ�ȭ
        }

        Debug.Log($"�ҷ��� ���� UI �÷��̾� ��ġ: {position} in scene {lastSceneName}");
        return position; // �÷��̾� ��ġ ��ȯ
    }

    // �� ��ȯ �޼���
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // ������ ������ ��ȯ
    }

    // �÷��̾� ��ġ�� �����ϴ� �޼��� (�� �ε� �� ȣ��)
    public void SetPlayerPosition(GameObject player)
    {
        Vector2 position = LoadPlayerPosition();
        player.transform.position = position; // �÷��̾��� ��ġ ����
    }

    // Ư�� Ű ����
    public void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
        Debug.Log($"Ű '{key}' �����Ǿ����ϴ�.");
    }

    // ��� ������ ����
    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("��� �����Ͱ� �����Ǿ����ϴ�.");
    }
    public void SavePlayerData(Vector2 position, string sceneName, bool hasSeenIntroEvent)
    {
        PlayerPrefs.SetFloat("MainPlayerPosX", position.x);
        PlayerPrefs.SetFloat("MainPlayerPosY", position.y);
        PlayerPrefs.SetString("LastSceneName", sceneName);
        PlayerPrefs.SetInt("HasSeenIntroEvent", hasSeenIntroEvent ? 1 : 0); // �̺�Ʈ ���� ����
        PlayerPrefs.Save();
        Debug.Log($"���� UI �÷��̾� ��ġ�� ����Ǿ����ϴ�: ({position.x}, {position.y}) in scene {sceneName}");
    }
}
