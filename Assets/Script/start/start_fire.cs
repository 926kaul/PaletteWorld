using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fire_start : MonoBehaviour
{
    public SpriteRenderer PlayerRender;
    public int chosen = 0;
    // Start is called before the first frame update
    void Start(){
        PlayerRender = GetComponent<SpriteRenderer>();
        PlayerRender.color = new Color32(216,56,56,255);
        transform.position = new Vector3(7,9,0);
    }
    void Update(){
    }
    void OnMouseDown(){
        if(chosen==0){
            chosen=1;
            List<Water_start> win_start = new List<Water_start>(FindObjectsOfType<Water_start>());
            Destroy(win_start[0].gameObject);
            List<Grass_start> lose_start = new List<Grass_start>(FindObjectsOfType<Grass_start>());
            lose_start[0].chosen = -1;
            System.Random rnd = new System.Random();
            byte rrr= (byte)rnd.Next(65,190);
            byte ggg = (byte)rnd.Next(65,190);
            byte bbb = (byte)rnd.Next(65,90);
            lose_start[0].GetComponent<SpriteRenderer>().color = new Color32(rrr,ggg,bbb,255);
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

