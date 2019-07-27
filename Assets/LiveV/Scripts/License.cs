using System.Text;
using UniRx.Async;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class License : MonoBehaviour
{
    async UniTask Start()
    {
        var text = gameObject.GetComponent<Text>();
        byte[] data;
        using (UnityWebRequest uwr = UnityWebRequest.Get(Application.streamingAssetsPath + "/License.txt"))
        {
            await uwr.SendWebRequest();
            data = uwr.downloadHandler.data;
        }
        text.text = Encoding.Unicode.GetString(data);
    }
}
