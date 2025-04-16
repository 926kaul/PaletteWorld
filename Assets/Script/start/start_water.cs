using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Water_start : MonoBehaviour
{
    public SpriteRenderer PlayerRender;
    public int chosen = 0;
    // Start is called before the first frame update
    void Start(){
        PlayerRender = GetComponent<SpriteRenderer>();
        PlayerRender.color = new Color32(56,56,216,255);
        transform.position = new Vector3(11,9,0);
    }
    void Update(){
    }
    void OnMouseDown()
    {
        if(chosen==0){
            chosen=1;
            List<Grass_start> win_start = new List<Grass_start>(FindObjectsOfType<Grass_start>());
            Destroy(win_start[0].gameObject);
            List<Fire_start> lose_start = new List<Fire_start>(FindObjectsOfType<Fire_start>());
            
            //lose start 상대 배치
            lose_start[0].chosen = -1;
            lose_start[0].transform.position = new Vector3(9,15,0);
            if (lose_start[0].GetComponent<enemy_color>() == null){
                lose_start[0].AddComponent<enemy_color>();
            }
            
            transform.position = GlobalVariables.setballs[0].transform.position;
            if (gameObject.GetComponent<my_color>() == null){
                gameObject.AddComponent<my_color>();
            }
            Destroy(this);
        }
    }

}

