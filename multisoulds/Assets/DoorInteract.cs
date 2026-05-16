using UnityEngine;

using UnityEngine;

public class DoorInteract : Interactable
{
    public Animator doorAnimator;

    bool opened = false;

    public override void Interact()
    {
        opened = !opened;

        if (doorAnimator != null)
        {
            doorAnimator.SetBool(
                "Open",
                opened
            );
        }
    }
}