using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass_start : MonoBehaviour
{
    public SpriteRenderer PlayerRender;
    public int chosen = 0;

    void OnMouseDown()
    {
        if (chosen == 0)
        {
            chosen = 1;

            System.Random rnd = new System.Random();

            Grass_start originalGrass = this;
            Fire_start originalFire = FindObjectOfType<Fire_start>();
            Water_start originalWater = FindObjectOfType<Water_start>();

            // Water 적 복제
            (int,int)[] water_positions = new (int,int)[] { (3, 15), (9, 15), (15, 15)};
            List<GameObject> enemyUnits = new List<GameObject>();
            for (int i = 0; i < 3; i++)
            {
                GameObject g = Instantiate(originalWater.gameObject);
                if (g.GetComponent<enemy_color>() == null)
                    g.AddComponent<enemy_color>();
                g.GetComponent<Water_start>().chosen = -1;
                int tmpr = rnd.Next(0, 32);
                int tmpg = rnd.Next(0, 32);
                int tmpb = rnd.Next(0, 32);
                g.GetComponent<enemy_color>().color = new Color32((byte)(tmpr + 32), (byte)(tmpg + 32), (byte)(tmpb + 192), 255);
                g.GetComponent<enemy_color>().Update_stat();
                g.transform.position = new Vector3(water_positions[i].Item1, water_positions[i].Item2, 0);
                enemyUnits.Add(g);
            }

            // 플레이어 유닛 복제
            List<GameObject> playerUnits = new List<GameObject>();
            for (int i = 0; i < 6; i++)
            {
                GameObject f = Instantiate(originalGrass.gameObject);
                f.transform.position = GlobalVariables.setballs[i].transform.position;
                if (f.GetComponent<my_color>() == null)
                    f.AddComponent<my_color>();
                f.GetComponent<Grass_start>().chosen = 1;
                int tmpr = rnd.Next(0, 32);
                int tmpg = rnd.Next(0, 32);
                int tmpb = rnd.Next(0, 32);
                f.GetComponent<my_color>().color = new Color32((byte)(tmpr + 32), (byte)(tmpg + 192), (byte)(tmpb + 32), 255);
                f.GetComponent<my_color>().Update_stat();
                Destroy(f.GetComponent<Grass_start>());
                playerUnits.Add(f);
            }

            // 기존 세 개 제거
            Destroy(originalGrass.gameObject);
            Destroy(originalFire.gameObject);
            Destroy(originalWater.gameObject);
        }
    }
}
