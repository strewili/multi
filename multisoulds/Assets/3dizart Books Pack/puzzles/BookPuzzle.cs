using UnityEngine;



public class BookPuzzle : MonoBehaviour
{
    [Header("Correct Order")]
    public int[] correctOrder;

    [Header("Door")]
    public DoorCodePuzzle doorPuzzle;

    private int currentIndex = 0;

    private bool puzzleEnabled = false;

    // =========================================

    public void EnablePuzzle()
    {
        puzzleEnabled = true;

        Debug.Log("Book Puzzle Started");
    }

    // =========================================

    public void PressBook(int number)
    {
        if (!puzzleEnabled)
            return;

        // «· — Ì» ’ÕÌÕ
        if (number == correctOrder[currentIndex])
        {
            currentIndex++;

            Debug.Log("Correct Book");

            // Œ·’ «··€“
            if (currentIndex >= correctOrder.Length)
            {
                CompletePuzzle();
            }
        }
        else
        {
            // ≈⁄«œ… «· — Ì»
            currentIndex = 0;

            Debug.Log("Wrong Order");
        }
    }

    // =========================================

    void CompletePuzzle()
    {
        Debug.Log("Book Puzzle Complete");

        doorPuzzle.UnlockDoor();
    }
}