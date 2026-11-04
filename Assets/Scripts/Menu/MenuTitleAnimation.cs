
using UnityEngine;

public class MenuTitleAnimation : MonoBehaviour
{
    [SerializeField] private float pulseSpeed = 0.8f;
    [SerializeField] private float pulseAmount = 0.025f;

    private Vector3 initialScale;

    private void Start()
    {
        initialScale = transform.localScale;
    }

    private void Update()
    {
        float pulse = 1f +
            Mathf.Sin(Time.unscaledTime * pulseSpeed) * pulseAmount;

        transform.localScale = initialScale * pulse;
    }
}
