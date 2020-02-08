using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehaviour : MonoBehaviour
{
    public Transform targetObject;

    public float smoothTime = 10f;

    public Vector3 offset;
    private Vector3 velocity = Vector3.one;


    // Ran after Update
    void LateUpdate()
    {
        Vector3 toPos = targetObject.position + (targetObject.rotation * offset);
        Vector3 curPos = Vector3.SmoothDamp(transform.position, toPos, ref velocity, smoothTime);

        transform.position = curPos;

        transform.LookAt(targetObject, targetObject.up);
    }
}
