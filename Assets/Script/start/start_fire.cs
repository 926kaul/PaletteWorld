using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fire_start : MonoBehaviour
{
    public SpriteRenderer PlayerRender;
    public int chosen = 0;

    void OnMouseDown()
{
    if (chosen == 0)
    {
        chosen = 1;

        System.Random rnd = new System.Random();

        // 기존 불러오기
        Fire_start originalFire = this;
        Grass_start originalGrass = FindObjectOfType<Grass_start>();
        Water_start originalWater = FindObjectOfType<Water_start>();

        // Grass 제거 전에 복제
        (int,int)[] grass_positions = new (int,int)[] { (3, 15), (9, 15), (15, 15)};
        List<GameObject> enemyUnits = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            GameObject g = Instantiate(originalGrass.gameObject);
            if (g.GetComponent<enemy_color>() == null)
                g.AddComponent<enemy_color>();
            g.GetComponent<Grass_start>().chosen = -1;
            int tmpr = rnd.Next(0, 32);
            int tmpg = rnd.Next(0, 32);
            int tmpb = rnd.Next(0, 32);
            g.GetComponent<enemy_color>().color = new Color32((byte)(tmpr + 32), (byte)(tmpg + 192), (byte)(tmpb + 32), 255);
            g.GetComponent<enemy_color>().Update_stat();
            g.transform.position = new Vector3(grass_positions[i].Item1, grass_positions[i].Item2, 0);
            enemyUnits.Add(g);
        }

        // Fire 제거 전에 복제
        List<GameObject> playerUnits = new List<GameObject>();
        for (int i = 0; i < 6; i++)
        {
            GameObject f = Instantiate(originalFire.gameObject);
            f.transform.position = GlobalVariables.setballs[i].transform.position;
            if (f.GetComponent<my_color>() == null)
                f.AddComponent<my_color>();
            f.GetComponent<Fire_start>().chosen = 1;
            int tmpr = rnd.Next(0, 32);
            int tmpg = rnd.Next(0, 32);
            int tmpb = rnd.Next(0, 32);
            f.GetComponent<my_color>().color = new Color32((byte)(tmpr + 192), (byte)(tmpg + 32), (byte)(tmpb + 32), 255);
            f.GetComponent<my_color>().Update_stat();
            Destroy(f.GetComponent<Fire_start>()); // Fire_start 제거
            playerUnits.Add(f);
        }

        // 기존 세 개 제거
        Destroy(originalFire.gameObject);
        Destroy(originalGrass.gameObject);
        Destroy(originalWater.gameObject);
    }
}

}
