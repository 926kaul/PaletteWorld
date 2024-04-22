using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


public class Player_Right_Attack : MonoBehaviour
{
    public GameObject bulletPrefab;
    Vector2 MousePosition;
    Vector2 pos;
    Camera Camera;
    [SerializeField] int aa_state=1;
    private float wheel_update;
    // Start is called before the first frame update
    private void Start(){
        Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {   
        if(Input.GetKey(KeyCode.Alpha1)){ //min_mix, colormix
                aa_state=1;
        }
        if(Input.GetKey(KeyCode.Alpha2)){ //min_mix, colormix
                aa_state=2;
        }
        if(Input.GetKey(KeyCode.Alpha3)){  //max_mix, lightmix
                aa_state=3;
        }
        if(Input.GetKey(KeyCode.Alpha4)){  //avg_mix, palette_mix
                aa_state=3;
        }
        if((Input.GetAxis("Mouse ScrollWheel")>0)&&((Time.time-wheel_update)>0.3f)){ 
            wheel_update=Time.time;
            if(aa_state==4) aa_state=0;
            else aa_state++;
        }
        if((Input.GetAxis("Mouse ScrollWheel")<0)&&((Time.time-wheel_update)>0.3f)){
            wheel_update=Time.time;
            if(aa_state==0) aa_state=4;
            else aa_state--;
        }
        if(Input.GetMouseButtonDown(1)){
            MousePosition = Input.mousePosition;
            MousePosition = Camera.ScreenToWorldPoint(MousePosition);
            pos = this.transform.position;
            GameObject AA = Instantiate(bulletPrefab, transform.position, transform.rotation);
            AA.GetComponent<AA>().SetAA(MousePosition-pos,this.GetComponent<Player>().color,aa_state);
        }
    }
}

