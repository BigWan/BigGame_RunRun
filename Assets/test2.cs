using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2 : MonoBehaviour {


    public Quaternion a = Quaternion.identity;

    public Vector3 el;




	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.A))
            a *= Quaternion.Euler(0, -90, 0);
        el = a.eulerAngles;
	}
}
