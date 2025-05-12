using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting_with_particles : MonoBehaviour
{
    public Vector3 target;
    [SerializeField] private float startSpeed = 5f;        // 초기 속도
    [SerializeField] private float acceleration = 100f;     // 초당 속도 증가량
    private float currentSpeed = 0f;
    public System.Action onArrive;
    private Quaternion prefabRotationOffset;


    void Start()
    {
        currentSpeed = startSpeed;
        prefabRotationOffset = transform.rotation;
    }

    private bool arrived = false;

    void Update()
    {
        if (arrived) return;

        Vector3 toTarget = target - transform.position;
        Vector3 direction = toTarget.normalized;
        float step = currentSpeed * Time.deltaTime;
        Vector3 nextPos = transform.position + direction * step;

        float beforeDist = Vector3.Dot(target - transform.position, direction);
        float afterDist = Vector3.Dot(target - nextPos, direction);

        if (beforeDist > 0f && afterDist <= 0f)
        {
            transform.position = target;
            arrived = true;

            CreateBurstEffect(target);

            onArrive?.Invoke();
            Destroy(gameObject);
            return;
        }

        transform.position = nextPos;

        // 기본 회전 방향은 이동 방향(up 기준)
        Quaternion lookRotation = Quaternion.FromToRotation(Vector3.up, direction);
        Quaternion finalRotation = lookRotation * prefabRotationOffset;
        transform.rotation = finalRotation;

        currentSpeed += acceleration * Time.deltaTime;
    }

    void CreateBurstEffect(Vector3 pos)
    {
        GameObject burstPrefab = Resources.Load<GameObject>("Prefab/ImpactBurst");
        if (burstPrefab == null) return;

        int count = 6; // 파편 개수
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep + Random.Range(-10f, 10f); // 약간 랜덤화
            float rad = angle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);

            GameObject burst = Instantiate(burstPrefab, pos, Quaternion.identity);

            var burstScript = burst.AddComponent<ImpactBurst>();
            burstScript.direction = dir.normalized;

            SpriteRenderer sr = burst.GetComponent<SpriteRenderer>();
            SpriteRenderer shooterSR = GetComponent<SpriteRenderer>();
            if (sr != null && shooterSR != null)
                sr.color = shooterSR.color;
        }
    }
}
