using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverTransition : MonoBehaviour
{
    public Image gameOverSprite;
    public float duration = 2f;

    [SerializeField] private float shakeDuration = 0.6f;
	[SerializeField] private float shakeMagnitude = 12f;      // pixel jitter
	[SerializeField] private float shakeRotation = 4f;        // degrees
	[SerializeField] private float shakeFrequency = 30f;      // oscillations per second
	[SerializeField] private AnimationCurve shakeDamping = AnimationCurve.EaseInOut(0, 1, 1, 0);
    public void StartGameOverTransition()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
        StartCoroutine(FadeInSprite());
    }

    

    private IEnumerator FadeInSprite(){
        gameOverSprite.gameObject.SetActive(true);
        Color c = gameOverSprite.color;
        c.a = 0;
        gameOverSprite.color = c;

        Vector2 startPos = gameOverSprite.rectTransform.anchoredPosition + new Vector2(0, 200);
        Vector2 endPos = gameOverSprite.rectTransform.anchoredPosition;
        while(c.a < 1){
            c.a += Time.unscaledDeltaTime;
            gameOverSprite.color = c;
            gameOverSprite.rectTransform.anchoredPosition = Vector3.Lerp(startPos, endPos, c.a);
            yield return null;
        }
        yield return StartCoroutine(ShakeSprite(gameOverSprite.rectTransform, shakeDuration, shakeMagnitude, shakeRotation, shakeFrequency));
    }
    private IEnumerator ShakeSprite(RectTransform rt, float dur, float magnitude, float rotDeg, float freq){
		Vector2 basePos = rt.anchoredPosition;
		Quaternion baseRot = rt.localRotation;

		float t = 0f;
		while (t < dur){
			t += Time.unscaledDeltaTime;
			float n = t / dur;
			float damper = shakeDamping.Evaluate(n); // 1 -> 0

			// Sine-based jitter
			float ang = 2f * Mathf.PI * freq * t;

			// Offset XY
			float offsetX = Mathf.Sin(ang) * magnitude * damper;
			float offsetY = Mathf.Cos(ang * 1.3f) * (magnitude * 0.6f) * damper;

			// Small rotation
			float zRot = Mathf.Sin(ang * 0.8f) * rotDeg * damper;

			rt.anchoredPosition = basePos + new Vector2(offsetX, offsetY);
			rt.localRotation = Quaternion.Euler(0, 0, zRot);

			yield return null;
		}

		// Reset
		rt.anchoredPosition = basePos;
		rt.localRotation = baseRot;
	}
}

