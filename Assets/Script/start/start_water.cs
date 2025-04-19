using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_start : MonoBehaviour
{
    public SpriteRenderer PlayerRender;
    public int chosen = 0;

    void OnMouseDown()
    {
        if (chosen == 0)
        {
            chosen = 1;

            System.Random rnd = new System.Random();

            Water_start originalWater = this;
            Grass_start originalGrass = FindObjectOfType<Grass_start>();
            Fire_start originalFire = FindObjectOfType<Fire_start>();

            // Fire 적 복제
            (int,int)[] fire_positions = new (int,int)[] { (3, 15), (9, 15), (15, 15)};
            List<GameObject> enemyUnits = new List<GameObject>();
            for (int i = 0; i < 3; i++)
            {
                GameObject g = Instantiate(originalFire.gameObject);
                if (g.GetComponent<enemy_color>() == null)
                    g.AddComponent<enemy_color>();
                g.GetComponent<Fire_start>().chosen = -1;
                int tmpr = rnd.Next(0, 32);
                int tmpg = rnd.Next(0, 32);
                int tmpb = rnd.Next(0, 32);
                g.GetComponent<enemy_color>().color = new Color32((byte)(tmpr + 192), (byte)(tmpg + 32), (byte)(tmpb + 32), 255);
                g.GetComponent<enemy_color>().Update_stat();
                g.transform.position = new Vector3(fire_positions[i].Item1, fire_positions[i].Item2, 0);
                enemyUnits.Add(g);
            }

            // 플레이어 유닛 복제
            List<GameObject> playerUnits = new List<GameObject>();
            for (int i = 0; i < 6; i++)
            {
                GameObject f = Instantiate(originalWater.gameObject);
                f.transform.position = GlobalVariables.setballs[i].transform.position;
                if (f.GetComponent<my_color>() == null)
                    f.AddComponent<my_color>();
                f.GetComponent<Water_start>().chosen = 1;
                int tmpr = rnd.Next(0, 32);
                int tmpg = rnd.Next(0, 32);
                int tmpb = rnd.Next(0, 32);
                f.GetComponent<my_color>().color = new Color32((byte)(tmpr + 32), (byte)(tmpg + 32), (byte)(tmpb + 192), 255);
                f.GetComponent<my_color>().Update_stat();
                Destroy(f.GetComponent<Water_start>());
                playerUnits.Add(f);
            }

            // 기존 세 개 제거
            Destroy(originalWater.gameObject);
            Destroy(originalFire.gameObject);
            Destroy(originalGrass.gameObject);
        }
    }
}