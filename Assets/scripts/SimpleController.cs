using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour {
    public float horizontalSpeed = 1.0F;
    public float verticalSpeed = 1.0F;
    void Update()
    {
        float h = horizontalSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        float v = verticalSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        transform.Translate(v, h, 0);
    }
}
