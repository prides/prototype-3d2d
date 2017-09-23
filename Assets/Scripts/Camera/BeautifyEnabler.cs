using UnityEngine;

public class BeautifyEnabler : MonoBehaviour
{
    private void Awake()
    {
        BeautifyEffect.Beautify beautify = GetComponent<BeautifyEffect.Beautify>();
        if (null != beautify)
        {
            beautify.enabled = true;
        }
    }
}