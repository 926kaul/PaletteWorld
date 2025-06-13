using UnityEngine;
using System;

public class shooting_effect_endless : MonoBehaviour
{
    public Vector3 target;
    [SerializeField] private float startSpeed = 5f;
    [SerializeField] private float acceleration = 100f;
    private float currentSpeed = 0f;
    private Quaternion prefabRotationOffset;

    private Vector3 moveDirection;
    private bool initialized = false;

    public Action onDestroyCallback;  // ✅ 파괴 시 콜백

    void Start()
    {
        currentSpeed = startSpeed;
        prefabRotationOffset = transform.rotation;
        moveDirection = (target - transform.position).normalized;
        initialized = true;
    }

    void Update()
    {
        if (!initialized) return;

        transform.position += moveDirection * currentSpeed * Time.deltaTime;

        Quaternion lookRotation = Quaternion.FromToRotation(Vector3.up, moveDirection);
        transform.rotation = lookRotation * prefabRotationOffset;

        currentSpeed += acceleration * Time.deltaTime;

        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPos.x < -0.1f || viewPos.x > 1.1f || viewPos.y < -0.1f || viewPos.y > 1.1f)
        {
            onDestroyCallback?.Invoke(); // ✅ 파괴 직전 콜백 호출
            Destroy(gameObject);
        }
    }
}
