using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    [Header("�� ĳ���� �Ӽ�")]
    public float detectionRange = 10f;//�÷��̾ ������ �� �ִ� �ִ� �Ÿ�
    public float shootingInterval = 2f;//�̻��� �߻� ������ ��� �ð�
    public GameObject missilePrefab;//�̻��� ������

    [Header("���� ������Ʈ")]
    public Transform firePoint;//�̻��� �߻� ��ġ
    private Transform player;//�÷��̾��� ��ġ
    private float shootTimer;//�߻� Ÿ�̸�
    private SpriteRenderer spriteRenderer;//��������Ʈ ���� ��ȯ��

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        shootTimer = shootingInterval;//Ÿ�̸� �ʱ�ȭ
    }

    void Update()
    {
        if (player == null) return;//�÷��̾ ������ �������� ����
        //�÷��̾���� �Ÿ� ���
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if(distanceToPlayer <= detectionRange)
        {
            spriteRenderer.flipX = (player.position.x < transform.position.x);

            //�̻���
            shootTimer -= Time.deltaTime; //Ÿ�̸� ����
            if(shootTimer <= 0)
            {
                Shoot(); //�̻��� �߻�
                shootTimer = shootingInterval;//Ÿ�̸� ����
            }
        }

    }
    //�̻��� �߻� �Լ�
    void Shoot()
    {
        //�̻��� ����
        GameObject missile = Instantiate(missilePrefab, firePoint.position, Quaternion.identity);

        //�÷��̾� �������� �߻� 
        Vector2 direction = (player.position - firePoint.position).normalized;
        missile.GetComponent<EnemyMissile>().SetDirection(direction);
        missile.GetComponent<SpriteRenderer>().flipX = (player.position.x < transform.position.x);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
