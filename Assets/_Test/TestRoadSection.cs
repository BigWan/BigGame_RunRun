using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRoadSection : MonoBehaviour {

    public RunRun.RoadSection[] sections;

    void Spawn() {
        foreach (var s in sections) {
            s.Execute();
        }
    }


    private void OnGUI() {
        if(GUI.Button(new Rect(10, 10, 100, 30),"测试")) {
            Spawn();
        }
    }
}
