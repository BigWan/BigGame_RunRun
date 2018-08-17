using System.Collections;
using System.Collections.Generic;
using UnityChan;
using UnityEngine;
using UnityEngine.EventSystems;


namespace RunRun {

	public enum TrackSide{
		Left = -1,
		Center = 0,
		Right = 1
	}

	[RequireComponent(typeof (Animator))]
	[RequireComponent(typeof(CapsuleCollider))]
	public class ChanController : MonoBehaviour {


        private bool canTurn = false;
        private bool canJump = true;
        private bool canChangeSide = true;

        private TurnDirection moveDirection = TurnDirection.Straight;

        // 角色脚底下的block
        private Block standBlock;

        // 人物当前行动的原点(最近一次拐弯的出口点)(根据moveDirection 会锁定角色xyz的某个分量为原点的对应分量)
        public Vector3 origin;

        public int coinCount = 0;

        private TrackSide side;

        /// <summary>
        /// 变道,变道只改变坐标的x或者z分量
        /// moveDirection:
        /// straight      :锁x  左x-1    中x    右x+1
        /// right         :锁z  左z+1    中z    右z-1
        /// back          :锁x  左x+1    中x    右x-1
        /// left          :锁z  左z-1    中z    右z+1
        /// </summary>
        void ChangeSide(TrackSide side) {
            this.side = side;
            switch (moveDirection) {
                case TurnDirection.Straight:
                    transform.localPosition = new Vector3(
                        origin.x + (int)side, 
                        transform.localPosition.y, 
                        transform.localPosition.z
                        );
                    break;
                case TurnDirection.Right:
                    transform.localPosition = new Vector3(
                    transform.localPosition.x,
                    transform.localPosition.y,
                    origin.z - (int)side
                    );
                    break;
                case TurnDirection.Back:
                    transform.localPosition = new Vector3(
                        origin.x - (int)side,
                        transform.localPosition.y,
                        transform.localPosition.z
                        );
                    break;
                case TurnDirection.Left:
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

		[SerializeField]
		private bool _falling;
		public bool a_falling {
			get { return _falling; }
			set {
				_fail = value;
				animator.SetBool ("falling", a_falling);
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

            side = TrackSide.Center;
            origin = Vector3.zero;
            SetMoveDirection(TurnDirection.Straight);

            canJump = true;
            canChangeSide = true;            
            canTurn = false;

            RegEvent();
        }

        void RegEvent() {
            SpeedController.Instance.VelocityChange += OnVelocityChange;
            SpeedController.Instance.OnStop.AddListener(OnStop);
            LevelManager.Instance.OnWinGame.AddListener(WinGame);

            collector.EatCoin += OnEatCoin;
        }



        #region  动画状态机Layer的get
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

        private void TriggerGetDown(PlugShape ps) {
            animator.SetTrigger("trigGetDown");
            a_running = false;
            canChangeSide = false;
            SpeedController.Instance.Stop();
            StartCoroutine(Recover2Game(ps));
        }

        private void SetMoveDirection(TurnDirection moveDirection) {
            this.moveDirection = moveDirection;
            transform.localRotation = TurnDirectionUtil.ToQuaternion(this.moveDirection);
        }

        /// <summary>
        /// 倒地后回到正确位置继续跑
        /// </summary>
        private IEnumerator Recover2Game(PlugShape ps) {

            yield return new WaitForSeconds(2f);

            if (ps == PlugShape.L) ChangeSide(TrackSide.Left);
            if (ps == PlugShape.R) ChangeSide(TrackSide.Right);
            if (ps == PlugShape.C) ChangeSide(TrackSide.Center);

            yield return new WaitForSeconds(2f);
            a_running = true;
            canChangeSide = true;
            SetMoveDirection(TurnDirectionUtil.Turn(standBlock.parentSection.direction, standBlock.turnDirection)); //
            SpeedController.Instance.SpeedBack();            
        }


        private void TriggerJump() {
            animator.SetTrigger("trigJump");
        }

        void WinGame() {
            canChangeSide = false;
            animator.SetBool("salute", true);
        }

        // 移动
        void MoveControl() {
            float moveDelta = SpeedController.Instance.currentVelocity * Time.fixedDeltaTime;
            transform.localPosition += TurnDirectionUtil.ToVector3(moveDirection)* moveDelta;
        }



		private void TurnLeft(){
            if (!canChangeSide) return;
			switch (side) {
				case (TrackSide.Left):
					return;

				case (TrackSide.Right):
                    ChangeSide(TrackSide.Center);
					break;

				case (TrackSide.Center):
                    ChangeSide(TrackSide.Left);
                    break;
			}
		}


		private void TurnRight(){
            if (!canChangeSide) return;

            switch (side) {
				case (TrackSide.Left):
                    ChangeSide(TrackSide.Center);
                    break;
				case (TrackSide.Right):
					return;                    
				case (TrackSide.Center):
                    ChangeSide(TrackSide.Right);
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
            canChangeSide = true;
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
        public void Turn(TurnDirection dir) {
            SetMoveDirection(TurnDirectionUtil.Turn(moveDirection, dir));
            transform.localRotation = TurnDirectionUtil.ToQuaternion(moveDirection);

            ChangeSide(TrackSide.Center);
            

            //moveDirection = TurnDirectionUtil.Turn(moveDirection, standBlock.turnDirection);


            // SetNew StandardPosition
            // ChangeRoleDirection

        }


        // ==============Event Handler============================


        void OnStop() {
            canChangeSide = false;
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
                a_moveSpeedMultiple = spd / 8f + 1f;
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
        /// 碰撞处理
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other) {
            // 碰到栅栏
            if (other.CompareTag("_Fence")) {
                PlugShape ps = other.GetComponentInParent<Block>().enterPlug.shape;
                TriggerGetDown(ps);
            }
            // 碰到转弯触发器
            if (other.CompareTag("_Turn")) {
                canTurn = true;
                standBlock = other.GetComponentInParent<Block>();// .GetComponent<Block>();
                origin = standBlock.jointWorldPosition;
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("_Turn")) {
                canTurn = false;
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
                if (canTurn)
                    Turn(TurnDirection.Left);
                else
                    TurnLeft();
            }
            if (Input.GetKeyDown(KeyCode.D)) {
                if (canTurn)
                    Turn(TurnDirection.Right);
                else
                    TurnRight();
            }

        }
        private void FixedUpdate() {

            if (a_running) MoveControl();

        }

        void OnDrawGizmos() {
            Vector3 o = origin+ Vector3.up;
            Vector3 e = origin+ Vector3.up + TurnDirectionUtil.ToVector3(moveDirection) * 1000f;
            Debug.DrawLine(o, e);
        }
    }
}

