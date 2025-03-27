using UnityEngine;
using UnityEngine.UIElements;

public class Slash : MonoBehaviour
{
    private GameObject p;
    Vector2 MousePos;
    Vector2 dir;
    float angle;
    Vector3 dirNo;

    void Start()
    {
        p = GameObject.FindGameObjectWithTag("Player");

        Transform tr = p.GetComponent<Transform>();
        MousePos = Input.mousePosition;
        MousePos = Camera.main.ScreenToWorldPoint(MousePos);
        Vector3 pos = new Vector3(MousePos.x, MousePos.y, 0);
        dir = pos - tr.position;

        //바라보는 각도 구하기
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        transform.position = p.transform.position;
    }

    public void DestroySlash()
    {
        Destroy(gameObject);
    }
}
