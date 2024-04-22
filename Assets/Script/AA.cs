using Unity.VisualScripting;
using UnityEngine;

public class AA : MonoBehaviour{
    public int state=1; // 1 for bullet, 2 for lux_ball, 3 for color_ball, 4 for palette_ball
    public float speed = 10f;
    private float lifetime = 10f;
    private float spawnTime;
    private Vector2 direction;
    private bool isclone = false;
    public Color32 color;
    private SpriteRenderer DefaultSprite;
    [SerializeField] private Sprite BulletSprite;
    [SerializeField] private Sprite LuxSprite;
    [SerializeField] private Sprite ColorSprite;
    [SerializeField] private Sprite PaletteSprite;

    public void SetAA(Vector2 AA_direction,Color player_color,int AA_state) 
    {
        direction = AA_direction; //for created clone
        isclone = true;
        state = AA_state;
        gameObject.SetActive(true);
        DefaultSprite = GetComponent<SpriteRenderer>();
        switch(state){
            case 1: //bullet creation
                color = player_color;
                DefaultSprite.sprite = BulletSprite;
                DefaultSprite.color = color;
                break;
            case 2: //lux_ball creation
                DefaultSprite.sprite = LuxSprite;
                DefaultSprite.color = new Color32(255, 255, 255, 255);
                break;
            case 3: //color_ball creation
                DefaultSprite.sprite = ColorSprite;
                DefaultSprite.color = new Color32(255, 255, 255, 255);
                break;
            case 4: //palette_ball creation
                DefaultSprite.sprite = PaletteSprite;
                DefaultSprite.color = new Color32(255, 255, 255, 255);
                break;
            default:
                break;
        }
    }

    void Start(){
        if(!isclone){
            gameObject.SetActive(false);
        }
        spawnTime = Time.time;
    }
    void Update(){
        transform.Translate(Time.deltaTime * speed * direction.normalized);
        if(((Time.time - spawnTime) > lifetime) && isclone){
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Enemy"){
            Enemy enemy = other.GetComponent<Enemy>();
            if(enemy.state==1){
                switch(state){
                    case 1:
                        enemy.giveDamage(10);
                        Destroy(this.gameObject);
                        break;
                    case 2:
                        enemy.captured(this);
                        this.speed=0;
                        break;
                    case 3:
                        enemy.captured(this);
                        this.speed=0;
                        break;
                    case 4:
                        enemy.captured(this);
                        this.speed=0;
                        break;
                    default:
                        break;
                }
            }
        }
    }

}