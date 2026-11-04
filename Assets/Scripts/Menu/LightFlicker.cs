using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    [Header("Light Reference")]
    [SerializeField] private Light2D targetLight;

    [Header("Intensity")]
    [SerializeField] private float minIntensity = 0.15f;
    [SerializeField] private float maxIntensity = 0.8f;

    [Header("Timing")]
    [SerializeField] private float minInterval = 0.04f;
    [SerializeField] private float maxInterval = 0.3f;

    private float timer;
    private float targetIntensity;

    private void Start()
    {
        if (targetLight == null)
            targetLight = GetComponent<Light2D>();

        if (targetLight == null)
        {
            Debug.LogWarning(
                "LightFlicker: nenhuma Light2D atribuída.",
                this
            );

            enabled = false;
            return;
        }

        targetIntensity = maxIntensity;
        targetLight.intensity = maxIntensity;

        timer = Random.Range(minInterval, maxInterval);
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            targetIntensity = Random.Range(
                minIntensity,
                maxIntensity
            );

            timer = Random.Range(
                minInterval,
                maxInterval
            );
        }

        targetLight.intensity = Mathf.MoveTowards(
            targetLight.intensity,
            targetIntensity,
            Time.deltaTime * 5f
        );
    }
}
