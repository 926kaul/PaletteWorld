using UnityEngine;

public class TriggerListener2D : MonoBehaviour
{
    public System.Action<Collider2D> onTriggerEnter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        onTriggerEnter?.Invoke(other);
    }
}