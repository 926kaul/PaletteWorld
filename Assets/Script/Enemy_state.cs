using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int hp=100;
    // Update is called once per frame
    void Update()
    {
        if(hp<=0){
            Destroy(this.gameObject);
        }
    }

    public void giveDamage(int damage){
        hp -= damage;
    }
}
