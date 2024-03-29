using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


public class PlayerAttack : MonoBehaviour
{
    public GameObject bulletPrefab;
    Vector2 MousePosition;
    Vector2 pos;
    Camera Camera;
    // Start is called before the first frame update
    private void Start(){
        Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            MousePosition = Input.mousePosition;
            MousePosition = Camera.ScreenToWorldPoint(MousePosition);
            pos = this.transform.position;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.GetComponent<bullet>().SetBullet(MousePosition-pos);
        }
    }
}

