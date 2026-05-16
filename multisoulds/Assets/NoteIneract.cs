using UnityEngine;

public class NoteInteract : Interactable
{
    public GameObject noteUI;

    public override void Interact()
    {
        if (noteUI != null)
        {
            noteUI.SetActive(true);
        }
    }
}