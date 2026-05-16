using UnityEngine;

public class SecondDoll : Interactable
{
    public DollPuzzle dollPuzzle;

    public override void Interact()
    {
        dollPuzzle.CollectSecondDoll();
    }
}