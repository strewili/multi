using UnityEngine;

public class PanelPuzzle : MonoBehaviour
{
    public int[] correctOrder;
    private int currentIndex = 0;

    public GameObject door;

    public void PressButton(int buttonNumber)
    {
        // ÇáÒÑ ÇáÕÍíÍ
        if (buttonNumber == correctOrder[currentIndex])
        {
            currentIndex++;

            // ÎáÕ ÇáÊÑÊíÈ
            if (currentIndex >= correctOrder.Length)
            {
                Debug.Log("Puzzle Solved");

                if (door != null)
                    door.SetActive(false);
            }
        }
        else
        {
            // ÅÚÇÏÉ
            currentIndex = 0;

            Debug.Log("Wrong Order");
        }
    }
}