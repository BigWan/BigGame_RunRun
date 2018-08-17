using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

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


        private readonly UnityEvent ExecuteFinishedHandler = new UnityEvent();

        /// <summary>
        /// 段落的连接点的坐标(在段落内部,起点是0,0,0)
        /// </summary>
        private Vector3 jointInsidePosition;

        /// <summary>
        /// 段落的连接点坐标,计算过段落的旋转和段落自身的坐标
        /// </summary>
        public Vector3 jointOutsidePosition {
            get {
                return transform.localRotation * jointInsidePosition + transform.localPosition;
            }
        }


        /// <summary>
        /// Section本身的朝向
        /// </summary>
        public TurnDirection direction;

        /// <summary>
        /// 终点的朝向,不管Section内部如何弯弯绕,只管终点的相对朝向.
        /// </summary>
        private TurnDirection jointInsideDirection;
        
        public TurnDirection jointOutsideDirection {
            get { return TurnDirectionUtil.Turn(direction,jointInsideDirection); }
        }


        /// <summary>
        /// 跑道已经生产完毕
        /// </summary>
        private bool isFinished;

        


        // components
        private BoxCollider exitTrigger; // 出口触发器
        private EndTriggerSpawner endSpawner;
        private List<Block> blocks;

        List<SpawnBlockCommand> commands {
            get {
                return data?.commands;
            }
        }

        private void Awake() {
            exitTrigger = GetComponent<BoxCollider>();
            endSpawner = GetComponent<EndTriggerSpawner>();
            ExecuteFinishedHandler.AddListener(OnExecuteFinished);
            Init();
        }
        
        public void SetData(RoadSectionData data,Vector3 position, TurnDirection direction) {
            this.data = data;
            transform.localPosition = position;
            this.direction = direction;
            transform.eulerAngles = TurnDirectionUtil.ToEuler(direction);
        }



        /// <summary>
        /// 生成结束的触发器
        /// </summary>
        /// <param name="z"></param>
        public void SpawnEnd(float z) {
            endSpawner.SetEndPositionAndRoation(jointInsidePosition,jointInsideDirection);
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
                blocks.Clear();
            }

            jointInsidePosition = Vector3.zero;
            jointInsideDirection = TurnDirection.Straight;

            if (blocks==null)
                blocks = new List<Block>();

            isFinished = false;
            executeStepIndex = 0;
            exitTrigger.center = Vector3.Scale(exitTrigger.center, new Vector3(1, 1, 0));
        }

        public float GetLength() {
            float length = 0;
            foreach (var block in blocks) {
                length += block.length;
            }
            return length;
        }


        /// <summary>
        /// 执行生成Block命令
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
            ExecuteFinishedHandler?.Invoke();            
        }




        private int executeStepIndex;
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

                    blockInstant.transform.localPosition = jointInsidePosition;
                    blockInstant.transform.localRotation = TurnDirectionUtil.ToQuaternion(jointInsideDirection);

                    // 设置下一个生产点的信息
                    /// 下一个点的旋转
                    jointInsideDirection = TurnDirectionUtil.Turn(jointInsideDirection, blockInstant.turnDirection);

                    jointInsidePosition = blockInstant.jointOutsidePosition;


                    if (Random.value < coinRate)
                        blockInstant.SpawnCoin();

                    blockInstant.parentSection = this;

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
            exitTrigger.center = jointInsidePosition;

            switch (jointInsideDirection) {
                case TurnDirection.Straight:
                    exitTrigger.size = new Vector3(10, 10, 2);
                    break;
                case TurnDirection.Right:
                    exitTrigger.size = new Vector3(2,10,10);
                    break;
                case TurnDirection.Back:
                    exitTrigger.size = new Vector3(10,10,2);
                    break;
                case TurnDirection.Left:
                    exitTrigger.size = new Vector3(2, 10, 10);
                    break;
                default:
                    break;
            }
        }

        void OnTriggerExit(Collider other) {
            if(other.tag == "Player") {
                StartCoroutine(SelfDestroy());
                Track.Instance.SpawnNextSection();
            }
        }


        public IEnumerator SelfDestroy() {
            yield return new WaitForSeconds(3);
            foreach (var b in blocks) {
                b.SelfDestroy();
            }
            Destroy(gameObject);
        }

        void OnExecuteFinished() {
            SetBoxTrigger();
        }

    }



}
