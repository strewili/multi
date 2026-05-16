using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathroomExitTrigger : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "BodyCollider")
        {
            GameManagerScript manager =
                GameObject.Find("GameManager").GetComponent<GameManagerScript>();

            if (manager != null)
            {
                manager.TriggerEvent(
                    GameManagerScript.EventTypes.EXIT_BATHROOM
                );
            }
        }
    }
}