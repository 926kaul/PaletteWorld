using Unity.VisualScripting;
using UnityEngine;

public class bullet : MonoBehaviour{
    public float speed = 10f;
    private float lifetime = 5f;
    private float spawnTime;
    private Vector2 direction;
    private bool isclone = false;

    public void SetBullet(Vector2 bullet_direction)
    {
        direction = bullet_direction;
        isclone = true;
        gameObject.SetActive(true);
    }

    void Start(){
        if(!isclone){
            gameObject.SetActive(false);
        }
        spawnTime = Time.time;
    }
    void Update(){
        transform.Translate(Time.deltaTime * speed * direction.normalized);
        if(((Time.time - spawnTime) > lifetime) && (isclone)){
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Enemy"){
            other.GetComponent<Enemy>().giveDamage(10);
            Destroy(this.gameObject);
        }
    }
}