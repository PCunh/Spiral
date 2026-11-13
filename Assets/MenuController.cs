
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("Transition")]
    [SerializeField] private Image fadeOverlay;
    [SerializeField] private float fadeDuration = 1.5f;

    [Header("Scenes")]
    [SerializeField] private string gameSceneName = "Game";

    private bool isTransitioning;

    private void Start()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (creditsPanel != null)
            creditsPanel.SetActive(false);

        if (fadeOverlay != null)
        {
            fadeOverlay.gameObject.SetActive(true);
            SetFadeAlpha(1f);
            StartCoroutine(FadeIn());
        }
    }

    public void PlayGame()
    {
        if (!isTransitioning)
            StartCoroutine(LoadGame());
    }

    public void NovoJogo()
    {
        if (!isTransitioning)
            StartCoroutine(IniciarNovoJogo());
    }

    private IEnumerator IniciarNovoJogo()
    {
        isTransitioning = true;

        // Faz a transicao antes de iniciar o jogo.
        yield return Fade(0f, 1f);

        // Apaga o progresso de todos os puzzles.
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);

        if (creditsPanel != null)
            creditsPanel.SetActive(false);
    }

    public void OpenCredits()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(true);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void ClosePanels()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (creditsPanel != null)
            creditsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        Debug.Log("Sair do jogo funciona na versao compilada.");
#endif
    }

    private IEnumerator LoadGame()
    {
        isTransitioning = true;

        yield return Fade(0f, 1f);

        SceneManager.LoadScene(gameSceneName);
    }

    private IEnumerator FadeIn()
    {
        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float start, float end)
    {
        if (fadeOverlay == null)
            yield break;

        float elapsed = 0f;
        SetFadeAlpha(start);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / fadeDuration);
            t = Mathf.SmoothStep(0f, 1f, t);

            SetFadeAlpha(Mathf.Lerp(start, end, t));

            yield return null;
        }

        SetFadeAlpha(end);
    }

    private void SetFadeAlpha(float alpha)
    {
        if (fadeOverlay == null)
            return;

        Color color = fadeOverlay.color;
        color.a = alpha;
        fadeOverlay.color = color;
    }
}
