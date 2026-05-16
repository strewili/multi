using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainScript : MonoBehaviour
{
    public GameObject shutterSwitch;

    void Start()
    {

    }

    void Update()
    {

    }

    public void EnableSwitch()
    {
        GameManagerScript manager =
            GameObject.Find("GameManager").GetComponent<GameManagerScript>();

        if (manager != null)
        {
            manager.CompleteTask(
                GameManagerScript.TaskTypes.CURTAIN
            );

            manager.TriggerEvent(
                GameManagerScript.EventTypes.CURTAIN_OPEN
            );
        }
    }

    public void PlaySound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}