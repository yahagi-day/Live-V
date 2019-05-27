using UnityEngine;
using UnityEngine.UI;
using UniRx.Async;
using VRMLoader;
using SFB;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Live_V
{
    public class VRMLoadUniRx : MonoBehaviour
    {
        [SerializeField, Header("GUI")]
        Canvas canvas;

        [SerializeField]
        GameObject modalWindowPrefabs;

        [SerializeField]
        Dropdown language;

        public static string VRMpath;

        public async void DefaultVRM()
        {
            var path = Application.streamingAssetsPath + "/Avater/model.vrm";
            await LoadVRM(path);
            VRMpath = path;
        }

        public async void OpenVRM()
        {
#if UNITY_EDITOR
            var path = EditorUtility.OpenFilePanel("Open VRM file", "", "vrm");
#else
            var extensions = new[] { new ExtensionFilter("VRM Files", "vrm") };
            var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
            var path = paths[0];
            
#endif

            if(path.Length != 0)
            {
                await LoadVRM(path);
                Debug.Log(path);
                VRMpath = path;
            }
        }

        async UniTask LoadVRM(string path)
        {
            Debug.Log(path);
            var meta = await VRMMetaImporter.ImportVRMMeta(path, true);

            GameObject modalObject = Instantiate(modalWindowPrefabs, canvas.transform) as GameObject;
            Debug.Log("ここまで来た");
            var modalLocale = modalObject.GetComponentInChildren<VRMPreviewLocale>();
            modalLocale.SetLocale(language.captionText.text);

            var modalUi = modalObject.GetComponentInChildren<VRMPreviewUI>();
            modalUi.setMeta(meta);

            modalUi.setLoadable(true);
        }

        public static string GetVRMPath()
        {
            return VRMpath;
        }
    }
}