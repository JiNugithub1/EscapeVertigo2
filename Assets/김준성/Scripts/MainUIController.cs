using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIController : MonoBehaviour
{
    private Animator anim;

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
        // ����� �� �ʱ�ȭ
        PlayerPrefs.SetInt("QUSTID", 0); // QUSTID �ʱ�ȭ
        PlayerPrefs.SetFloat("PlayerPosX", 0f); // �÷��̾� X ��ġ �ʱ�ȭ
        PlayerPrefs.SetFloat("PlayerPosY", 0f); // �÷��̾� Y ��ġ �ʱ�ȭ
        PlayerPrefs.SetFloat("PlayerPosZ", 0f); // �÷��̾� Z ��ġ �ʱ�ȭ
        PlayerPrefs.SetString("LastScene", "���ΰ���"); // �⺻ �� ����
        PlayerPrefs.Save(); // ������� ����

        // ���ΰ��� ������ �̵�
        SceneManager.LoadScene("���ΰ���");
        Debug.Log("�� ���� ����: ��� �� �ʱ�ȭ �� ���ΰ��� ������ ��ȯ.");
    }

    public void LoadGame()
    {
        // ����� �� �̸��� QUSTID �� �÷��̾� ��ġ �ҷ�����
        string lastScene = PlayerPrefs.GetString("LastScene", "���ΰ���"); // �⺻������ ���ΰ��� ����
        int questID = PlayerPrefs.GetInt("QUSTID", 0);
        float playerPosX = PlayerPrefs.GetFloat("PlayerPosX", 0f);
        float playerPosY = PlayerPrefs.GetFloat("PlayerPosY", 0f);
        float playerPosZ = PlayerPrefs.GetFloat("PlayerPosZ", 0f);

        // �ҷ��� ���� ����� �޽����� Ȯ��
        Debug.Log("�ҷ�����: ����� �� �̸� = " + lastScene);
        Debug.Log("�ҷ�����: ����� QUSTID = " + questID);
        Debug.Log("�ҷ�����: ����� �÷��̾� ��ġ = (" + playerPosX + ", " + playerPosY + ", " + playerPosZ + ")");

        // ����� ������ �̵�
        SceneManager.LoadScene(lastScene);
        Debug.Log("���� �ε�: " + lastScene + " ������ ��ȯ.");
    }

    public void GameExit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // ������ ��忡�� ���� ����
#endif
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
