using UnityEngine;

public class KeyHoldTrigger : MonoBehaviour
{
    public GameObject keyPos;
    public GameObject key;

    private bool executed = false;

    void Start()
    {
        keyPos.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!executed && other.gameObject == key)
        {
            executed = true;

            keyPos.SetActive(true);

            key.SetActive(false);

            GetComponentInParent<Animator>().SetTrigger("Open");
        }
    }
}