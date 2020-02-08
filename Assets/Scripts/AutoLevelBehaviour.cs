using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLevelBehaviour : MonoBehaviour
{
    public float smoothTime = 1f;
    private Vector3 velocity;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 curRot = transform.eulerAngles;
        Vector3 toRot = new Vector3(0f, curRot.y, 0f);

        transform.eulerAngles = toRot;
    }
}
