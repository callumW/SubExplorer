
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    public float mainSpeed = 1f;
    public float rotateSpeed = 0.1f;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddRelativeForce(Vector3.forward * mainSpeed, ForceMode.Impulse);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddRelativeForce(Vector3.back * mainSpeed, ForceMode.Impulse);
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.AddRelativeTorque(Vector3.up * (0 - rotateSpeed), ForceMode.Impulse);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.AddRelativeTorque(Vector3.up * rotateSpeed, ForceMode.Impulse);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            rb.AddRelativeForce(Vector3.up * mainSpeed, ForceMode.Impulse);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rb.AddRelativeForce(Vector3.down * mainSpeed, ForceMode.Impulse);
        }
    }
}
