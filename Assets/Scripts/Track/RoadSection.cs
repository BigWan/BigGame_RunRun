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

        [SerializeField]
        private float length;

        private EndTriggerSpawner endSpawner;

        public void SetData(RoadSectionData data) {
            this.data = data;
        }


        List<SpawnBlockCommand> commands {
            get {
                return data?.commands;
            }
        }



        private List<Block> blocks;

        /// <summary>
        /// Block结尾的Plug位置和朝向
        /// </summary>
        private Vector3 endPlugLocalPosition;

        private Quaternion endPlugLocalRotation = Quaternion.identity;

        /// <summary>
        /// 跑道已经生产完毕
        /// </summary>
        private bool isFinished;


        private int executeStepIndex;


        private BoxCollider col;

        private void Awake() {
            col = GetComponent<BoxCollider>();
            endSpawner = GetComponent<EndTriggerSpawner>();
        }

        public void SpawnEnd(float z) {
            endSpawner.SetEndPositionAndRoation(endPlugLocalPosition,endPlugLocalRotation);
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


            blocks = new List<Block>();
            endPlugLocalPosition = Vector3.zero;
            endPlugLocalRotation = Quaternion.identity;
            isFinished = false;
            executeStepIndex = 0;
            col.center = Vector3.Scale(col.center, new Vector3(1, 1, 0));
        }

        public float getLength() {
            float length = 0;
            foreach (var block in blocks) {
                length += block.length;
            }
            this.length = length;
            return length;
        }

        public Vector3 getEndPosition() {
            return endPlugLocalPosition;
        }

        public Quaternion getEndRoation() {
            return endPlugLocalRotation;
        }

        /// <summary>
        /// 执行命令列表
        /// </summary>
        public void Execute(float coinRate = 0f) {
            if (isFinished) Init();
            if (commands == null || commands.Count <= 0) {
                Debug.LogError("命令队列为空，无法执行",transform);
                return;
            }

            for (int i = 0; i < commands.Count; i++) {
                ExecuteCommand(commands[i],coinRate);
            }
            isFinished = true;
            SetBoxTrigger();
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
            if(executeStepIndex<commands.Count)
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
                    blockInstant.transform.localRotation = endPlugLocalRotation;// * blockInstant.exitPlugs[0].getRotation(); //TODO:没有考虑多出口的情况
                    endPlugLocalRotation =  blockInstant.exitPlugs[0].getRotation() * endPlugLocalRotation;
                    blockInstant.transform.localPosition = endPlugLocalPosition;
                    endPlugLocalPosition += blockInstant.transform.localRotation * blockInstant.exitPlugs[0].transform.localPosition;

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
            getLength();
            col.center = new Vector3(col.center.x, col.center.y, length*0.5f);
            col.size = new Vector3(col.size.x, col.size.y, length);
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
