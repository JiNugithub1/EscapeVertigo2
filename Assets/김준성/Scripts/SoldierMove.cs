using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierMove : MonoBehaviour
{
    public Transform player;  // �÷��̾��� Transform
    public float speed = 2.0f; // Soldier �̵� �ӵ�
    private bool shouldMove = false; // Soldier�� �������� �ϴ��� ����
    private bool isMoving = false; // Soldier�� �̵� ������ ����

    // Soldier�� �̵��� ������ �� ȣ���ϴ� �޼���
    public void StartMoving()
    {
        if (!isMoving) // Soldier�� �̹� �̵� ������ Ȯ��
        {
            isMoving = true; // �̵� ���� ���
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true); // ������Ʈ�� Ȱ��ȭ
            }
            shouldMove = true; // Soldier�� �̵��ؾ� ��
        }
    }

    void Update()
    {
        MoveTowardsPlayer(); // �� �����Ӹ��� �÷��̾ ���� �̵�
    }

    void MoveTowardsPlayer()
    {
        // Soldier�� �÷��̾�� �浹�� ������ �÷��̾� �������� �̵�
        if (shouldMove)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Soldier�� �÷��̾�� �浹���� ��
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x); // x�� �������� ������
            transform.localScale = scale;
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = startPosition + new Vector3(7.0f, 0, 0); // ���������� 2 ���� �̵�
            shouldMove = false; // �浹 �� Soldier ������ ����
     
            isMoving = false; // �̵� ���� ���·� ����
            Debug.Log("Player�� �浹!");

            // GameDirector�� OnPlayerSoldierCollision �޼��� ȣ��
            FindObjectOfType<GameDirector>().OnPlayerSoldierCollision();
        }
    }

    // Soldier�� �̵���Ű�� ������� �ϴ� �ڷ�ƾ
    public IEnumerator MoveAndDisappear()
    {
        Vector3 scale = transform.localScale;
        scale.x = -Mathf.Abs(scale.x); // x�� �������� ������
        transform.localScale = scale;
        // Soldier�� ���������� 3�ʰ� �̵�
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + new Vector3(11.0f, 0, 0); // ���������� 2 ���� �̵�

        while (elapsedTime < 3.0f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 3.0f);
            elapsedTime += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }

        // Soldier ������Ʈ ��Ȱ��ȭ
        gameObject.SetActive(false);
        
    }
}
