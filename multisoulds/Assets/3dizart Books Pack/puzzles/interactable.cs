
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Interactable()
    {
    }

    public virtual void Interact()
    {
        Debug.Log("Interacted");
    }
}