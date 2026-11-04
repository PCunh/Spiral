
using System.Collections;
using UnityEngine;

public class RandomAmbientSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;

    [SerializeField] private float minInterval = 8f;
    [SerializeField] private float maxInterval = 22f;

    [SerializeField, Range(0f, 1f)]
    private float volume = 0.35f;

    private IEnumerator Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null || clips == null || clips.Length == 0)
        {
            Debug.LogWarning("Configure o AudioSource e os clips.", this);
            yield break;
        }

        audioSource.loop = false;
        audioSource.playOnAwake = false;

        while (true)
        {
            yield return new WaitForSeconds(
                Random.Range(minInterval, maxInterval)
            );

            AudioClip clip = clips[Random.Range(0, clips.Length)];

            audioSource.PlayOneShot(clip, volume);
        }
    }
}
