using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private float speed;
    GameObject player;
    Vector2 automoveVector;
    System.Random rand;
    Vector2 randVector;
    int frame_cnt;
    // Start is called before the first frame update
    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        automoveVector = (player.transform.position - transform.position).normalized;
        rand = new System.Random();
        randVector = new Vector2((float)(rand.NextDouble()-0.5),(float)(rand.NextDouble()-0.5));
        automoveVector = randVector;
        speed = 4;
        frame_cnt =0;
    }
    // Update is called once per frame
    void Update()
    {   
        if(this.GetComponent<Enemy>().state==1){
            frame_cnt++;    
            if(frame_cnt%120==0){
                automoveVector = (player.transform.position - transform.position).normalized;
                randVector = new Vector2((float)(rand.NextDouble()-0.5),(float)(rand.NextDouble()-0.5));
                Debug.Log(randVector);
                automoveVector = randVector;
            }

            transform.Translate(Time.deltaTime * speed * automoveVector.normalized);

            Vector3 position = transform.position;
            position = Vector3.Max(position, new Vector3(-9,-5,0));
            position = Vector3.Min(position, new Vector3(9,5,0));
            transform.position = position;
        }
        if(this.GetComponent<Enemy>().state==0){
            speed = 0;
        }
    }
}

