using UnityEngine;

public class CandleInteract : Interactable
{
    public CandleManger manager;

    public ParticleSystem candleFire;

    private bool activated = false;

    public override void Interact()
    {
        if (activated)
            return;

        activated = true;

        //  ‘€Ì· «·‰«—
        if (candleFire != null)
            candleFire.Play();

        // ≈—”«· ··„«‰Ã—
        if (manager != null)
            manager.CheckCandle(this);
    }
}