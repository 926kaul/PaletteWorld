using Unity.VisualScripting;
using UnityEngine;

public class bullet : MonoBehaviour{
    public float speed = 10f;
    private float lifetime = 5f;
    private float spawnTime;
    private Vector2 direction;
    private bool isclone = false;
    public Color32 color;
    private SpriteRenderer BulletRenderer;

    public void SetBullet(Vector2 bullet_direction,Color player_color)
    {
        direction = bullet_direction;
        isclone = true;
        gameObject.SetActive(true);
        color = player_color;
        BulletRenderer = GetComponent<SpriteRenderer>();
        BulletRenderer.color = color;
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
            Enemy enemy = other.GetComponent<Enemy>();
            if(enemy.state==1){
                enemy.giveDamage(10);
                Destroy(this.gameObject);
            }
        }
    }

}