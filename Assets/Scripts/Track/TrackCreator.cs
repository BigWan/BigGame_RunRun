using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace RunRun {

    public class TrackCreator : MonoBehaviour {


        /// <summary>
        /// 生成出来的所有的Blocks
        /// </summary>
        public List<Block> currentBlocks = new List<Block>();

        /// <summary>
        /// 正在生成的块
        /// </summary>
        public List<Block> spawingBlocks = new List<Block>();


        /// <summary>
        /// 场景的出口
        /// </summary>
        public List<Block> endBlocks = new List<Block>();


        /// <summary>
        /// 跑道的起始点
        /// </summary>
        public Block startBlock;


        /// <summary>
        /// 是否允许拐弯
        /// </summary>
        public bool allowTurn;

        private Coroutine sc;

        private void Awake() {
            spawingBlocks.Add(startBlock);
            currentBlocks.Add(startBlock);
        }

        public IEnumerator StartSpawn() {

            Block current;
            List<Block> spawned = new List<Block>();
            
            while (spawingBlocks.Count > 0) {
               
                yield return new WaitForSeconds(0.2f);
                // 获取队列中一个拉出来生产
                current = spawingBlocks[spawingBlocks.Count-1];
                spawingBlocks.RemoveAt(spawingBlocks.Count - 1);

                Debug.Log("队列数量" + spawingBlocks.Count);
                if (current == null) {
                    Debug.Log("停止了", transform);
                    yield break;
                }
                // 生产出来的东西
                spawned = current.SpawnNextPlug(transform);
                //Debug.Log(current.GetInstanceID(),transform);
                Debug.Log("产出" + spawned.Count);
                //foreach (var item in spawned) {
                //    item.transform.SetParent(transform);
                //}

                // 生产出来的东西同时放回队列和列表
                currentBlocks.Union(spawned);
                for (int i = 0; i < spawned.Count; i++) {
                    spawingBlocks.Add(spawned[i]);
                }

            }           

        }


        private void OnGUI() {
            if(GUI.Button(new Rect(30,30,100,30),new GUIContent("生成"))) {
                sc = StartCoroutine( StartSpawn());
            }
            if (GUI.Button(new Rect(30, 80, 100, 30), new GUIContent("停止"))) {
                StopCoroutine (sc);
            }
        }


    }

}
