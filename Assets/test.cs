using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    public RunRun.Track a ;




    private void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            a.SpawnNextSection();
        }
    }
}
