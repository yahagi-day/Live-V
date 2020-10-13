using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectStage : MonoBehaviour
{
    Dictionary<int, string> StageName;
    private void Start()
    {
        StageName = new Dictionary<int, string>()
        {
            {1, "Underground" },
            {2, "UnityChanCRS" },
        };
    }

    public void NextScene(int num)
    {
        var SceneName = StageName[num];
        SceneManager.LoadSceneAsync(SceneName);
    }
}
