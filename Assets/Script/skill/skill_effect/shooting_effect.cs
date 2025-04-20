using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting_effect : MonoBehaviour
{
    public Vector3 target;
    [SerializeField] private float startSpeed = 5f;        // 초기 속도
    [SerializeField] private float acceleration = 100f;     // 초당 속도 증가량
    private float currentSpeed = 0f;
    public System.Action onArrive;

    void Start()
    {
        currentSpeed = startSpeed;
    }

    private bool arrived = false;

    void Update()
    {
        if (arrived) return;

        Vector3 toTarget = target - transform.position;
        Vector3 direction = toTarget.normalized;

        // 이동 거리 계산
        float step = currentSpeed * Time.deltaTime;
        Vector3 nextPos = transform.position + direction * step;

        // 이동 경로 선분이 타겟을 지나치는지 확인
        float beforeDist = Vector3.Dot(target - transform.position, direction);
        float afterDist = Vector3.Dot(target - nextPos, direction);

        if (beforeDist > 0f && afterDist <= 0f)  // 타겟을 지나쳤다면
        {
            transform.position = target;
            arrived = true;
            onArrive?.Invoke();
            Destroy(gameObject);
            return;
        }

        // 일반 이동
        transform.position = nextPos;
        transform.up = direction;
        currentSpeed += acceleration * Time.deltaTime;
    }


}
