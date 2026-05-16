using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Camera playerCamera;

    public float interactDistance = 3f;

    public KeyCode interactKey = KeyCode.E;

    public GameObject interactUI;

    public MonoBehaviour playerMovement;

    private bool canMove = true;

    void Update()
    {
        if (!canMove)
            return;

        Ray ray =
            playerCamera.ViewportPointToRay(
                new Vector3(0.5f, 0.5f)
            );

        RaycastHit hit;

        bool detected =
            Physics.Raycast(
                ray,
                out hit,
                interactDistance
            );

        if (interactUI != null)
        {
            interactUI.SetActive(false);
        }

        if (detected)
        {
            Interactable interactable =
                hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                if (interactUI != null)
                {
                    interactUI.SetActive(true);
                }

                if (Input.GetKeyDown(interactKey))
                {
                    interactable.Interact();
                }
            }
        }
    }

    public void StopPlayer()
    {
        canMove = false;

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;
    }

    public void ResumePlayer()
    {
        canMove = true;

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;
    }
}