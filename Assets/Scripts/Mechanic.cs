using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanic : MonoBehaviour
{
    private ABlock _head;

    public void SetHeadBlock(ABlock block)
    {
        _head = block;
        block.transform.parent = transform;
        block.transform.localPosition = Vector3.zero;
        block.transform.localRotation = Quaternion.identity;
    }
}
