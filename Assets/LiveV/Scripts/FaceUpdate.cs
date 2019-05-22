using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRM;

namespace Live_V
{ 
	public class FaceUpdate : MonoBehaviour
	{
        private VRMBlendShapeProxy proxy;

        private void Start()
        {
            proxy = GetComponent<VRMBlendShapeProxy>();
        }

        public void OnCallChangeFace(string str)
        {
            if (str == "default@unitychan")
            {
                proxy.ImmediatelySetValue(BlendShapePreset.Neutral, 1.0f);
                proxy.ImmediatelySetValue(BlendShapePreset.Angry, 0);
                proxy.ImmediatelySetValue(BlendShapePreset.Fun, 0);
                proxy.ImmediatelySetValue(BlendShapePreset.Blink, 0);
            }

            if (str == "conf@unitychan")
                proxy.ImmediatelySetValue(BlendShapePreset.Angry, 1.0f);
            if (str == "smile3@unitychan")
                proxy.ImmediatelySetValue(BlendShapePreset.Fun, 1.0f);
            if (str == "eye_close@unitychan")
                proxy.ImmediatelySetValue(BlendShapePreset.Blink, 1.0f);
        }
    }
}
