using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class PanelCodePuzzle : MonoBehaviour
{
    [Header("Door")]
    public Animator doorAnimator;

    [Header("Player")]
    public MonoBehaviour playerController;

    [Header("Video")]
    public VideoPlayer videoPlayer;
    public GameObject videoCanvas;

    private bool opened = false;

    // =====================================================
    // Ì ›⁄· »⁄œ Õ· ·€“ «·»«‰·
    // =====================================================

    public void UnlockDoor()
    {
        if (opened)
            return;

        opened = true;

        StartCoroutine(PlayCutscene());
    }

    // =====================================================
    // «·ﬂ  ”Ì‰
    // =====================================================

    IEnumerator PlayCutscene()
    {
        // Êﬁ› Õ—ﬂ… «··«⁄»
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