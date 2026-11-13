using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;

    public void PlayClickSound()
    {
        AudioManager.Instance.PlaySound(clickSound);
    }
}

