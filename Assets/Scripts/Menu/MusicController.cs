using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour
{
    [Header("Configuração da música")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float fadeInDuration = 3f;
    [SerializeField] private float fadeOutDuration = 2f;
    [SerializeField] private float targetVolume = 0.25f;

    private Coroutine fadeCoroutine;

    private void Start()
    {
        musicSource.volume = 0f;
        musicSource.Play();

        FadeIn();
    }

    public void FadeIn()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(
            FadeMusic(0f, targetVolume, fadeInDuration)
        );
    }

    public void FadeOut()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(
            FadeMusic(musicSource.volume, 0f, fadeOutDuration)
        );
    }

    private IEnumerator FadeMusic(float start, float end, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / duration);
            musicSource.volume = Mathf.Lerp(start, end, t);

            yield return null;
        }

        musicSource.volume = end;

        if (end == 0f)
            musicSource.Stop();

        fadeCoroutine = null;
    }
}
