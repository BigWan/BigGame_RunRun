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

        private TurnDirection moveDirection = TurnDirection.Straight;

        private bool canJump = true;

        private bool canChangeSide = true;

        // 角色脚底下的block
        private Block standBlock;

        /// <summary>
        /// 人物当前行动射线(根据moveDirection 会锁定角色xyz的某个分量)
        /// straight      :锁x  左x-1    中x    右x+1
        /// right         :锁z  左z+1    中z    右z-1
        /// back          :锁x  左x+1    中x    右x-1
        /// left          :锁z  左z-1    中z    右z+1
        /// </summary>
        public Ray moveRay;

        private int hp;

        [Header("移动的距离")]
        [SerializeField]

        private float moveDistance;

        [Header("金币数量")]
        public int coinCount = 0;

        private TrackSide _side;
        public TrackSide side {
            get {
                return _side;
            }
            set {
                _side = value;
                transform.localPosition = new Vector3((int)side * 1f, transform.localPosition.y, transform.localPosition.z);
            }
        }

        /// <summary>
        /// 变道,左中右
        /// </summary>
        void ChangeSide(TrackSide side) {
            this.side = side;
        }
        

        private ItemCollector collector;


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
		//private FaceManager face;
		//private SpringManager spring;
		//private RandomWind wind;
		//private IKLookAt lookat;

        void Awake() {
            // getComponent
            animator = GetComponent<Animator>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            collector = GetComponentInChildren<ItemCollector>();

            side = TrackSide.Center;
            moveRay = new Ray() {
                origin = Vector3.zero,
                direction = Vector3.forward
            };
            
            hp = 0;

            canTurn = false;
            moveDirection = TurnDirection.Straight;

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


        /// <summary>
        /// 倒地后回到正确位置继续跑
        /// </summary>
        private IEnumerator Recover2Game(PlugShape ps) {

            yield return new WaitForSeconds(2f);

            if (ps == PlugShape.L)
                side = TrackSide.Left;
            if (ps == PlugShape.R)
                side = TrackSide.Right;
            if (ps == PlugShape.C)
                side = TrackSide.Center;

            yield return new WaitForSeconds(2f);
            a_running = true;
            canChangeSide = true;
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
            transform.localPosition =
                transform.localPosition +  (TurnDirectionUtil.ToVector3(moveDirection)* moveDelta);
        }



		private void TurnLeft(){
            if (!canChangeSide) return;
			switch (side) {
				case (TrackSide.Left):
					return;

				case (TrackSide.Right):
					side = TrackSide.Center;
					break;

				case (TrackSide.Center):
					side = TrackSide.Left;
					break;
			}
		}


		private void TurnRight(){
            if (!canChangeSide) return;

            switch (side) {
				case (TrackSide.Left):
					side = TrackSide.Center;
					break;

				case (TrackSide.Right):
					return;

				case (TrackSide.Center):
					side = TrackSide.Right;
					break;
			}
		}



		void TriggerObstacle(Obstacle o){
			Debug.Log("碰到障碍了");
			TriggerDamage();
			hp -= o.damage;
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
        /// 拐弯跑道,设定基准坐标(Section的localposition)和行走方向(section的direction)
        /// </summary>
        public void Turn(TurnDirection dir) {
            moveDirection = TurnDirectionUtil.Turn(moveDirection, dir);
            transform.localRotation = TurnDirectionUtil.ToQuaternion(moveDirection);

            ChangeSide(TrackSide.Center);
            moveRay.origin = standBlock.jointOutsidePosition;
            moveRay.direction = TurnDirectionUtil.ToVector3(moveDirection);

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
        /// 响应速度值的改变
        /// </summary>
        /// <param name="spd">速度值</param>
        private void OnVelocityChange(float spd) {
            Debug.Log("SpeedChange");
            if (spd > 2) {
                a_blendMovement = 1.0f;
                a_moveSpeedMultiple = spd / 8f + 1f;
            } else {
                a_blendMovement = Mathf.Lerp(0, 1f, spd / 2f);
                a_moveSpeedMultiple = 1.5f;
            }
        }

        void OnEatCoin() {
            coinCount++;
            LevelManager.Instance.uiSystem.SetCoin(coinCount);
            Debug.Log("EatCoin");
        }

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
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("_Turn")) {
                canTurn = false;
            }
        }



        private void Update() {
            // if (a_running)
            // transform.Translate (Vector3.forward * Time.deltaTime * SpeedController.Instance.currentVelocity);
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
                if (canTurn) Turn(TurnDirection.Left);
                TurnLeft();
            }
            if (Input.GetKeyDown(KeyCode.D)) {
                if (canTurn) Turn(TurnDirection.Right);
                TurnRight();
            }

        }
        private void FixedUpdate() {
            // if (a_running) {
            // 	transform.Translate (Vector3.forward * Time.fixedDeltaTime * SpeedController.Instance.currentVelocity);
            // }
            if (a_running)
                MoveControl();
            //float th = animator.GetFloat("CV_Jump");
            //sphereCollider.radius = th;
            // transform.localPosition = new Vector3(th,transform.localPosition.y,transform.localPosition.z);
        }

        void OnDrawGizmos() {
            Debug.DrawRay(moveRay.origin+Vector3.up, moveRay.direction, Color.red,1000f);
        }
    }
}

