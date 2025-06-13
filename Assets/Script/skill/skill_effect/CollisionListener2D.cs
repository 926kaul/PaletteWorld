using UnityEngine;

public class CollisionListener2D : MonoBehaviour
{
    public System.Action<Collision2D> onCollisionEnter;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        onCollisionEnter?.Invoke(collision);
    }
}
