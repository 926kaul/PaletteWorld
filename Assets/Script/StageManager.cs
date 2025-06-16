using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;
    
    public GameObject enemyPrefab;
    public int stageWidth = 18;
    public int stageHeight = 18;

    private int currentStage = 1;      // 📌 현재 스테이지
    private int enemiesLeft = 0;

    public start_button start_button;

    void Awake()
    {
        Instance = this;
    }
    public void SpawnEnemies(int count)
    {
        enemiesLeft = count;
        System.Random rnd = new System.Random();
        List<Vector2Int> usedPositions = new List<Vector2Int>();
        int maxTries = 100;

        for (int i = 0; i < count; i++)
        {
            Vector2Int pos;
            int tries = 0;

            // 1. 겹치지 않는 위치 찾기
            while (true)
            {
                int x = rnd.Next(0, stageWidth);
                int y = rnd.Next(stageHeight / 2, stageHeight); // 위쪽 절반
                pos = new Vector2Int(x, y);

                bool conflict = false;
                foreach (Vector2Int used in usedPositions)
                {
                    if (Vector2Int.Distance(used, pos) < 1f) // 거리 1 미만이면 겹쳤다고 판단
                    {
                        conflict = true;
                        break;
                    }
                }

                if (!conflict || ++tries > maxTries)
                    break;
            }

            usedPositions.Add(pos);

            // 2. 색깔 생성 (조건 만족)
            Color32 newColor;
            while (true)
            {
                byte r = (byte)rnd.Next(0, 256);
                byte g = (byte)rnd.Next(0, 256);
                byte b = (byte)rnd.Next(0, 256);
                bool tooDark = r <= 64 && g <= 64 && b <= 64;
                bool tooBright = r >= 192 && g >= 192 && b >= 192;
                newColor = new Color32(r, g, b, 255);

                if (!tooDark && !tooBright && every_skill.check_get_skill(newColor) != null)
                    break;
            }

            // 3. 적 생성
            GameObject e = Instantiate(enemyPrefab, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
            enemy_color ec = e.GetComponent<enemy_color>();

            ec.color = newColor;
            ec.Update_stat();
            ec.hp = 55 + 3 * ec.H;
            ec.Update_skill();
        }

    }

    public void OnEnemyDefeated()
    {
        enemiesLeft--;
        if (enemiesLeft <= 0)
        {
            StartCoroutine(NextStage());
        }
    }

    private IEnumerator NextStage()
    {
        Debug.Log($"Stage {currentStage} Clear!");
        
        my_color[] myColors = GameObject.FindObjectsOfType<my_color>();
        for (int i = 0; i < myColors.Length; i++)
        {
            if(GlobalVariables.setballs[i] == null)
                continue;
            myColors[i].transform.position = GlobalVariables.setballs[i].transform.position;
            myColors[i].stage_set = 0;
        }

        yield return new WaitForSeconds(1f);

        currentStage++; 
        start_button.started = false;
        SpawnEnemies(currentStage);
    }
}

