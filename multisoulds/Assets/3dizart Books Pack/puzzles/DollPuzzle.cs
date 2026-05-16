using UnityEngine;

public class DollPuzzle : MonoBehaviour
{
    public GameObject firstDoll;
    public GameObject secondDoll;

    public Transform[] spawnPoints;

    public GameObject ellieGhost;

    public BookPuzzle bookPuzzle;

    private bool firstCollected = false;
    private bool secondCollected = false;

    void Start()
    {
        secondDoll.SetActive(false);
        ellieGhost.SetActive(false);
    }

    public void CollectFirstDoll()
    {
        if (firstCollected)
            return;

        firstCollected = true;

        firstDoll.SetActive(false);

        int randomIndex = Random.Range(0, spawnPoints.Length);

        secondDoll.transform.position = spawnPoints[randomIndex].position;

        secondDoll.SetActive(true);
    }

    public void CollectSecondDoll()
    {
        if (secondCollected)
            return;

        secondCollected = true;

        secondDoll.SetActive(false);

        ellieGhost.SetActive(true);

        Invoke(nameof(StartBookPuzzle), 3f);
    }

    void StartBookPuzzle()
    {
        bookPuzzle.EnablePuzzle();
    }
}