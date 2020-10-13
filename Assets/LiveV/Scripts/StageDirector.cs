using UniRx.Async;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using VRM;

namespace Live_V
{
    public class StageDirector : MonoBehaviour
    {
        //Prefab読み込み
        public GameObject MusicPlayer;
        public GameObject MainCameraRig;
        public GameObject SubCameraRig;
        public GameObject[] prefabsNeedsActivation;
        public GameObject[] miscPrefabs;
        public GameObject LipSync;

        //Cameraの場所
        public Transform[] cameraPoints;
        public GameObject Canvas;

        ScreenOverlay screenoverlays;

        //Instatiate後操作用
        GameObject musicPlayer;
        CameraSwitcher mainCameraSwitcher;
        CameraSwitcher SubCameraRigContoller;
        GameObject[] objectsNeedsActivation;
        private GameObject PropActivatorObjects;
        GameObject VRMAvaterController;
        GameObject LipsSyncContoller;
        VRMImporterContext context;
        Vector3 DefaulteyePos;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                bool active = !Canvas.activeSelf;
                Canvas.SetActive(active);
            }
        }

#if UNITY_WEBGL
        private void Awake()
#else
        private async UniTask Awake()
#endif
        {
            if(SceneManager.GetActiveScene().name == "Underground")
            {
                DefaulteyePos =  new Vector3(0, 0.7f, 0); //シヴィちゃん基準
            }
            if(SceneManager.GetActiveScene().name == "UnityChanCRS")
            {
                DefaulteyePos = new Vector3(0, 0.7f, 0);
            }
            //PrefabをInstantiateするよ!!
            musicPlayer = (GameObject)Instantiate(MusicPlayer);

            var cameraRig = (GameObject)Instantiate(MainCameraRig);
            mainCameraSwitcher = cameraRig.GetComponentInChildren<CameraSwitcher>();
            screenoverlays = cameraRig.GetComponentInChildren<ScreenOverlay>();

            objectsNeedsActivation = new GameObject[prefabsNeedsActivation.Length];
            for (var i = 0; i < prefabsNeedsActivation.Length; i++)
                objectsNeedsActivation[i] = (GameObject)Instantiate(prefabsNeedsActivation[i]);
            
#if UNITY_WEBGL
            VRMAvaterController = LoadVRMAvater();
#else
            VRMAvaterController = await LoadVRMAvater();
#endif
            if(SceneManager.GetActiveScene().name == "DeepDusk")
            {
                VRMAvaterController.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            mainCameraSwitcher.GetComponentInChildren<CameraSwitcher>().vrm = VRMAvaterController;
            var VRMAnimator = VRMAvaterController.GetComponent<Animator>();
            var eye = transform.TransformPoint(VRMAnimator.GetBoneTransform(HumanBodyBones.LeftEye).transform.position);
            var eyediff = eye - DefaulteyePos;
            var campos = cameraRig.GetComponentInChildren<FindObject>().FindGameObject();
            foreach (Transform child in transform) child.position += eyediff;
            foreach (Transform child in campos.transform) child.position += eyediff;
            cameraRig.SetActive(true);

            if (SubCameraRig != null)
            {
                var subcamerarig = (GameObject) Instantiate(SubCameraRig);
                SubCameraRigContoller = subcamerarig.GetComponentInChildren<CameraSwitcher>();
                SubCameraRigContoller.vrm = VRMAvaterController;
                subcamerarig.SetActive(true);
            }

            LipsSyncContoller = (GameObject)Instantiate(LipSync);
            LipsSyncContoller.GetComponent<LipSyncController>().target = VRMAvaterController.GetComponent<VRMBlendShapeProxy>();

            foreach (var p in miscPrefabs)Instantiate(p);

            GetComponent<Animator>().enabled = true;
            
        }

#if UNITY_WEBGL
        public GameObject LoadVRMAvater()
#else
        public async UniTask<GameObject> LoadVRMAvater()
#endif
        {
            var path = VRMLoadUniRx.GetVRMPath();
            if (path == null){path = Application.streamingAssetsPath + "/Avater/model.vrm";}
            byte[] VRMByteData;
            GameObject go;
#if UNITY_WEBGL
            VRMByteData = VRMLoadUniRx.GetVRMData();
#else
            using (var uwr = UnityWebRequest.Get(path))
            {
                await uwr.SendWebRequest();
                VRMByteData = uwr.downloadHandler.data;
            }
#endif
            context = new VRMImporterContext();
            context.ParseGlb(VRMByteData);
#if UNITY_WEBGL
            context.Load();
#else
            await context.LoadAsyncTask();
#endif
            go =  context.Root;
            context.ShowMeshes();
           
            go.AddComponent<Blinker>();
            go.AddComponent<FaceUpdate>();
            var animator = go.GetComponent<Animator>();
            animator.applyRootMotion = true;
            animator.runtimeAnimatorController = (RuntimeAnimatorController)Instantiate(Resources.Load("MocapC86"));
            go.SetLayerRecursively(8);            

            return go;
        }

        public void StartMusic()
        {
            foreach (var source in musicPlayer.GetComponentsInChildren<AudioSource>())
                source.Play();
        }

        public void ActivateProps()
        {
            foreach (var o in objectsNeedsActivation) o.BroadcastMessage("ActivateProps");
        }


        public void SwitchCamera(int index)
        {
            if (mainCameraSwitcher)
                mainCameraSwitcher.ChangePosition(cameraPoints[index], true);
        }

        public void StartAutoCameraChange()
        {
            if (mainCameraSwitcher)
                mainCameraSwitcher.StartAutoChange();
        }

        public void StopAutoCameraChange()
        {
            if (mainCameraSwitcher)
                mainCameraSwitcher.StopAutoChange();
        }

        public void  SwitchOverlays()
        {
            screenoverlays.enabled = true;
        }

        public void FastForward(float second)
        {
            /*if (false)
            {
                FastForwardAnimator(GetComponent<Animator>(), second, 0);
                foreach (var go in objectsOnTimeline)
                    foreach (var animator in go.GetComponentsInChildren<Animator>())
                        FastForwardAnimator(animator, second, 0.5f);
            }*/
        }

        void FastForwardAnimator(Animator animator, float second, float crossfade)
        {
            for (var layer = 0; layer < animator.layerCount; layer++)
            {
                var info = animator.GetCurrentAnimatorStateInfo(layer);
                if (crossfade > 0.0f)
                    animator.CrossFade(info.fullPathHash, crossfade / info.length, layer, info.normalizedTime + second / info.length);
                else
                    animator.Play(info.fullPathHash, layer, info.normalizedTime + second / info.length);
            }
        }

        public void EndPerformance()
        {
            /*
            Application.LoadLevel(0);
            SceneManager.LoadScene(0);
            screenoverlays.enabled = true;
            context.Dispose();
            Destroy(LipsSyncContoller);
            Destroy(mainCameraSwitcher);
            foreach (var p in objectsNeedsActivation)
                Destroy(p);
            */
            Canvas.SetActive(true);
        }

    }

    public static class GameObjectExtensions
    {
        /// <summary>
        /// 自分自身を含むすべての子オブジェクトのレイヤーを設定します
        /// </summary>
        public static void SetLayerRecursively(
            this GameObject self,
            int layer
        )
        {
            self.layer = layer;

            foreach (Transform n in self.transform)
            {
                SetLayerRecursively(n.gameObject, layer);
            }
        }
    }
}