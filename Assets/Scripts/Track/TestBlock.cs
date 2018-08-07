using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {
    public class TestBlock : MonoBehaviour {


        public List<Block> allBlocks;



        // Update is called once per frame
        void Update() {



        }

        private void OnGUI() {
            if (GUI.Button(new Rect(30, 30, 100, 100), new GUIContent("随机一下"))) {
                foreach (var b in allBlocks) {
                    b.SpawnNextPlug(transform);
                }
            }
        }
    } }
