using UnityEngine;
using UnityEngine.UI;

public class SafeBoxMonitor : MonoBehaviour
{
    public Text text;

    public int currentCode = 0;

    void Update()
    {
        if (text != null)
        {
            text.text = currentCode.ToString();
        }
    }
}