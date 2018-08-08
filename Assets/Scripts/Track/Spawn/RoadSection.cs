using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace RunRun {

    /// <summary>
    /// 有若干Block构成的一个路段
    /// </summary>
    public class RoadSection : MonoBehaviour {

        /// <summary>
        /// 配置文件
        /// </summary>
        public RoadSectionData data;
        //commands;

        List<SpawnBlockCommand> commands {
            get {
                return data?.commands;
            }
        }


        /// <summary>
        /// 当前生产点
        /// </summary>
        private Vector3 currentSpawnPosition;

        private Quaternion currentRotation;

        /// <summary>
        /// 执行命令列表
        /// </summary>
        public void Execute() {

            if (commands == null || commands.Count <= 0) {
                Debug.LogError("命令队列为空，无法执行",transform);
                return;
            }

            for (int i = 0; i < commands.Count; i++) {
                ExecuteCommand(commands[i]);
            }
        }

        public int executeIndex;
        public void ExecuteStep(){
            if (commands == null || commands.Count <= 0) {
                Debug.LogError("命令队列为空，无法执行",transform);
                return;
            }
            if(executeIndex<commands.Count)
                ExecuteCommand(commands[executeIndex++]);
        }

        public void ExecuteCommand(SpawnBlockCommand cmd) {

            int count = Random.Range(cmd.start,cmd.end);
            Block rndBlock;
            for (int i = 0; i < count; i++) {
                rndBlock = cmd.GetRandomBlock();
                if (rndBlock != null) {
                    Block go = Instantiate(rndBlock) as Block;
                    go.transform.SetParent(transform);

                    go.transform.localRotation = currentRotation * go.exitPlugs[0].getRotation();
                    currentRotation = go.transform.localRotation;

                    go.transform.localPosition = currentSpawnPosition;
                    currentSpawnPosition += currentRotation * go.exitPlugs[0].transform.localPosition;

                } else {
                    continue;
                }
            }
        }




        private void OnGUI() {

            if(GUI.Button(new Rect(530, 30, 100, 50),new GUIContent("执行"))) {
                Execute();
            }

            if(GUI.Button(new Rect(130, 30, 100, 50),new GUIContent("执行"))) {
                ExecuteStep();
            }
        }




    }



}
