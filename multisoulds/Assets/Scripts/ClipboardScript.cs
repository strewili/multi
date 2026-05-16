using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipboardScript : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void sendDropTrigger()
    {
        GameManagerScript manager =
            GameObject.Find("GameManager").GetComponent<GameManagerScript>();

        if (manager != null)
        {
            manager.TriggerEvent(
                GameManagerScript.EventTypes.AFTER_CLIP_BOARD
            );
        }
    }
}