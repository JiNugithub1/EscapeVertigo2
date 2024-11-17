using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ObjectManager : MonoBehaviour
{
    public GameObject messagePanel; // �޽����� ǥ���� �г�
    public Text messageText; // �޽��� �ؽ�Ʈ
    public string targetSceneName = "�ǹ���"; // �̵��� �⺻ �� �̸�
    private GameObject scanObject; // ���� ��ȣ�ۿ� ������ ������Ʈ
    public bool isAction = false; // �г� Ȱ��ȭ ����

    public void Action(GameObject scanObj)
    {
        ObjData objData = scanObj.GetComponent<ObjData>(); // ObjData ������Ʈ ��������

        if (isAction)
        {
            isAction = false;
            messagePanel.SetActive(false);
        }
        else
        {
            isAction = true;
            scanObject = scanObj;

            if (objData != null && objData.id == 500) // ID�� 500�� ���
            {
                messageText.text = "���� ���Ƚ��ϴ�.";
                StartCoroutine(OpenDoorAndChangeScene("�ǹ���"));
            }
            else if (objData != null && objData.id == 501) // ID�� 501�� ���
            {
                messageText.text = "���� ���Ƚ��ϴ�.";
                StartCoroutine(OpenDoorAndChangeScene("���"));
            }
            else
            {
                messageText.text = "�̰��� �̸��� " + scanObj.name + "�̶�� �Ѵ�.";
            }

            messagePanel.SetActive(isAction);
        }
    }

    private IEnumerator OpenDoorAndChangeScene(string sceneName)
    {
        yield return new WaitForSeconds(2f); // 2�� ���
        messagePanel.SetActive(false); // �г� ��Ȱ��ȭ
        isAction = false; // ��ȣ�ۿ� �÷��� �ʱ�ȭ
        SceneManager.LoadScene(sceneName); // ������ ������ ��ȯ
        Debug.Log("�� ��ȯ �Ϸ�: " + sceneName);
    }
}
