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

    void Update()
    {
        // 타겟 방향 계산
        Vector3 direction = (target - transform.position).normalized;

        // 현재 속도만큼 전진
        transform.position += direction * currentSpeed * Time.deltaTime;
        transform.up = direction;

        // 속도 점점 증가
        currentSpeed += acceleration * Time.deltaTime;

        // 너무 가까우면 삭제
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            onArrive?.Invoke();
            Destroy(gameObject);
        }
    }
}
