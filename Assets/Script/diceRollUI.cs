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

    public IEnumerator Roll(int result, int hitScore)
    {   
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
    }
}