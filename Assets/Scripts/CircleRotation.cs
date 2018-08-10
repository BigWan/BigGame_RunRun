using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRotation : MonoBehaviour {

    public float speed;

    private void FixedUpdate() {
        transform.Rotate(0, speed*Time.fixedDeltaTime, 0);
    }
}
