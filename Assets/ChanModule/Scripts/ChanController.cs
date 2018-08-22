using System.Collections;
using System.Collections.Generic;
using UnityChan;
using UnityEngine;
using UnityEngine.EventSystems;


namespace RunRun {

	public enum RoadSide{
		Left = -1,
		Center = 0,
		Right = 1
	}

	[RequireComponent(typeof (Animator))]
	[RequireComponent(typeof(CapsuleCollider))]
	public class ChanController : MonoBehaviour {


        private bool canTurn = false;
        private bool hasTurned = false;
        private bool canChangeRoad = true;
        private bool canJump = true;


        public Orientation moveToward = Orientation.North;

        // 角色脚底下的block
        private Block standBlock;

        // 人物当前行动的原点(最近一次拐弯的出口点)(根据moveDirection 会锁定角色xyz的某个分量为原点的对应分量)
        public Vector3 origin;

        public int coinCount = 0;

        private RoadSide side;

        private Coroutine RecoverGameCoroutine;


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
            switch (moveToward) {
                case Orientation.North:
                    transform.localPosition = new Vector3(
                        origin.x + (int)side, 
                        transform.localPosition.y, 
                        transform.localPosition.z
                        );
                    break;
                case Orientation.East:
                    transform.localPosition = new Vector3(
                    transform.localPosition.x,
                    transform.localPosition.y,
                    origin.z - (int)side
                    );
                    break;
                case Orientation.South:
                    transform.localPosition = new Vector3(
                        origin.x - (int)side,
                        transform.localPosition.y,
                        transform.localPosition.z
                        );
                    break;
                case Orientation.West:
                    transform.localPosition = new Vector3(
                    transform.localPosition.x,
                    transform.localPosition.y,
                    origin.z + (int)side
                    );
                    break;
                default:
                    break;
            }
        }
        
                


        #region 动画状态机参数
        [Header ("动画状态机相关参数")]
		// 动画状态机参数封装
		[SerializeField]
		private float _moveSpeedMultiple;
		public float a_moveSpeedMultiple {
			get { return _moveSpeedMultiple; }
			set {
				_moveSpeedMultiple = value;
				animator.SetFloat ("moveSpeedMultiple", _moveSpeedMultiple);
			}
		}

		[SerializeField]
		private float _blendMovement;
		public float a_blendMovement {
			get { return _blendMovement; }
			set {
				_blendMovement = value;
				animator.SetFloat ("blendMovement", _blendMovement);
			}
		}

		[SerializeField]
		private bool _running;
		public bool a_running {
			get { return _running; }
			set {
				_running = value;
				animator.SetBool ("running", _running);
			}
		}

		[SerializeField]
		private bool _salute;
		public bool a_salute {
			get { return _salute; }
			set {
				_salute = value;
				animator.SetBool ("salute", _salute);
			}
		}

		[SerializeField]
		private bool _fail;
		public bool a_fail {
			get { return _fail; }
			set {
				_fail = value;
				animator.SetBool ("fail", _fail);
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
            SpeedController.Instance.SpeedChange += OnVelocityChange;
            SpeedController.Instance.OnStop.AddListener(OnStop);
            LevelManager.Instance.OnWinGame.AddListener(WinGame);

            collector.EatCoin += OnEatCoin;
        }



        #region  动画状态机Layer的属性
        private AnimatorStateInfo bodyStat {
			get { return animator.GetCurrentAnimatorStateInfo (0); }
		}

		private AnimatorStateInfo hurtStat {
			get {
				return animator.GetCurrentAnimatorStateInfo (1);
			}
		}

		private AnimatorStateInfo faceStat {
			get { return animator.GetCurrentAnimatorStateInfo (2); }
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
            SpeedController.Instance.Stop();
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

            SpeedController.Instance.SpeedBack();            
        }


        private void TriggerJump() {
            animator.SetTrigger("trigJump");
        }

        void WinGame() {
            canChangeRoad = false;
            animator.SetBool("salute", true);
        }

        // 移动
        void MoveControl() {
            float moveDelta = SpeedController.Instance.currentVelocity * Time.fixedDeltaTime;
            transform.localPosition += DirectionUtil.TowardToVector3(moveToward)* moveDelta;
        }



		private void Change2Left(){
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


		private void Change2Right(){
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



		void TriggerObstacle(Obstacle o){
			Debug.Log("碰到障碍了");
			TriggerDamage();
		}





        /// <summary>
        /// Jumping@loop  Animation Event
        /// </summary>
        public void ChangeJumpFlag(){
			canJump = true;
		}

        public IEnumerator EnterGround(Vector3 target) {
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

        }

        // API

        /// <summary>
        /// 开始跑
        /// </summary>
        public void StartRun() {
            //if (bodyStat.IsName ("Standing@loop") || bodyStat.IsName ("DownToUp") || bodyStat.IsName("Blend_Movement")) {
            a_running = true;
            canChangeRoad = true;
            SpeedController.Instance.StartMotion();
            // StartCoroutine(AccelerateFoward());
            //}
        }


        /// <summary>
        /// 测试加速
        /// </summary>
        public void TestSPeedup() {
            if (a_running) {
                SpeedController.Instance.SpeedUp(5f);
            }
        }


        /// <summary>
        /// 跳
        /// </summary>
        public void Jump() {
            // 只有移动的时候才能跳
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

        }


        // ==============Event Handler============================


        void OnStop() {
            canChangeRoad = false;
            animator.SetBool("running", false);
            //animator.CrossFade("Standing@loop", 0.1f);
        }

        /// <summary>
        /// 速度值的改变
        /// </summary>
        /// <param name="spd">速度值</param>
        private void OnVelocityChange(float spd) {
            if (spd > 2) {
                a_blendMovement = 1.0f;
                a_moveSpeedMultiple = spd / 12f + 1f;
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
            LevelManager.Instance.uiSystem.SetCoin(coinCount);
            Debug.Log("EatCoin");
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
            // 碰到转弯触发器
            if (other.CompareTag("_Turn")) {
                hasTurned = false;
                canTurn = true;
                canChangeRoad = false;
                //standBlock = other.GetComponentInParent<Block>();// .GetComponent<Block>();
                //origin = standBlock.jointWorldPosition;

            }

            // 碰到地面
            if (other.CompareTag("_Ground")) {
                standBlock = other.GetComponentInParent<Block>();// .GetComponent<Block>();
                origin = standBlock.exitWorldPosition;
                Debug.Log(other.GetComponentInParent<Block>().name, other.GetComponentInParent<Block>().transform);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("_Turn")) {
                canTurn = false;
                canChangeRoad = true;
            }
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.W)) {
                StartRun();
            }
            if (Input.GetKeyDown(KeyCode.S)) {
                a_running = false;
                SpeedController.Instance.Stop();
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
        private void FixedUpdate() {

            if (a_running) MoveControl();

        }

        void OnDrawGizmos() {
            Vector3 o = origin+ Vector3.up;
            Vector3 e = origin+ Vector3.up + DirectionUtil.TowardToVector3(moveToward) * 1000f;
            Debug.DrawLine(o, e);
        }
    }
}

