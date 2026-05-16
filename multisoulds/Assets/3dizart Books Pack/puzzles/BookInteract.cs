using UnityEngine;

public class BookInteract : Interactable
{
    public BookPuzzle bookPuzzle;

    public int bookNumber;

    public override void Interact()
    {
        bookPuzzle.PressBook(bookNumber);
    }
}