using UnityEngine;

public class ImpactBurst : MonoBehaviour
{
    public Vector3 direction;
    public float speed = 3.5f;
    public float duration = 0.4f;
    private float elapsed = 0f;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        float t = elapsed / duration;

        // 이동
        transform.position += direction * speed * Time.deltaTime;

        // 점점 투명해짐
        if (sr != null)
        {
            Color c = sr.color;
            c.a = Mathf.Lerp(1f, 0f, t);
            sr.color = c;
        }

        // 삭제
        if (t >= 1f)
            Destroy(gameObject);
    }
}