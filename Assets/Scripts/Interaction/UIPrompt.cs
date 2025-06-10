using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UIPrompt : MonoBehaviour
{
    [SerializeField] private TMP_Text _promptText;
    [SerializeField] private float _fadeDuration = 0.3f;

    private Coroutine _fadeCoroutine;

    public void ShowPrompt(string text)
    {
        _promptText.text = text;

        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeIn());
    }

    public void HidePrompt()
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float alpha = 0;
        _promptText.color = new Color(_promptText.color.r, _promptText.color.g, _promptText.color.b, alpha);

        while (alpha < 1)
        {
            alpha += Time.deltaTime / _fadeDuration;
            _promptText.color = new Color(_promptText.color.r, _promptText.color.g, _promptText.color.b, alpha);
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float alpha = 1;

        while (alpha > 0)
        {
            alpha -= Time.deltaTime / _fadeDuration;
            _promptText.color = new Color(_promptText.color.r, _promptText.color.g, _promptText.color.b, alpha);
            yield return null;
        }
    }
}