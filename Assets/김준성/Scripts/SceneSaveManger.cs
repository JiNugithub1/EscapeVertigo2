using UnityEngine;
using UnityEngine.SceneManagement; // �� ������

public class SceneSaveManager : MonoBehaviour
{
    // �÷��̾� ��ġ ���� �޼���
    public void SavePlayerPosition(Vector2 position)
    {
        if (MainSaveManager.instance != null)
        {
            string sceneName = SceneManager.GetActiveScene().name; // ���� �� �̸� ��������
            MainSaveManager.instance.SavePlayerData(position, sceneName); // �� �̸��� ��ġ�� �Բ� ����
            Debug.Log($"������ �÷��̾� ��ġ�� ����Ǿ����ϴ�: ({position.x}, {position.y}) in scene {sceneName}");
        }
        else
        {
            Debug.LogWarning("MainSaveManager �ν��Ͻ��� ã�� �� �����ϴ�.");
        }
    }

    // �� �����͸� �����ϴ� �޼���
    public void SaveSceneData()
    {
        GameObject player = GameObject.FindWithTag("Player"); // "Player" �±װ� ���� ������Ʈ ã��
        if (player != null)
        {
            Vector2 playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
            SavePlayerPosition(playerPosition); // �÷��̾� ��ġ ����
        }
        else
        {
            Debug.LogWarning("�÷��̾� ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }
}
