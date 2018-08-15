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
        [SerializeField]
        private RoadSectionData data;


        /// <summary>
        /// 起点的世界坐标
        /// </summary>
        public Vector3 localPosition;

        /// <summary>
        /// Section 本身的旋转
        /// </summary>
        public Quaternion localRotation;


        /// <summary>
        /// 终点的本地坐标
        /// </summary>
        public Vector3 offset;
        /// <summary>
        /// 终点的相对Section本身的偏转
        /// </summary>
        public Quaternion localYaw;

        // 调试用
        public Vector3 startEuler, endEuler;

        private void Update() {
            startEuler = localRotation.eulerAngles;
            endEuler = localYaw.eulerAngles;
        }


        /// <summary>
        /// 跑道已经生产完毕
        /// </summary>
        private bool isFinished;

        private int executeStepIndex;


        // components
        private BoxCollider col;
        private EndTriggerSpawner endSpawner;
        private List<Block> blocks;

        List<SpawnBlockCommand> commands {
            get {
                return data?.commands;
            }
        }

        private void Awake() {
            col = GetComponent<BoxCollider>();
            endSpawner = GetComponent<EndTriggerSpawner>();
            Init();
        }
        
        public void SetData(RoadSectionData data,Vector3 position, Quaternion rotation) {
            this.data = data;
            localPosition = position;
            localRotation = rotation;
            transform.localPosition = localPosition;
            transform.localRotation = localRotation;
        }


        /// <summary>
        /// 生成结束的触发器
        /// </summary>
        /// <param name="z"></param>
        public void SpawnEnd(float z) {
            endSpawner.SetEndPositionAndRoation(offset,localYaw);
            endSpawner.SpawnEnd();
        }

        

        /// <summary>
        /// 手动初始化
        /// </summary>
        void Init() {
            if (blocks != null) {
                foreach (var b in blocks) {
                    DestroyImmediate (b.gameObject);
                }
            }

            offset = Vector3.zero;
            localYaw = Quaternion.identity;

            blocks = new List<Block>();
            isFinished = false;
            executeStepIndex = 0;
            col.center = Vector3.Scale(col.center, new Vector3(1, 1, 0));
        }

        public float GetLength() {
            float length = 0;
            foreach (var block in blocks) {
                length += block.length;
            }
            return length;
        }

        //public Vector3 GetEndPosition() {
        //    return endLocalPosition;
        //}

        //public Quaternion GetEndRoation() {
        //    return endRotationChange;
        //}

        /// <summary>
        /// 执行命令列表
        /// </summary>
        public (Vector3,Quaternion) Execute(float coinRate = 0f) {

            if (isFinished) Init();

            if (commands == null || commands.Count <= 0) {
                Debug.LogError("命令队列为空，无法执行",transform);
                return(Vector3.zero, Quaternion.identity);
            }

            for (int i = 0; i < commands.Count; i++) {
                ExecuteCommand(commands[i],coinRate);
            }

            isFinished = true;
            SetBoxTrigger();

            return (offset, localYaw);
        }

        
        /// <summary>
        /// 一次执行一个command
        /// </summary>
        public void ExecuteStep(float coinRate = 0){

            if (isFinished) Init();

            if (commands == null || commands.Count <= 0) {
                Debug.LogError("命令队列为空，无法执行",transform);
                return;
            }

            if(executeStepIndex < commands.Count)
                ExecuteCommand(commands[executeStepIndex++],coinRate);
            else {
                isFinished = true;
                SetBoxTrigger();
            }
        }

        public void ExecuteCommand(SpawnBlockCommand cmd,float coinRate = 0) {

            int count = Random.Range(cmd.start,cmd.end+1);
            Block rndBlock;
            for (int i = 0; i < count; i++) {
                rndBlock = cmd.GetRandomBlock();
                if (rndBlock != null) {
                    //实例化一个Block
                    Block blockInstant = Instantiate(rndBlock) as Block;

                    // 设置Block参数
                    blockInstant.transform.SetParent(transform);

                    blockInstant.transform.localPosition = offset;

                    blockInstant.transform.localRotation = localYaw; // * blockInstant.exitPlugs[0].getRotation(); //TODO:没有考虑多出口的情况

                    localYaw =  blockInstant.localYaw * localYaw;
                    offset = offset + blockInstant.transform.localRotation * blockInstant.exitPlugs[0].transform.localPosition;

                    if (Random.value < coinRate)
                        blockInstant.SpawnCoin();

                    // 增加引用
                    AddBlock(blockInstant);
                } else {
                    continue;
                }
            }            
        }


        private void AddBlock(Block  block) {
            if (blocks == null) {
                blocks = new List<Block>();
            }
            blocks.Add(block);
        }

        /// <summary>
        /// 设置section的触发器
        /// </summary>
        void SetBoxTrigger() {
            float l = GetLength();
            col.center = new Vector3(col.center.x, col.center.y, l*0.5f);

            col.size = new Vector3(col.size.x, col.size.y, l);
        }

       

//#if UNITY_EDITOR
//        private void OnGUI() {           

//            if(GUI.Button(new Rect(530, 30, 100, 50),new GUIContent("执行"))) {
//                Execute();
//            }

//            if(GUI.Button(new Rect(130, 30, 100, 50),new GUIContent("执行单步"))) {
//                ExecuteStep(0);
//            }
//        }
//#endif


        public void SelfDestroy() {
            foreach (var b in blocks) {
                b.SelfDestroy();
            }
            Destroy(gameObject);
        }



        void OnTriggerExit(Collider other) {
            if(other.tag == "Player") {
                SelfDestroy();
                Track.Instance.SpawnNextSection();
            }
        }

    }



}
