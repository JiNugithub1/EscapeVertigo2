using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public Transform player;  // ���ΰ��� Transform�� ����
    public float detectionRange = 20.0f;  // ���� �Ÿ� ����
    public float attackAnimationSpeed = 1.5f; // ���� �ִϸ��̼� �ӵ� ����
    private bool isChasing = false;  // ���ΰ� ���� ���� üũ
    private bool hasLineOfSight = false; // �þ� �ȿ� �ִ��� üũ
    private bool isAttacking = false; // ���� ������ üũ

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.enabled = false;  // ó������ ������ ����
        Invoke("Think", 5);
    }

    void Update()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRange, LayerMask.GetMask("Player"));

        Debug.DrawRay(transform.position, directionToPlayer.normalized * detectionRange, Color.red); // �ð�ȭ

        if (rayHit.collider != null && rayHit.collider.CompareTag("Player"))
        {
            Debug.Log("Player detected!");
            spriteRenderer.enabled = true;
            isChasing = true;
            hasLineOfSight = true;
        }
        else
        {
            hasLineOfSight = false;
            if (!isChasing)
                spriteRenderer.enabled = false;
        }

        if (isChasing && hasLineOfSight && !isAttacking)//�÷��̾� �ν��ϰ� ���󰡴� �ڵ�
        {
            Vector2 direction = directionToPlayer.normalized;
            rigid.velocity = new Vector2(direction.x * 7, rigid.velocity.y);//�÷��̾� ���󰡴� �ӵ� 
            anim.SetInteger("WalkSpeed", 1);
            spriteRenderer.flipX = direction.x < 0;
        }
        else if (!isAttacking)
        {
            rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            // �÷��̾�� ������ ���� ���߰� ���� ���·� ��ȯ
            isChasing = false;
            isAttacking = true;
            rigid.velocity = Vector2.zero; // ���߱�
            anim.SetInteger("WalkSpeed", 0); // �ȱ� �ִϸ��̼� ����
            anim.SetTrigger("Attack"); // ���� �ִϸ��̼� Ʈ����
            anim.SetFloat("AttackSpeed", attackAnimationSpeed); // ���� �ִϸ��̼� �ӵ� ����

            // �ڷ�ƾ�� �����Ͽ� ���� �ִϸ��̼��� �Ϸ�Ǹ� Idle�� ���ư��� ��
            StartCoroutine(AttackCoroutine());
        }
    }

    IEnumerator AttackCoroutine()
    {
        // ���� �ִϸ��̼��� �Ϸ�� ������ ���
        yield return new WaitForSeconds(1f / attackAnimationSpeed);

        // ������ ������ ��� ���·� ��ȯ
        isAttacking = false;
        anim.ResetTrigger("Attack"); // ���� �ִϸ��̼� Ʈ���� �ʱ�ȭ
        anim.SetInteger("WalkSpeed", 0); // idle ���·� ��ȯ
    }

    void Think()
    {
        if (!isChasing && !isAttacking)  // ���ΰ��� ���� ���� �ƴ� ���� ����
        {
            nextMove = Random.Range(-1, 2);
            Invoke("Think", 5);
            anim.SetInteger("WalkSpeed", nextMove);

            if (nextMove != 0)
                spriteRenderer.flipX = nextMove == -1;
        }
    }
}
