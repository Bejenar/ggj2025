using System.Collections;
using TMPro;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public float animationDuration = 0.5f;
    public float maxScale = 1.5f;

    private void Start()
    {
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TMP_Text>();
        }
    }

    public IEnumerator AnimateText()
    {
        string originalText = textMeshPro.text;
        for (int i = 0; i < originalText.Length; i++)
        {
            string before = originalText.Substring(0, i);
            string current = originalText.Substring(i, 1);
            string after = originalText.Substring(i + 1);

            textMeshPro.text = before + $"<size={maxScale * 100}%>" + current + "</size>" + after;
            yield return StartCoroutine(ScaleLetter(i));
        }
        textMeshPro.text = originalText;
    }

    private IEnumerator ScaleLetter(int index)
    {
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            float scale = Mathf.Lerp(1f, maxScale, elapsedTime / animationDuration);
            textMeshPro.text = textMeshPro.text.Insert(index + 1, $"<size={scale * 100}%>");
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            float scale = Mathf.Lerp(maxScale, 1f, elapsedTime / animationDuration);
            textMeshPro.text = textMeshPro.text.Insert(index + 1, $"<size={scale * 100}%>");
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}