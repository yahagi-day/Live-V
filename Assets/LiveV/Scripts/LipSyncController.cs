using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using VRM;

public class LipSyncController : MonoBehaviour
{
    //public GameObject Avater;

    public Transform nodeA;
    public Transform nodeE;
    public Transform nodeI;
    public Transform nodeO;
    public Transform nodeU;

    public AnimationCurve weightCurve;

    [SerializeField] float w;

    public VRMBlendShapeProxy target;

    void Start()
    {
        //Avater = GameObject.Find("シヴィ");
        //target = Avater.GetComponent<VRMBlendShapeProxy>();
    }

    float GetWeight(Transform tr)
    {
        return weightCurve.Evaluate(tr.localPosition.z);
    }

    void LateUpdate()
    {
        var total = 10.0f;

        w = total * GetWeight(nodeA);
        target.ImmediatelySetValue(BlendShapePreset.A, w);
        total -= w;

        w = total * GetWeight(nodeI);
        target.ImmediatelySetValue(BlendShapePreset.I, w);
        total -= w;

        w = total * GetWeight(nodeU);
        target.ImmediatelySetValue(BlendShapePreset.U, w);
        total -= w;

        w = total * GetWeight(nodeE);
        target.ImmediatelySetValue(BlendShapePreset.E, w);
        total -= w;

        w = total * GetWeight(nodeO);
        target.ImmediatelySetValue(BlendShapePreset.O, w);
    }
}
