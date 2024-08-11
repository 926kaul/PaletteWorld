using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grass_start : MonoBehaviour
{
    public SpriteRenderer PlayerRender;
    public int chosen = 0;
    // Start is called before the first frame update
    void Start(){
        PlayerRender = GetComponent<SpriteRenderer>();
        PlayerRender.color = new Color32(24,216,24,255);
        transform.position = new Vector3(9,9,0);
    }
    void Update(){
    }
    void OnMouseDown(){
        if(chosen==0){
            chosen=1;
            List<Fire_start> win_start = new List<Fire_start>(FindObjectsOfType<Fire_start>());
            Destroy(win_start[0].gameObject);
            List<Water_start> lose_start = new List<Water_start>(FindObjectsOfType<Water_start>());
            lose_start[0].chosen = -1;
            lose_start[0].transform.position = new Vector3(9,15,0);
            transform.position = GlobalVariables.setballs[0].transform.position;
            if (gameObject.GetComponent<my_color>() == null){
                gameObject.AddComponent<my_color>();
            }
            Destroy(this);
        }
    }

}

