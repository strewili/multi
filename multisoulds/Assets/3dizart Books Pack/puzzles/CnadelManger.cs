using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class CandleManger : MonoBehaviour
{
    [Header("Candles Order")]
    public CandleInteract[] candles;

    private int currentIndex = 0;
    private bool solved = false;

    [Header("Door")]
    public Animator doorAnimator;

    [Header("Player")]
    public FPSController playerController;

    [Header("Video")]
    public VideoPlayer videoPlayer;
    public GameObject videoCanvas;

    // =========================================
    // Ì Õﬁﬁ „‰ «·‘„⁄
    // =========================================

    public void CheckCandle(CandleInteract candle)
    {
        if (solved)
            return;

        // ≈–« «·‘„⁄… ’Õ
        if (candle == candles[currentIndex])
        {
            currentIndex++;

            // Œ·’ «··€“
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

    // =========================================
    // «·ﬂ  ”Ì‰
    // =========================================

    IEnumerator PlayCutscene()
    {
        // Êﬁ› «·Õ—ﬂ…
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // ≈ŸÂ«— «·›ÌœÌÊ
        if (videoCanvas != null)
        {
            videoCanvas.SetActive(true);
        }

        //  ‘€Ì· «·›ÌœÌÊ
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }

        // «‰ Ÿ«— ﬁ»· › Õ «·»«»
        yield return new WaitForSeconds(2f);

        // › Õ «·»«»
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open");
        }

        // «‰ Ÿ«— ‰Â«Ì… «·›ÌœÌÊ
        yield return new WaitForSeconds(6f);

        // ≈Œ›«¡ «·›ÌœÌÊ
        if (videoCanvas != null)
        {
            videoCanvas.SetActive(false);
        }

        // —ÃÊ⁄ «·Õ—ﬂ…
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }
}