using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text _procent;
    [SerializeField] private int sceneIndexToLoad;
    [SerializeField] private Sprite[] animationFrames;

    private void Start()
    {
        StartCoroutine(LoadSceneAsync(sceneIndexToLoad));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // Обновление текста с процентами
            if (_procent != null)
            {
                _procent.text = $"{(int)(progress * 100)}%";
            }

            // Обновление анимации (спрайта)
            if (animationFrames != null && animationFrames.Length > 0)
            {
                int frameIndex = Mathf.Clamp((int)(progress * animationFrames.Length), 0, animationFrames.Length - 1);
                image.sprite = animationFrames[frameIndex];
            }

            yield return null;
        }

        // Обновляем на финальный кадр и 100% после 0.9
        _procent.text = "100%";
        image.sprite = animationFrames[animationFrames.Length - 1];

        // Небольшая пауза (по желанию)
        yield return new WaitForSeconds(0.3f);

        operation.allowSceneActivation = true;
    }
}
