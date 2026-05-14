using UnityEngine;

public class LightControl : MonoBehaviour
{
    public bool lightOn = true;

    public Light[] allLights;

    public AudioClip buttonSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        UpdateLights();
    }

    public void switchLight()
    {
        lightOn = !lightOn;

        if (audioSource != null && buttonSound != null)
        {
            audioSource.clip = buttonSound;
            audioSource.Play();
        }

        UpdateLights();
    }

    void UpdateLights()
    {
        foreach (Light l in allLights)
        {
            l.enabled = lightOn;
        }
    }
}