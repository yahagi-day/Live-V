using SFB;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using VRM;
using VRMLoader;

namespace Live_V
{
    public class VRMLoadUniRx : MonoBehaviour
    {
        [SerializeField]
        GameObject modalWindowPrefabs;

        [SerializeField]
        Dropdown language;

        public static string VRMpath;
        public static byte[] VRMdata;

        public async void DefaultVRM()
        {
            var path = Application.streamingAssetsPath + "/Avater/model.vrm";
            Debug.Log(path);
            await LoadVRM(path);
            VRMpath = path;
        }

        private async UniTask LoadJson(string url)
        {
            byte[] data;
            VRMMetaObject meta;
            using (var uwr = UnityWebRequest.Get(url))
            {
                await uwr.SendWebRequest();
                data = uwr.downloadHandler.data;
            }
            using (var context = new VRMImporterContext())
            {
                context.ParseGlb(VRMdata);
                meta = context.ReadMeta(true);
            }

            SetVRMmeta(meta);
        }
        [DllImport("__Internal")]
        private static extern void FileImporterCaptureClick();

#if UNITY_WEBGL
        public void OpenVRM()
        {
            FileImporterCaptureClick();
            Debug.Log("Call FileImporter");
        }
        public async void FileSelected(string url)
        {
            Debug.Log(url);
            await LoadVRM(url);
        }
#else
        public async void OpenVRM()
        {
            var extensions = new[] { new ExtensionFilter("VRM Files", "vrm") };
            var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
            var path = paths[0];            
            if (path.Length != 0)
            {
                await LoadVRM(path);
                VRMpath = path;
            }
        }
#endif
        
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
            SetVRMmeta(meta);
        }
        void SetVRMmeta(VRMMetaObject meta)
        {
            GameObject modalObject = modalWindowPrefabs;
            modalObject.SetActive(true);
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