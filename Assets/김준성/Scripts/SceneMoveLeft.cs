using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMoveLeft : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform
    public string targetSceneName = "�ǹ��� ����"; // �̵��� �� �� �̸�
    public string currentSceneName = "�ʱ� ����"; // ���� �� �̸�
    public string beforeSceneName = "���ΰ���"; // ���� �� �̸�(���ΰ��� �̵� ���� �Ⱦ�) 


    public float targetSceneTransitionX = 13f; // �̵��� ������ ��ȯ�Ǵ� X ��
    public float currentSceneTransitionStartX = 24f; // ���� ������ �̵� ������ ���� X ��
    public float currentSceneTransitionEndX = 25f; // ���� ������ �̵� ������ �� X ��

    void Update()
    {
        // �÷��̾��� x ��ǥ�� targetSceneTransitionX ������ �� �̵��� �� ������ ��ȯ
        if (player.position.x <= targetSceneTransitionX && SceneManager.GetActiveScene().name != targetSceneName)
        {
            SceneManager.LoadScene(targetSceneName); // �̵��� �� ������ ��ȯ
        }

        // ���� ���� targetSceneName�̰�, �÷��̾ currentSceneTransitionStartX ~ currentSceneTransitionEndX ���̿� ������ Space�� �Է����� �� ���ΰ��� ������ �̵�
        if (SceneManager.GetActiveScene().name == targetSceneName &&
            player.position.x >= currentSceneTransitionStartX && player.position.x <= currentSceneTransitionEndX && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(beforeSceneName); // ���ΰ��� ������ ��ȯ
        }
    }
}
