using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Live_V
{
    public class StartCameraPos : MonoBehaviour
    {
        private void Awake()
        {
            GameObject.Find("StageDirector").GetComponent<StageDirector>().SwitchCamera(2);
        }

    }
}
