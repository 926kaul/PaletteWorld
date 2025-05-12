using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting_effect_norotation : MonoBehaviour
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
        onArrive?.Invoke();
        Destroy(gameObject);
        return;
    }

    transform.position = nextPos;

    currentSpeed += acceleration * Time.deltaTime;
}





}
