using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class CandleDoorPuzzle1 : MonoBehaviour
{
    [Header("Candles Order")]
    public CandleInteract[] candles;

    private int currentIndex = 0;
    private bool solved = false;

    [Header("Door")]
    public Animator doorAnimator;

    [Header("Player")]
    public MonoBehaviour playerController;

    [Header("Video")]
    public VideoPlayer videoPlayer;
    public GameObject videoCanvas;

    // =====================================================
    // Ì‰«œÌÂ «·‘„⁄
    // =====================================================

    public void CheckCandle(CandleInteract candle)
    {
        if (solved)
            return;

        // «· — Ì» «·’ÕÌÕ
        if (candle == candles[currentIndex])
        {
            currentIndex++;

            // «‰ ÂÏ «··€“
            if (currentIndex >= candles.Length)
            {
                solved = true;

                StartCoroutine(PlayCutscene());
            }
        }
        else
        {
            // ≈⁄«œ… «· — Ì»
            currentIndex = 0;

            Debug.Log("Wrong Candle Order");
        }
    }

    // =====================================================
    // «·ﬂ  ”Ì‰
    // =====================================================

    IEnumerator PlayCutscene()
    {
        // Êﬁ› «··«⁄»
        if (playerController != null)
            playerController.enabled = false;

        //  ‘€Ì· «·›ÌœÌÊ
        if (videoCanvas != null)
            videoCanvas.SetActive(true);

        if (videoPlayer != null)
            videoPlayer.Play();

        // «‰ Ÿ«— »”Ìÿ
        yield return new WaitForSeconds(2f);

        // › Õ «·»«»
        if (doorAnimator != null)
            doorAnimator.SetTrigger("Open");

        // „œ… «·›ÌœÌÊ
        yield return new WaitForSeconds(6f);

        // ≈Œ›«¡ «·›ÌœÌÊ
        if (videoCanvas != null)
            videoCanvas.SetActive(false);

        // —ÃÊ⁄ «· Õﬂ„
        if (playerController != null)
            playerController.enabled = true;
    }
}