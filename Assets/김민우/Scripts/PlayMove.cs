using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMove : MonoBehaviour
{
    public float maxSpeed;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //stop speed
        if(Input.GetButtonUp("Horizontal")){
            
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);

        }

        //������ȯ
        if(Input.GetButtonDown("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
            //animation
        if(Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("isWalking",false);
        else
            anim.SetBool("isWalking",true);

        Attack();
    }

    private float curTime;
    public float coolTime = 0.5f;
    private void Attack()
    {
         //����
        //z��ư�� ����
        if(curTime <= 0)
{
    if(Input.GetKeyDown(KeyCode.Z)) // Ű�� ���� �� �� ���� ����
    {
        anim.SetTrigger("atk");
        curTime = coolTime;
    }
}
else
{
    curTime -= Time.deltaTime;
}

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //move by control
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if(rigid.velocity.x > maxSpeed)//right maxSpeed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if(rigid.velocity.x < maxSpeed*(-1))//left maxSpeed
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);
    }
}
