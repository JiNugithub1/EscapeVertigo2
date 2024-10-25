using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform
    public string initialHallwaySceneName = "�ʱ� ����"; // �ʱ� ���� �� �̸�
    public string prisonSceneName = "���ΰ���"; // ���ΰ��� �� �̸�

    void Start()
    {
    }

    void Update()
    {
        // �÷��̾��� x ��ǥ�� 13�� �� �ʱ� ���� ������ ��ȯ
        if (player.position.x >= 13 && SceneManager.GetActiveScene().name != initialHallwaySceneName)
        {
            SceneManager.LoadScene(initialHallwaySceneName); // �ʱ� ���� ������ ��ȯ
        }

        // ���� ���� �ʱ� �����̰�, �÷��̾ x ��ǥ 24~25 ���̿� ������ Space�� �Է����� �� ���ΰ��� ������ �̵�
        if (SceneManager.GetActiveScene().name == initialHallwaySceneName &&
            player.position.x >= 24 && player.position.x <= 25 && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(prisonSceneName); // ���ΰ��� ������ ��ȯ
        }
    }

   
}
