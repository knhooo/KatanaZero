using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [Header("�÷��̾� �Ӽ�")]
    public float speed = 5;
    public float jumpUp = 1;
    public float power = 5;
    public Vector3 direction;
    public GameObject slash;

    //�׸���
    public GameObject shadow;
    List<GameObject> shadowList = new List<GameObject>();

    //��Ʈ ����Ʈ
    public GameObject hit_Lazer;


    Animator animator;
    Rigidbody2D rigid;
    SpriteRenderer sprite;

    public GameObject jumpDust;
    public GameObject wallJumpDust;

    public Transform wallCheck;
    public float wallDistance;
    public LayerMask wallLayer;
    public bool isWall;
    public float SlidingSpeed;
    public float wallJumpPower;
    public bool isWallJump;
    float isRight = 1;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        direction = Vector2.zero;
        sprite = GetComponent<SpriteRenderer>();
    }

    void KeyInput()
    {
        direction.x = Input.GetAxisRaw("Horizontal");//-1 0 1
        if (direction.x < 0)//����
        {
            sprite.flipX = true;
            animator.SetBool("Run", true);

            //������
            isRight =-1;
        }

        else if (direction.x > 0)//������
        {
            sprite.flipX = false;
            animator.SetBool("Run", true);

            //������
            isRight = 1; 
        }
        else
        {
            animator.SetBool("Run", false);
            for (int i = 0; i < shadowList.Count; i++)
            {
                Destroy(shadowList[i]); //���ӿ�����Ʈ�����
                shadowList.RemoveAt(i); //���ӿ�����Ʈ �����ϴ� ����Ʈ�����
            }
        }

        if (Input.GetMouseButtonDown(0))//���� ���콺
        {
            animator.SetTrigger("Attack");
            Instantiate(hit_Lazer, transform.position, Quaternion.identity);
        }

        //�׸��� ����
        for (int i = 0; i < shadowList.Count; i++)
        {
            shadowList[i].GetComponent<SpriteRenderer>().flipX = sprite.flipX;
        }

    }
    void Update()
    {
        if (!isWallJump)
        {
            KeyInput();
            Move();
        }

        //������ üũ
        isWall = Physics2D.Raycast(wallCheck.position, Vector2.right * isRight, wallDistance, wallLayer);
        Debug.DrawRay(wallCheck.position, Vector2.right * isRight, new Color(0, 1, 0));

        animator.SetBool("Grab", isWall);

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (animator.GetBool("Jump") == false)
            {
                Jump();
                animator.SetBool("Jump", true);
                JumpDust();
            }
        }

        if (isWall)
        {
            isWallJump = false;
            //�� ���� ����
            rigid.linearVelocity = new Vector2(rigid.linearVelocityX, rigid.linearVelocityY*SlidingSpeed);

            //���� ����ִ� ���¿��� ����
            if (Input.GetKeyDown(KeyCode.W))
            {
                isWallJump = true;

                //������ ����
                GameObject go = Instantiate(wallJumpDust, transform.position + new Vector3(0.8f * isRight, 0, 0), Quaternion.identity);
                go.GetComponent<SpriteRenderer>().flipX = sprite.flipX;
                Invoke("FreezeX", 0.3f);

                rigid.linearVelocity = new Vector2(-isRight * wallJumpPower, 0.9f * wallJumpPower);

                sprite.flipX = sprite.flipX == false ? true : false;
                isRight = -isRight;
            }
        }
    }

    void FreezeX()
    {
        isWallJump = false;

    }

    private void FixedUpdate()
    {
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Ground"));
        if (rigid.linearVelocityY < 0)
        {
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.7f)
                {
                    animator.SetBool("Jump", false);
                }
            }
            else
            {
                if (!isWall)//����������
                {
                    animator.SetBool("Jump", true);
                }
                else//��Ÿ��
                {
                    animator.SetBool("Grab", true);
                }
            }
        }
    }

    public void Move()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void Jump()
    {
        rigid.linearVelocity = Vector2.zero;

        rigid.AddForce(new Vector2(0, jumpUp), ForceMode2D.Impulse);
    }

    public void AttackSlash()
    {
        rigid.AddForce(Vector2.right * power, ForceMode2D.Impulse);
        GameObject go = Instantiate(slash, transform.position, Quaternion.identity);
        //go.GetComponent<SpriteRenderer>().flipX = sprite.flipX;
    }

    //�׸���

    public void RunShadow()
    {
        if (shadowList.Count < 6)
        {
            GameObject go = Instantiate(shadow, transform.position, Quaternion.identity);
            go.GetComponent<Shadow>().speed = 10 - shadowList.Count;
            shadowList.Add(go);
        }
    }

    //�����
    public void RandDust(GameObject dust)
    {
        Instantiate(dust, transform.position + new Vector3(-0.1f,-0.45f,0), Quaternion.identity);
    }
    //���� ����
    public void JumpDust()
    {
        if (isWall)
        {
            Instantiate(wallJumpDust, transform.position + new Vector3(0, 0.05f, 0), Quaternion.identity);
        }
        Instantiate(jumpDust,transform.position + new Vector3(0,0.05f,0), Quaternion.identity);
    }

    //������
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(wallCheck.position, Vector2.right * isRight * wallDistance);
    }
}
