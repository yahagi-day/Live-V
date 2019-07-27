using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyMe : MonoBehaviour
{
    public void Active()
    {
        gameObject.SetActive(true);
    }
    public void destroyMe()
    {
        gameObject.SetActive(false);
    }

    public void NextScene()
    {
        SceneManager.LoadSceneAsync("LoadVRM");
    }
}
