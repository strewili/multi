using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UIContent;

public class GameManagerScript : MonoBehaviour
{
    public bool debug = false;

    public enum TaskTypes
    {
        RELEASE,
        RADIO,
        DESK,
        FIND_FUSE,
        FIND_MUSIC_BOX,
        FIND_FLARE,
        LIGHT,
        KEY_HINT,
        KEY,
        ICE,
        AMMO,
        CURTAIN
    }

    public enum EventTypes
    {
        NULL,
        WAKE_UP,
        TRY_TO_RELEASE,
        CUTTER_CUT,
        EXIT_BATHROOM,
        AFTER_RADIO_MILITARY,
        AFTER_FLASHLIGHT,
        AFTER_PICK_UP_DIARY,
        AFTER_DIARY_FUSE_PAGE,
        AFTER_DIARY_MUSIC_PAGE,
        AFTER_DIARY_FLARE_GUN,
        OPENED_FUSE_COVER,
        AFTER_LIGHT_ON,
        MUSIC_BOX_TOUCHED,
        PICKED_UP_CLIPBOARD,
        AFTER_CLIP_BOARD,
        ICE_CUBE_TOUCHED,
        PICKED_UP_KEY,
        MUSIC_BOX_KEY_INSERTED,
        SECRETE_DOOR_OPEN,
        ENTERED_SECRET_ROOM,
        PICKED_UP_GUN,
        DOOR_CLOSED_WHILE_IN,
        EXIT_SECRET_ROOM,
        SAFEBOX_CABINET_OPEN,
        SAFEBOX_OPEN,
        GUN_LOADED,
        CURTAIN_OPEN,
        GLASS_BROKEN,
        FLARE_GUN_FIRED,
        HELI_ARRIVED,
        ESCAPED,
        FAILED,
        GAS_STOVE,
    }

    public HashSet<TaskTypes> completedTasks;
    public HashSet<TaskTypes> activeTasks;

    public GameObject UIDisplaySystem;

    private HashSet<EventTypes> triggeredEvents;

    void Start()
    {
        completedTasks = new HashSet<TaskTypes>();
        activeTasks = new HashSet<TaskTypes>();
        triggeredEvents = new HashSet<EventTypes>();
    }

    public void TriggerTask(TaskTypes task, EventTypes afterUI = EventTypes.NULL, float extraDelay = 0.2f)
    {
        StartCoroutine(TriggerDelay(task,
            afterUI == EventTypes.NULL
            ? 0 + extraDelay
            : getAudioLength(afterUI) + extraDelay));
    }

    IEnumerator TriggerDelay(TaskTypes task, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!activeTasks.Contains(task) && !completedTasks.Contains(task))
        {
            log("Trigger: " + task.ToString());

            activeTasks.Add(task);

            if (UIDisplaySystem != null)
            {
                UIDisplaySystem.SendMessage(
                    "updateTasks",
                    new object[] { GetTasks(), task },
                    SendMessageOptions.DontRequireReceiver
                );
            }
        }
    }

    public void CompleteTask(TaskTypes task)
    {
        if (!completedTasks.Contains(task))
        {
            log("Complete: " + task.ToString());

            activeTasks.Remove(task);
            completedTasks.Add(task);

            if (UIDisplaySystem != null)
            {
                UIDisplaySystem.SendMessage(
                    "completeTask",
                    new object[] { GetTasks(), task },
                    SendMessageOptions.DontRequireReceiver
                );
            }
        }
    }

    public string GetTasks()
    {
        string tasks = "";

        foreach (TaskTypes task in activeTasks)
        {
            tasks += " - " + TaskToUI[task] + "\n";
        }

        if (activeTasks.Count == 0)
        {
            return "";
        }

        return tasks.Substring(0, tasks.Length - 1);
    }

    private void log(string s)
    {
        if (debug)
        {
            Debug.Log(s);
        }
    }

    public void TriggerEvent(EventTypes e, float delay = 0)
    {
        if (!triggeredEvents.Contains(e))
        {
            triggeredEvents.Add(e);

            if (e == EventTypes.ESCAPED || e == EventTypes.FAILED)
            {
                StartCoroutine(TriggerUIEventDelay(e, delay));
            }
            else
            {
                StartCoroutine(TriggerEventVoiceDelay(e, delay));
            }
        }
    }

    IEnumerator TriggerUIEventDelay(EventTypes e, float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        string words = EventToUI[e];
        if (UIDisplaySystem != null)
        {
            UIDisplaySystem.SendMessage(
                "updateEventUI",
                words,
                SendMessageOptions.DontRequireReceiver
            );
        }
    }

    IEnumerator TriggerEventVoiceDelay(EventTypes e, float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        if (UIDisplaySystem != null)
        {
            UIDisplaySystem.SendMessage(
                "updateEventNarrative",
                EventToVoice[e],
                SendMessageOptions.DontRequireReceiver
            );
        }
    }

    public static int getUILength(EventTypes e)
    {
        return Mathf.Max(
            EventToUI[e].Split(' ').Length / 2,
            UIContent.UI_MIN_DELAY_SECONDS
        );
    }

    public static float getAudioLength(EventTypes e)
    {
        return EventToVoice[e].length;
    }
}