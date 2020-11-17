using UnityEngine;

public class DisableAudio : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
#if UNITY_WEBGL
        gameObject.SetActive(false);
#endif
    }
}
