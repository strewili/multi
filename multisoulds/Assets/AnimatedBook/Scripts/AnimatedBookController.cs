using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AnimatedBookController : MonoBehaviour
{
    public GameManagerScript manager;

    public enum BOOK_STATE
    {
        CLOSED,
        OPENING,
        OPENED,
        CLOSING
    }

    public enum PAGES_TRANSITIONS
    {
        NONE,
        TURNING_RIGHT,
        TURNING_LEFT
    }

    [System.Serializable]
    public class PageObjects
    {
        public Transform page;
        public Image RectoImage;
        public Image VersoImage;

        [System.NonSerialized]
        public GameObject UiRecto;

        [System.NonSerialized]
        public GameObject UiVerso;
    }

    [System.Serializable]
    public class Page
    {
        public GameObject UiRecto;
        public Sprite RectoBackground;

        public GameObject UiVerso;
        public Sprite VersoBackground;
    }

    public Transform rightRotationReference;
    public Transform leftRotationReference;

    public Sprite defaultBackground;

    public Transform pagesParent;

    public Page[] pagesUi;

    [Range(1f, 10)]
    public float quickRotationSpeed = 2;

    public AudioClip pageTurnSound;

    public System.Action _onBookClose;
    public System.Action _onBookOpen;

    private PageObjects[] bookPages;

    private Quaternion pageUnturnedRotation;
    private Quaternion pageTurnedRotation;

    private BOOK_STATE state = BOOK_STATE.CLOSED;

    private Animator anim;

    private float t = 0;
    private float speed = 1;

    public int currentPage = 0;

    private PAGES_TRANSITIONS inTransition =
        PAGES_TRANSITIONS.NONE;

    public void SetOpened()
    {
        state = BOOK_STATE.OPENED;

        if (_onBookOpen != null)
        {
            _onBookOpen.Invoke();
        }
    }

    public void SetOpening()
    {
        state = BOOK_STATE.OPENING;
    }

    public void SetClosing()
    {
        state = BOOK_STATE.CLOSING;
    }

    public void SetClosed()
    {
        state = BOOK_STATE.CLOSED;

        if (_onBookClose != null)
        {
            _onBookClose.Invoke();
        }
    }

    public BOOK_STATE getBookState()
    {
        return state;
    }

    public PageObjects[] getPageObjects()
    {
        return bookPages;
    }

    void Start()
    {
        InitReferences();

        pageUnturnedRotation =
            Quaternion.Euler(0, 89, 0);

        pageTurnedRotation =
            Quaternion.Euler(0, 271, 0);

        bookPages[1].page.gameObject.SetActive(false);
        bookPages[2].page.gameObject.SetActive(false);

        if (pagesUi.Length == 0)
        {
            bookPages[0].page.gameObject.SetActive(false);
        }
        else
        {
            ActivatePage(0, currentPage);
        }
    }

    private void InitReferences()
    {
        anim = GetComponent<Animator>();

        bookPages = new PageObjects[3];

        for (int i = 0; i < 3; i++)
        {
            PageObjects page = new PageObjects();

            Transform pageTransform =
                pagesParent.Find("Page" + i);

            page.page = pageTransform;

            page.RectoImage =
                pageTransform
                .Find("Recto")
                .Find("CanvasRecto")
                .GetComponent<Image>();

            page.VersoImage =
                pageTransform
                .Find("Verso")
                .Find("CanvasVerso")
                .GetComponent<Image>();

            bookPages[i] = page;
        }
    }

    private void ActivatePage(int i, int pageIndex)
    {
        bookPages[i].page.gameObject.SetActive(true);

        if (bookPages[i].UiRecto != null)
        {
            Destroy(bookPages[i].UiRecto);
        }

        if (pagesUi[pageIndex].RectoBackground != null)
        {
            bookPages[i].RectoImage.sprite =
                pagesUi[pageIndex].RectoBackground;
        }
        else
        {
            bookPages[i].RectoImage.sprite =
                defaultBackground;
        }

        if (pagesUi[pageIndex].UiRecto != null)
        {
            bookPages[i].UiRecto =
                Instantiate(pagesUi[pageIndex].UiRecto);

            bookPages[i].UiRecto.transform.SetParent(
                bookPages[i].RectoImage.transform,
                false
            );
        }

        if (bookPages[i].UiVerso != null)
        {
            Destroy(bookPages[i].UiVerso);
        }

        if (pagesUi[pageIndex].VersoBackground != null)
        {
            bookPages[i].VersoImage.sprite =
                pagesUi[pageIndex].VersoBackground;
        }
        else
        {
            bookPages[i].VersoImage.sprite =
                defaultBackground;
        }

        if (pagesUi[pageIndex].UiVerso != null)
        {
            bookPages[i].UiVerso =
                Instantiate(pagesUi[pageIndex].UiVerso);

            bookPages[i].UiVerso.transform.SetParent(
                bookPages[i].VersoImage.transform,
                false
            );
        }
    }

    private void DeactivatePage(int i)
    {
        Destroy(bookPages[i].UiRecto);
        Destroy(bookPages[i].UiVerso);

        bookPages[i].page.gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        if (
            state == BOOK_STATE.OPENING ||
            state == BOOK_STATE.CLOSING
        )
        {
            for (int i = 0; i < bookPages.Length; i++)
            {
                if (
                    currentPage == 0 &&
                    leftRotationReference != null
                )
                {
                    bookPages[i].page.rotation =
                        leftRotationReference.rotation;
                }
                else if (rightRotationReference != null)
                {
                    bookPages[i].page.rotation =
                        rightRotationReference.rotation;
                }
            }
        }
    }

    private void PlayFlipPageSound()
    {
        if (pageTurnSound != null)
        {
            AudioSource.PlayClipAtPoint(
                pageTurnSound,
                transform.position
            );
        }
    }

    public void TurnToNextPage()
    {
        if (
            state == BOOK_STATE.CLOSED &&
            currentPage == 0
        )
        {
            anim.SetTrigger("OpenBook");
        }
        else if (state == BOOK_STATE.OPENED)
        {
            if (currentPage < pagesUi.Length)
            {
                if (
                    inTransition ==
                    PAGES_TRANSITIONS.TURNING_RIGHT
                )
                {
                    ImproveTransitionSpeed();
                }
                else
                {
                    StopAllCoroutines();

                    StartCoroutine(
                        "TurnToNextPageTransition"
                    );
                }
            }
            else
            {
                anim.SetTrigger("CloseBookLeft");
            }
        }
    }

    public void TurnToPreviousPage()
    {
        if (state == BOOK_STATE.CLOSED)
        {
            anim.SetTrigger("OpenBook");
        }
        else if (state == BOOK_STATE.OPENED)
        {
            if (currentPage >= 0)
            {
                if (
                    currentPage == 0 &&
                    inTransition ==
                    PAGES_TRANSITIONS.NONE
                )
                {
                    anim.SetTrigger("CloseBookRight");
                    return;
                }
                else if (
                    currentPage > 0 &&
                    inTransition ==
                    PAGES_TRANSITIONS.NONE
                )
                {
                    currentPage--;
                }

                if (
                    inTransition ==
                    PAGES_TRANSITIONS.TURNING_LEFT
                )
                {
                    ImproveTransitionSpeed();
                }
                else
                {
                    StopAllCoroutines();

                    StartCoroutine(
                        "TurnToPreviousPageTransition"
                    );
                }
            }
        }
    }

    public void ImproveTransitionSpeed()
    {
        speed = quickRotationSpeed;
    }

    IEnumerator TurnToNextPageTransition()
    {
        bool hasActivatedNextPage = false;

        PlayFlipPageSound();

        if (
            inTransition ==
            PAGES_TRANSITIONS.TURNING_LEFT
        )
        {
            t = 1 - t;
        }

        inTransition =
            PAGES_TRANSITIONS.TURNING_RIGHT;

        while (t < 1)
        {
            bookPages[currentPage % 3]
                .page.localRotation =
                Quaternion.Lerp(
                    pageUnturnedRotation,
                    pageTurnedRotation,
                    t
                );

            t += Time.deltaTime * speed;

            if (
                t > 0.05f &&
                !hasActivatedNextPage
            )
            {
                if (
                    currentPage + 1 <
                    pagesUi.Length
                )
                {
                    ActivatePage(
                        (currentPage + 1) % 3,
                        currentPage + 1
                    );

                    bookPages[(currentPage + 1) % 3]
                        .page.localRotation =
                        pageUnturnedRotation;
                }

                hasActivatedNextPage = true;
            }

            yield return new WaitForFixedUpdate();
        }

        bookPages[currentPage % 3]
            .page.localRotation =
            pageTurnedRotation;

        DeactivatePage((currentPage + 2) % 3);

        currentPage++;

        if (currentPage > pagesUi.Length)
        {
            currentPage = pagesUi.Length;
        }

        TransitionFinished();
    }

    IEnumerator TurnToPreviousPageTransition()
    {
        bool hasActivatedPreviousPage = false;

        bool hasDeactivatedNextPage = false;

        PlayFlipPageSound();

        if (
            inTransition ==
            PAGES_TRANSITIONS.TURNING_RIGHT
        )
        {
            t = 1 - t;
        }

        inTransition =
            PAGES_TRANSITIONS.TURNING_LEFT;

        while (t < 1)
        {
            bookPages[currentPage % 3]
                .page.localRotation =
                Quaternion.Lerp(
                    pageTurnedRotation,
                    pageUnturnedRotation,
                    t
                );

            t += Time.deltaTime * speed;

            if (
                t > 0.05f &&
                !hasActivatedPreviousPage
            )
            {
                if (currentPage > 0)
                {
                    ActivatePage(
                        (currentPage + 2) % 3,
                        currentPage - 1
                    );

                    bookPages[(currentPage + 2) % 3]
                        .page.localRotation =
                        pageTurnedRotation;
                }

                hasActivatedPreviousPage = true;
            }

            if (
                t > 0.95f &&
                !hasDeactivatedNextPage
            )
            {
                DeactivatePage(
                    (currentPage + 1) % 3
                );

                hasDeactivatedNextPage = true;
            }

            yield return new WaitForFixedUpdate();
        }

        bookPages[currentPage % 3]
            .page.localRotation =
            pageUnturnedRotation;

        TransitionFinished();
    }

    private void TransitionFinished()
    {
        t = 0;

        inTransition =
            PAGES_TRANSITIONS.NONE;

        speed = 1;

        if (manager == null)
            return;

        if (currentPage == 1)
        {
            manager.CompleteTask(
                GameManagerScript.TaskTypes.DESK
            );

            manager.TriggerEvent(
                GameManagerScript.EventTypes
                .AFTER_DIARY_FUSE_PAGE
            );
        }
        else if (currentPage == 2)
        {
            manager.TriggerEvent(
                GameManagerScript.EventTypes
                .AFTER_DIARY_MUSIC_PAGE
            );
        }
        else if (currentPage == 3)
        {
            manager.TriggerEvent(
                GameManagerScript.EventTypes
                .AFTER_DIARY_FLARE_GUN
            );
        }
    }
}