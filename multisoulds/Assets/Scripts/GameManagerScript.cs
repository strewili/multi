using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance;

    // =====================================================
    // PLAYER
    // =====================================================

    [Header("Player")]
    public MonoBehaviour playerController;
    public GameObject player;
    public Camera playerCamera;

    // =====================================================
    // UI
    // =====================================================

    [Header("UI")]
    public GameObject interactionUI;
    public TextMeshProUGUI interactionText;

    [Header("Notes UI")]
    public GameObject noteCanvas;

    // =====================================================
    // AUDIO
    // =====================================================

    [Header("Audio")]
    public AudioSource ambienceSource;
    public AudioSource jumpscareSource;

    public AudioClip roomAmbience;

    public AudioClip whisperSound;
    public AudioClip knockSound;
    public AudioClip jumpscareSound;

    // =====================================================
    // LIGHTS
    // =====================================================

    [Header("Global Lights")]
    public Light hallwayLight;
    public Light flickerLight;

    [Header("Post Processing")]
    public Volume globalVolume;

    // =====================================================
    // VIDEO
    // =====================================================

    [Header("Cutscene")]
    public VideoPlayer videoPlayer;
    public GameObject videoCanvas;

    // =====================================================
    // PUZZLES
    // =====================================================

    [Header("Puzzle States")]
    public bool puzzle1Solved;
    public bool puzzle2Solved;
    public bool puzzle3Solved;
    public bool puzzle4Solved;

    // =====================================================
    // EVENTS
    // =====================================================

    [Header("Scares")]
    public GameObject maraGhost;
    public Animator hallwayDoor;

    private bool canInteract = true;

    // =====================================================
    // START
    // =====================================================

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        HideInteraction();

        if (noteCanvas != null)
            noteCanvas.SetActive(false);

        if (videoCanvas != null)
            videoCanvas.SetActive(false);

        if (maraGhost != null)
            maraGhost.SetActive(false);
    }

    // =====================================================
    // INTERACTION UI
    // =====================================================

    public void ShowInteraction(string text)
    {
        if (interactionUI != null)
            interactionUI.SetActive(true);

        if (interactionText != null)
            interactionText.text = text;
    }

    public void HideInteraction()
    {
        if (interactionUI != null)
            interactionUI.SetActive(false);
    }

    // =====================================================
    // NOTES
    // =====================================================

    public void OpenNote()
    {
        canInteract = false;

        if (playerController != null)
            playerController.enabled = false;

        if (noteCanvas != null)
            noteCanvas.SetActive(true);
    }

    public void CloseNote()
    {
        canInteract = true;

        if (playerController != null)
            playerController.enabled = true;

        if (noteCanvas != null)
            noteCanvas.SetActive(false);
    }

    // =====================================================
    // ROOM AMBIENCE
    // =====================================================

    public void PlayRoomAmbience()
    {
        if (ambienceSource != null && roomAmbience != null)
        {
            ambienceSource.clip = roomAmbience;
            ambienceSource.Play();
        }
    }

    // =====================================================
    // PUZZLES COMPLETE
    // =====================================================

    public void CompletePuzzle1()
    {
        puzzle1Solved = true;

        PlayKnock();
    }

    public void CompletePuzzle2()
    {
        puzzle2Solved = true;

        FlickerLights();
    }

    public void CompletePuzzle3()
    {
        puzzle3Solved = true;

        StartCoroutine(MaraHallwayScare());
    }

    public void CompletePuzzle4()
    {
        puzzle4Solved = true;

        StartCoroutine(EndingCutscene());
    }

    // =====================================================
    // LIGHT EFFECTS
    // =====================================================

    public void FlickerLights()
    {
        StartCoroutine(FlickerCoroutine());
    }

    IEnumerator FlickerCoroutine()
    {
        for (int i = 0; i < 6; i++)
        {
            hallwayLight.enabled = !hallwayLight.enabled;

            yield return new WaitForSeconds(0.1f);
        }

        hallwayLight.enabled = true;
    }

    // =====================================================
    // AUDIO EVENTS
    // =====================================================

    public void PlayWhisper()
    {
        jumpscareSource.PlayOneShot(whisperSound);
    }

    public void PlayKnock()
    {
        jumpscareSource.PlayOneShot(knockSound);
    }

    public void PlayJumpscare()
    {
        jumpscareSource.PlayOneShot(jumpscareSound);
    }

    // =====================================================
    // MARA EVENT
    // =====================================================

    IEnumerator MaraHallwayScare()
    {
        PlayWhisper();

        yield return new WaitForSeconds(1f);

        if (maraGhost != null)
            maraGhost.SetActive(true);

        FlickerLights();

        yield return new WaitForSeconds(2f);

        if (maraGhost != null)
            maraGhost.SetActive(false);
    }

    // =====================================================
    // CUTSCENE
    // =====================================================

    IEnumerator EndingCutscene()
    {
        if (playerController != null)
            playerController.enabled = false;

        if (videoCanvas != null)
            videoCanvas.SetActive(true);

        if (videoPlayer != null)
            videoPlayer.Play();

        yield return new WaitForSeconds((float)videoPlayer.length);

        if (videoCanvas != null)
            videoCanvas.SetActive(false);

        if (playerController != null)
            playerController.enabled = true;
    }

    // =====================================================
    // DOOR EVENTS
    // =====================================================

    public void OpenHallwayDoor()
    {
        if (hallwayDoor != null)
            hallwayDoor.SetTrigger("Open");
    }

    // =====================================================
    // UPDATE
    // =====================================================

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseNote();
        }
    }
    // =====================================================
    // OLD SYSTEM SUPPORT
    // =====================================================

    public enum EventTypes
    {
        AFTER_DIARY_FUSE_PAGE,
        AFTER_DIARY_MUSIC_PAGE,
        AFTER_DIARY_FLARE_GUN,
        CURTAIN_OPEN,
        EXIT_BATHROOM,
        AFTER_CLIP_BOARD,
        HELI_ARRIVED
    }

    public enum TaskTypes
    {
        DESK,
        CURTAIN
    }

    public void TriggerEvent(EventTypes e)
    {
        Debug.Log("Triggered Event: " + e);

        switch (e)
        {
            case EventTypes.CURTAIN_OPEN:
                PlayKnock();
                break;

            case EventTypes.HELI_ARRIVED:
                PlayWhisper();
                break;

            case EventTypes.AFTER_DIARY_FLARE_GUN:
                PlayJumpscare();
                break;
        }
    }

    public void TriggerEvent(EventTypes e, float delay)
    {
        StartCoroutine(TriggerEventDelay(e, delay));
    }

    IEnumerator TriggerEventDelay(EventTypes e, float delay)
    {
        yield return new WaitForSeconds(delay);

        TriggerEvent(e);
    }

    public void CompleteTask(TaskTypes t)
    {
        Debug.Log("Completed Task: " + t);

        switch (t)
        {
            case TaskTypes.DESK:
                puzzle1Solved = true;
                break;

            case TaskTypes.CURTAIN:
                puzzle2Solved = true;
                break;
        }
    }
}