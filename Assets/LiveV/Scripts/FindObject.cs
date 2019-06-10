using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Live_V
{
    public class FindObject : MonoBehaviour
    {
        public GameObject FindGameObject()
        {
            return transform.gameObject;
        }

    }
}