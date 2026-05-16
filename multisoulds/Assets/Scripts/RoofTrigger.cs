using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofTrigger : MonoBehaviour
{
    public Animator Helicopter;

    private bool executed = false;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider c)
    {
        if (!executed && c.name == "flarebullet(Clone)")
        {
            executed = true;

            if (Helicopter != null)
            {
                Helicopter.SetTrigger("Fly");
            }

            gameObject.SetActive(false);

            GameManagerScript manager =
                GameObject.Find("GameManager").GetComponent<GameManagerScript>();

            if (manager != null)
            {
                manager.TriggerEvent(
                    GameManagerScript.EventTypes.HELI_ARRIVED
                );
            }
        }
    }
}