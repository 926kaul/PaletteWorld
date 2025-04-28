using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using System;

public class diceRollUI : MonoBehaviour
{
    public SpriteRenderer diceRenderer;
    public Sprite[] diceSprites;
    public TextMeshPro hitScoreText;

    public float flashDuration = 0.5f;
    public float flashSpeed = 0.05f;
    public bool isRolling = false;

    public SpriteRenderer diceRenderer1;
    public SpriteRenderer diceRenderer2;

    public IEnumerator Roll(int result, int hitScore)
    {   
        isRolling = true;
        Monitor.instance?.Clear();
        diceRenderer.enabled = true;

        if (hitScoreText != null)
        {
            hitScoreText.gameObject.SetActive(true);
            hitScoreText.text = $"{Math.Max(hitScore,1)}";
        }

        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            int random = UnityEngine.Random.Range(0, 20);
            diceRenderer.sprite = diceSprites[random];
            elapsed += flashSpeed;
            yield return new WaitForSeconds(flashSpeed);
        }

        diceRenderer.sprite = diceSprites[result - 1];

        yield return new WaitForSeconds(0.5f);

        diceRenderer.enabled = false;
        if (hitScoreText != null)
            hitScoreText.gameObject.SetActive(false);
        isRolling = false;
    }

    public IEnumerator AdvantageRoll(int result1, int result2, int hitScore)
    {
        isRolling = true;
        Monitor.instance?.Clear();
        diceRenderer1.enabled = true;
        diceRenderer2.enabled = true;

        if (hitScoreText != null)
        {
            hitScoreText.gameObject.SetActive(true);
            hitScoreText.text = $"{Math.Max(hitScore, 1)}";
        }

        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            int random1 = UnityEngine.Random.Range(0, 20);
            int random2 = UnityEngine.Random.Range(0, 20);

            diceRenderer1.sprite = diceSprites[random1];
            diceRenderer2.sprite = diceSprites[random2];

            elapsed += flashSpeed;
            yield return new WaitForSeconds(flashSpeed);
        }

        // 실제 결과를 적용
        diceRenderer1.sprite = diceSprites[result1 - 1];
        diceRenderer2.sprite = diceSprites[result2 - 1];

        // 0.5초 기다렸다가
        yield return new WaitForSeconds(0.5f);

        // 더 작은 주사위는 끄거나 투명하게 만들기
        if (result1 >= result2)
        {
            // result1이 더 크니까 diceRenderer2를 흐리게
            diceRenderer2.color = new Color(1f, 1f, 1f, 0.3f); // 알파값 낮춰서 흐릿하게
        }
        else
        {
            diceRenderer1.color = new Color(1f, 1f, 1f, 0.3f);
        }

        yield return new WaitForSeconds(0.5f);

        // 모든 것 끄기
        diceRenderer1.enabled = false;
        diceRenderer2.enabled = false;
        if (hitScoreText != null)
            hitScoreText.gameObject.SetActive(false);
        diceRenderer1.color = Color.white; // 다음을 위해 복구
        diceRenderer2.color = Color.white;
        isRolling = false;
    }

    public IEnumerator DisadvantageRoll(int result1, int result2, int hitScore)
    {
        isRolling = true;
        Monitor.instance?.Clear();
        diceRenderer1.enabled = true;
        diceRenderer2.enabled = true;

        if (hitScoreText != null)
        {
            hitScoreText.gameObject.SetActive(true);
            hitScoreText.text = $"{Math.Max(hitScore, 1)}";
        }

        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            int random1 = UnityEngine.Random.Range(0, 20);
            int random2 = UnityEngine.Random.Range(0, 20);

            diceRenderer1.sprite = diceSprites[random1];
            diceRenderer2.sprite = diceSprites[random2];

            elapsed += flashSpeed;
            yield return new WaitForSeconds(flashSpeed);
        }

        // 실제 결과 표시
        diceRenderer1.sprite = diceSprites[result1 - 1];
        diceRenderer2.sprite = diceSprites[result2 - 1];

        yield return new WaitForSeconds(0.5f);

        // 이번에는 작은 값 쪽을 선택
        if (result1 <= result2)
        {
            // result1이 더 작으니까 result2를 흐리게
            diceRenderer2.color = new Color(1f, 1f, 1f, 0.3f);
        }
        else
        {
            diceRenderer1.color = new Color(1f, 1f, 1f, 0.3f);
        }

        yield return new WaitForSeconds(0.5f);

        // 마무리
        diceRenderer1.enabled = false;
        diceRenderer2.enabled = false;
        if (hitScoreText != null)
            hitScoreText.gameObject.SetActive(false);
        diceRenderer1.color = Color.white;
        diceRenderer2.color = Color.white;
        isRolling = false;
    }
}