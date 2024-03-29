using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletTime;
    [SerializeField] private float fireRate;
    
    private bool _onFire;
    private float fireTimer;
    
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _onFire = true;
            fireTimer = 0;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _onFire = false;
        }

        if (_onFire)
        {
            fireTimer -= Time.deltaTime;
        
            if (fireTimer <= 0) {
                var bullet = Instantiate(bulletPrefab);
                bullet.transform.position = transform.position;
                bullet.GetComponent<Bullet>().SetBullet(bulletSpeed, bulletTime);
                fireTimer = fireRate;
            }
        }
    }
}
