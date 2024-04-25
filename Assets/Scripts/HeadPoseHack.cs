using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadPoseHack : MonoBehaviour
{
    [SerializeField] Transform getPositionFrom;
    [SerializeField] Transform getRotationFrom;

    public void FixedUpdate()
    {
        transform.position = getPositionFrom.position;
        transform.rotation = getRotationFrom.rotation;
    }
}
 