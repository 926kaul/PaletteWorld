using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class my_color : MonoBehaviour
{
    public SpriteRenderer PlayerRender;
    public int chosen = 0;
    // Start is called before the first frame update
    void Start(){
        PlayerRender = GetComponent<SpriteRenderer>();
    }
    void Update(){
        Debug.Log(gameObject.name + "is executing my_color.");
    }

}