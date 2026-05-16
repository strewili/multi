using UnityEngine;

public class FirstDoll : Interactable
{
    public DollPuzzle dollPuzzle;

    public override void Interact()
    {
        dollPuzzle.CollectFirstDoll();
    }
}