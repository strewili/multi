using UnityEngine;
using System.Collections;

public class DrawerInteract : Interactable
{
    [Header("Drawer Settings")]
    public Vector3 openOffset;

    public float speed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private bool isOpen = false;
    private bool moving = false;

    void Start()
    {
        closedPosition = transform.localPosition;
        openPosition = closedPosition + openOffset;
    }

    public override void Interact()
    {
        if (moving)
            return;

        if (isOpen)
            StartCoroutine(MoveDrawer(closedPosition));
        else
            StartCoroutine(MoveDrawer(openPosition));

        isOpen = !isOpen;
    }

    IEnumerator MoveDrawer(Vector3 target)
    {
        moving = true;

        while (Vector3.Distance(transform.localPosition, target) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                target,
                Time.deltaTime * speed
            );

            yield return null;
        }

        transform.localPosition = target;

        moving = false;
    }
}