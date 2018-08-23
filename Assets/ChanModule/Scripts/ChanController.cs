using System.Collections;
using System.Collections.Generic;
using UnityChan;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace RunRun {

    public enum RoadSide {
        Left = -1,
        Center = 0,
        Right = 1
    }

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class ChanController : MonoBehaviour {


        private bool canTurn = false;
        private bool hasTurned = false;
        private bool canChangeRoad = true;
        private bool canJump = true;

        public float changeRoadSpeed = 5f;
        public Orientation moveToward = Orientation.North;

        // 角色脚底下的block
        private Block standBlock;

        // 人物当前行动的原点(最近一次拐弯的出口点)(根据moveDirection 会锁定角色xyz的某个分量为原点的对应分量)
        public Vector3 origin;

        public int coinCount = 0;
        public float moveDistance = 0f;

        private RoadSide side;


        public SpeedController speedController;


        private Coroutine RecoverGameCoroutine;




        public UnityAction<int> EatCoinAction;

        public UnityAction EntingFinishAction;


        /// <summary>
        /// 变道, 变道只改变坐标的x或者z分量
        /// moveDirection:
        /// straight      :锁x  左x-1    中x    右x+1
        /// right         :锁z  左z+1    中z    右z-1
        /// back          :锁x  左x+1    中x    右x-1
        /// left          :锁z  左z-1    中z    右z+1
        /// </summary>
        void ChangeRoad(RoadSide side) {
            this.side = side;
        }

        public float speed {
            get { return speedController.currentVelocity; }
        }

        public void StopSmooth (){
            speedController.SpeedTo(0);
            
        }

        public Vector3 GetCorrectPosition(RoadSide roadSide) {
            float x = transform.localPosition.x;
            float y = transform.localPosition.y;
            float z = transform.localPosition.z;
            switch (moveToward) {
                case Orientation.North:
                    return new Vector3(origin.x + (int)roadSide, y, z);
                case Orientation.East:
                    return new Vector3(x, y, origin.z - (int)roadSide);
                case Orientation.South:
                    return new Vector3(origin.x - (int)roadSide, y, z);
                case Orientation.West:
                    return new Vector3(x, y, origin.z + (int)roadSide);
                default:
                    return transform.localPosition;
            }
        }

        public Vector3 GetCorrectPosition() {
            return GetCorrectPosition(this.side);
        }



        #region 动画状态机参数
        [Header("动画状态机相关参数")]
        // 动画状态机参数封装
        [SerializeField]
        private float _moveSpeedMultiple;
        public float a_moveSpeedMultiple {
            get { return _moveSpeedMultiple; }
            set {
                _moveSpeedMultiple = value;
                animator.SetFloat("moveSpeedMultiple", _moveSpeedMultiple);
            }
        }

        [SerializeField]
        private float _blendMovement;
        public float a_blendMovement {
            get { return _blendMovement; }
            set {
                _blendMovement = value;
                animator.SetFloat("blendMovement", _blendMovement);
            }
        }

        [SerializeField]
        private bool _running;
        public bool a_running {
            get { return _running; }
            set {
                _running = value;
                animator.SetBool("running", _running);
            }
        }

        [SerializeField]
        private bool _salute;
        public bool a_salute {
            get { return _salute; }
            set {
                _salute = value;
                animator.SetBool("salute", _salute);
            }
        }

        [SerializeField]
        private bool _fail;
        public bool a_fail {
            get { return _fail; }
            set {
                _fail = value;
                animator.SetBool("fail", _fail);
            }
        }

        #endregion


        // componets
        private Animator animator;
        private CapsuleCollider capsuleCollider;
        private ItemCollector collector;
        //private FaceManager face;
        //private SpringManager spring;
        //private RandomWind wind;
        //private IKLookAt lookat;

        void GetComponents() {
            animator = GetComponent<Animator>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            collector = GetComponentInChildren<ItemCollector>();
        }


        void Awake() {
            // getComponent
            GetComponents();

            side = RoadSide.Center;
            origin = Vector3.zero;
            SetMoveDirection(Orientation.North);

            canJump = true;
            canChangeRoad = true;
            canTurn = false;
            hasTurned = false;

            RegEvent();
        }

        void RegEvent() {
            speedController.SpeedChangeAction += OnVelocityChange;
            collector.EatCoin += OnEatCoin;
            speedController.StopAction +=()=> a_running = false;
        }



        #region  动画状态机Layer的属性
        private AnimatorStateInfo bodyStat {
            get { return animator.GetCurrentAnimatorStateInfo(0); }
        }

        private AnimatorStateInfo hurtStat {
            get {
                return animator.GetCurrentAnimatorStateInfo(1);
            }
        }

        private AnimatorStateInfo faceStat {
            get { return animator.GetCurrentAnimatorStateInfo(2); }
        }
        #endregion


        // API
        private void TriggerDamage() {
            animator.SetTrigger("trigDamaged");
        }



        private void TriggerGetDown(RoadType ps) {
            animator.SetTrigger("trigGetDown");
            a_running = false;
            canChangeRoad = false;

            speedController.Stop();
            if (RecoverGameCoroutine != null) {
                StopCoroutine(RecoverGameCoroutine);
            }
            RecoverGameCoroutine = StartCoroutine(Recover2Game(ps));
        }

        private void SetMoveDirection(Orientation moveDirection) {
            this.moveToward = moveDirection;
            transform.localRotation = DirectionUtil.TowardToQuaternion(this.moveToward);
        }


        /// <summary>
        /// 倒地后回到正确位置继续跑
        /// </summary>
        private IEnumerator Recover2Game(RoadType ps) {

            yield return new WaitForSeconds(4f);

            a_running = true;
            canChangeRoad = true;

            SetMoveDirection(standBlock.getTurnToward());


            ChangeRoad(RoadSide.Center);
            if (ps == RoadType.L) ChangeRoad(RoadSide.Left);
            if (ps == RoadType.R) ChangeRoad(RoadSide.Right);
            if (ps == RoadType.C) ChangeRoad(RoadSide.Center);

            origin = standBlock.GetExitWorldPosition();
            yield return new WaitForSeconds(1f);
            speedController.SpeedBack();
        }


        private void TriggerJump() {
            animator.SetTrigger("trigJump");
        }

        void WinGame() {
            canChangeRoad = false;
            animator.SetBool("salute", true);
        }



        private void Change2Left() {
            if (!canChangeRoad) return;
            switch (side) {
                case (RoadSide.Left):
                    return;

                case (RoadSide.Right):
                    ChangeRoad(RoadSide.Center);
                    break;

                case (RoadSide.Center):
                    ChangeRoad(RoadSide.Left);
                    break;
            }
        }


        private void Change2Right() {
            if (!canChangeRoad) return;

            switch (side) {
                case (RoadSide.Left):
                    ChangeRoad(RoadSide.Center);
                    break;
                case (RoadSide.Right):
                    return;
                case (RoadSide.Center):
                    ChangeRoad(RoadSide.Right);
                    break;
            }
        }



        void TriggerObstacle(Obstacle o) {
            Debug.Log("碰到障碍了");
            TriggerDamage();
        }





        /// <summary>
        /// Jumping@loop  Animation Event
        /// </summary>
        public void ChangeJumpFlag() {
            canJump = true;
        }


        public void StartEngting() {
            StartCoroutine(EnterGround(Vector3.zero ));
        }

        private IEnumerator EnterGround(Vector3 target) {
            //chan.transform.localPosition

            animator.Play("TopOfJump", 0);

            animator.SetBool("onGround", false);
             
            while (!Mathf.Approximately(transform.localPosition.magnitude, 0)) {
                yield return null;
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, 0.25f);
            }
            yield return null;
            animator.SetBool("onGround", true);

            yield return new WaitForSeconds(0.9f);
            EntingFinishAction?.Invoke();
        }

        // API

        /// <summary>
        /// 开始跑
        /// </summary>
        public void StartRun() {
            a_running = true;
            canChangeRoad = true;
            speedController.StartMotion();
        }


        /// <summary>
        /// 测试加速
        /// </summary>
        public void TestSPeedup() {
            if (a_running) {
                speedController.SpeedUp(5f);
            }
        }


        /// <summary>
        /// 跳
        /// </summary>
        public void Jump() {
            if (bodyStat.IsName("Blend_Movement")) {
                if (canJump) {
                    TriggerJump();
                    canJump = false;
                }
                // animator.SetBool("bJumping",true);
            } else if (bodyStat.IsName("Jumping@loop") && bodyStat.normalizedTime >= 0.55f) {
                animator.CrossFade("Blend_Movement", 0.0f, 0, 0);
                if (canJump) {
                    TriggerJump();
                    canJump = false;
                }
            }
        }


        /// <summary>
        /// 拐弯跑道,设定基准坐标(Section的localposition)和行走方向(Block的Direction)
        /// </summary>
        public void TurnPrecess(TurnDirection dir) {
            SetMoveDirection(DirectionUtil.Turn(moveToward, dir));
            transform.localRotation = DirectionUtil.TowardToQuaternion(moveToward);

            ChangeRoad(RoadSide.Center);
            hasTurned = true;
            canTurn = false;
            canChangeRoad = true;
            origin = standBlock.GetExitWorldPosition();
        }


        // ==============Event Handler============================


        public void OnStop() {
            canChangeRoad = false;
            animator.SetBool("running", false);
            //animator.CrossFade("Standing@loop", 0.1f);
        }

        /// <summary>
        /// 速度值的改变
        /// </summary>
        /// <param name="spd">速度值</param>
        public void OnVelocityChange(float spd) {
            if (spd > 2) {
                a_blendMovement = 1.0f;
                a_moveSpeedMultiple = spd / 12f + 7/12f;
            } else {
                a_blendMovement = Mathf.Lerp(0, 1f, spd / 2f);
                a_moveSpeedMultiple = 1.5f;
            }
        }

        /// <summary>
        /// 吃金币处理
        /// </summary>
        void OnEatCoin() {
            coinCount++;
            EatCoinAction?.Invoke(coinCount);             
        }




        /// <summary>
        /// 触发器处理
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other) {
            // 碰到栅栏
            if (other.CompareTag("_Fence")) {
                RoadType roadType = other.GetComponentInParent<Block>().roadType;
                TriggerGetDown(roadType);
            }

            // 碰到地面
            if (other.CompareTag("_Ground")) {
                standBlock = other.GetComponentInParent<Block>();

                // 直路直接设置origin,弯道需要转弯后才设置
                if (standBlock.turnDirection == TurnDirection.Straight) {
                    origin = standBlock.GetExitWorldPosition();
                } else {
                    hasTurned = false;
                    canTurn = true;
                    canChangeRoad = false;
                }
            }
        }


        private void Update() {
            InputControl();
            MoveController();
        }

        void InputControl() {
            if (Input.GetKeyDown(KeyCode.W)) {
                StartRun();
            }
            if (Input.GetKeyDown(KeyCode.S)) {
                a_running = false;
                speedController.Stop();
            }
            if (Input.GetKeyDown(KeyCode.J)) {
                TestSPeedup();
            }
            if (Input.GetKey(KeyCode.Space)) {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.A)) {
                if (canTurn && hasTurned == false)
                    TurnPrecess(TurnDirection.Left);
                else
                    Change2Left();
            }
            if (Input.GetKeyDown(KeyCode.D)) {
                if (canTurn && hasTurned == false)
                    TurnPrecess(TurnDirection.Right);
                else
                    Change2Right();
            }
        }


        private void MoveController() {
            if (a_running) {
                float moveDelta = speedController.currentVelocity * Time.deltaTime;
                moveDistance += moveDelta;
                transform.localPosition += DirectionUtil.TowardToVector3(moveToward) * moveDelta;
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, GetCorrectPosition(), changeRoadSpeed * Time.deltaTime);
            }

        }

        void OnDrawGizmos() {
            Vector3 o = origin + Vector3.up;
            Vector3 e = origin + Vector3.up + DirectionUtil.TowardToVector3(moveToward) * 1000f;
            Debug.DrawLine(o, e);
        }
    }
}

