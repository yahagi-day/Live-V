using UnityEngine;
using UnityEngine.UI;
using UniRx.Async;
using VRMLoader;
using System;
using VRM;
using UnityEngine.Networking;

#if UNITY_EDITOR
using UnityEditor;
#else
using SFB;
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
        public static byte[] VRMdata;

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
                VRMpath = path;
            }
        }

        async UniTask LoadVRM(string path)
        {
#if UNITY_WEBGL
            VRMMetaObject meta;
            using (UnityWebRequest uwr = UnityWebRequest.Get(path))
            {
                await uwr.SendWebRequest();
                VRMdata = uwr.downloadHandler.data;
            }
            using (var context = new VRMImporterContext())
            {
                context.ParseGlb(VRMdata);
                meta = context.ReadMeta(true);
            }
#else
            var meta = await VRMMetaImporter.ImportVRMMeta(path, true);

#endif
            GameObject modalObject = Instantiate(modalWindowPrefabs, canvas.transform) as GameObject;
            var modalLocale = modalObject.GetComponentInChildren<VRMPreviewLocale>();
            modalLocale.SetLocale(language.captionText.text);

            var modalUi = modalObject.GetComponentInChildren<VRMPreviewUI>();

            modalUi.setMeta(meta);

            modalUi.setLoadable(true);
        }

        public static byte[] GetVRMData()
        {
            return VRMdata;
        }
        public static string GetVRMPath()
        {
            return VRMpath;
        }
    }

}