using System.Collections;
using UnityEngine;

public static class Explosion
{
    public static IEnumerator Exploding(
        string prefabPath,
        Vector3 position,
        float growDuration,
        float fadeDuration,
        float maxScale,
        float startAlpha,
        float endAlpha,
        Color startColor,
        Color endColor)
    {
        GameObject prefab = Resources.Load<GameObject>(prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"이펙트 프리팹을 찾을 수 없습니다: {prefabPath}");
            yield break;
        }

        GameObject effect = GameObject.Instantiate(prefab, position, Quaternion.identity);
        SpriteRenderer sr = effect.GetComponent<SpriteRenderer>();

        float elapsed = 0f;

        // 성장 + 진해짐
        while (elapsed < growDuration)
        {
            float t = elapsed / growDuration;
            float scale = Mathf.Lerp(0f, maxScale, t);
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            Color color = Color.Lerp(startColor, endColor, t);
            color.a = alpha;

            effect.transform.localScale = new Vector3(scale, scale, 1f);
            sr.color = color;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 사라짐
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            float alpha = Mathf.Lerp(endAlpha, 0f, t);
            Color color = Color.Lerp(endColor, Color.red, t);
            color.a = alpha;

            sr.color = color;

            elapsed += Time.deltaTime;
            yield return null;
        }

        GameObject.Destroy(effect);
    }
}