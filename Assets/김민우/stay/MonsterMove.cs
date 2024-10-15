using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    public Transform player;             // �÷��̾ ����
    public float chaseRange = 10f;       // ������ ������ �Ÿ�
    public float attackRange = 2f;       // ������ ������ �Ÿ�
    public float attackCooldown = 1f;    // ���� ��� �ð�
    public float moveSpeed = 2f;         // ���� �̵� �ӵ�
    public float idleMoveTime = 3f;      // ������ �̵� �ð�
    public float idleDirectionChangeInterval = 2f; // ���� ���� ����

    private Rigidbody2D rb;
    private Animator animator;
    private float distanceToPlayer = Mathf.Infinity;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private Vector2 randomDirection;    // ������ ���� ����
    private float directionChangeTimer; // ���� ���� Ÿ�̸�

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D ������Ʈ ����
        animator = GetComponent<Animator>();  // Animator ������Ʈ ����
        directionChangeTimer = idleDirectionChangeInterval;
        ChooseNewRandomDirection(); // ���� �� ������ ���� ����
    }

    void Update()
    {
        // �÷��̾���� �Ÿ� ���
        distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= attackRange)
        {
            // ���� ����
            AttackPlayer();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            // ���� ����
            ChasePlayer();
        }
        else
        {
            // ������ �̵� (Idle ����)
            Wander();
        }

        // ���� ��ٿ� Ÿ�̸� ������Ʈ
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                isAttacking = false;
            }
        }
    }

    // ������ �̵� ����
    void Wander()
    {
        // ���� �ð����� ���� ����
        directionChangeTimer -= Time.deltaTime;
        if (directionChangeTimer <= 0f)
        {
            ChooseNewRandomDirection(); // ���ο� ���� ����
            directionChangeTimer = idleDirectionChangeInterval; // Ÿ�̸� ����
        }

        // ������ ������ �������� �̵�
        rb.velocity = randomDirection * moveSpeed;
        animator.SetBool("isMoving", true); // �̵� �ִϸ��̼� Ȱ��ȭ
    }

    // ������ ���� ����
    void ChooseNewRandomDirection()
    {
        // -1���� 1 ������ ������ ���� ������ ���� ����
        // -1���� 1 ������ ������ ���� ������ x�� ���� ���� (y���� ����)
        float randomX = Random.Range(-1f, 1f);
        float randomY = 0f; // y���� �����Ͽ� ���� ���⸸ ������
        randomDirection = new Vector2(randomX, randomY);

    }

    // ���� ����
    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
        animator.SetBool("isMoving", true); // �̵� �ִϸ��̼� Ȱ��ȭ
    }

    // ���� ����
    void AttackPlayer()
    {
        if (!isAttacking)
        {
            rb.velocity = Vector2.zero; // ���� �� ����
            animator.SetTrigger("Attack"); // ���� �ִϸ��̼� ���
            isAttacking = true;
            attackTimer = attackCooldown; // ���� ��� �ð� ����
        }
    }

    // �÷��̾ ���� ���� ������ ������ �� ����
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // �÷��̾ �����Ǹ� �÷��̾ ����
            player = other.transform;
        }
    }

    // �÷��̾ ���� ���� ������ ������ ��
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // �÷��̾ ��ħ
            player = null;
            Wander();
        }
    }
}
