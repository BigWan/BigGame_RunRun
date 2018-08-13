﻿using System.Collections;
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

	/// <summary>
	/// 改血事件
	/// </summary>
	/// <param name="current">当前血量</param>
	/// <param name="delta">血量变化</param>
	public delegate void HpChange(int current,int delta);

	[RequireComponent(typeof (Animator))]
	// [RequireComponent(typeof(ChanSpeedController))]
	[RequireComponent(typeof(CapsuleCollider))]
	public class ChanController : MonoBehaviour {

		public HpChange hpChange;
		private int _hp;
		private int hp{
			get{return _hp;}
			set{
				int delta = value - _hp;
				_hp = value;
                hpChange?.Invoke(_hp, delta);
            }
		}


		private TrackSide _side;
		private TrackSide side{
			get{return _side;}
			set{
				_side = value;
				transform.localPosition = new Vector3((int)side*1f,transform.localPosition.y,transform.localPosition.z);
			}
		}

		public float startSpeedMultiple = 1.5f;

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

		// componets
		private Animator animator;
		private FaceManager face;
		private SpringManager spring;
		private RandomWind wind;
		private IKLookAt lookat;
        private CapsuleCollider capsuleCollider;

        public ItemCollector collector;
		// private ChanSpeedController spdCon;

		public void TriggerDamage () {
			animator.SetTrigger ("trigDamaged");
		}
		public void TriggerGetDown () {
			animator.SetTrigger ("trigGetDown");
			a_running = false;
			SpeedController.Instance.Stop ();
		}
		public void TriggerJump () {
			animator.SetTrigger ("trigJump");
		}

		// animationstat


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



       

		private void Awake () {
			animator = GetComponent<Animator> ();
            capsuleCollider = GetComponent<CapsuleCollider>();

            side = TrackSide.Center;
			a_moveSpeedMultiple = startSpeedMultiple;
			hp = 0;

            RegEventReg();
        }


		private void RegEventReg(){
			SpeedController.Instance.VelocityChange += OnVelocityChange;

            collector.EatCoin += OnEatCoin;
        }



		private void Update () {
			// if (a_running)
				// transform.Translate (Vector3.forward * Time.deltaTime * SpeedController.Instance.currentVelocity);
			if (Input.GetKeyDown (KeyCode.W)) {
				StartRun ();
			}
			if (Input.GetKeyDown (KeyCode.S)) {
				a_running = false;
				SpeedController.Instance.Stop ();
			}
			if (Input.GetKeyDown (KeyCode.J)) {
				SpeedUp ();
			}
			if (Input.GetKey (KeyCode.Space)) {
				Jump ();
			}
			if (Input.GetKeyDown (KeyCode.U)) {
				TriggerGetDown ();
			}
			if(Input.GetKeyDown(KeyCode.A)){
				TurnLeft();
			}
			if(Input.GetKeyDown(KeyCode.D)){
				TurnRight();
			}


 


           

		}


        public Vector3 currentDirection;
        

        // 移动
        void MoveControl() {
            transform.Translate(currentDirection.normalized * SpeedController.Instance.currentVelocity * Time.fixedDeltaTime);
        }



        public float startSpeed;
		public void StartRun () {
			if (bodyStat.IsName ("Standing@loop") || bodyStat.IsName ("DownToUp")) {
				a_running = true;
				SpeedController.Instance.SpeedTo (startSpeed);
				// StartCoroutine(AccelerateFoward());
			}
		}

		private void FixedUpdate () {
            // if (a_running) {
            // 	transform.Translate (Vector3.forward * Time.fixedDeltaTime * SpeedController.Instance.currentVelocity);
            // }
            if(a_running)
                MoveControl();
            //float th = animator.GetFloat("CV_Jump");
            //sphereCollider.radius = th;
            // transform.localPosition = new Vector3(th,transform.localPosition.y,transform.localPosition.z);
        }

		public void SpeedUp () {
			if (a_running) {
				// a_moveSpeedMultiple+=0.1f;
				SpeedController.Instance.SpeedUp (0.5f);
			}
		}


		private bool canJump = true;
		public void Jump () {
			// 只有移动的时候才能跳
			if (bodyStat.IsName ("Blend_Movement") ) {
				if(canJump){
					TriggerJump ();
					canJump = false;
				}
				// animator.SetBool("bJumping",true);
			}else if(bodyStat.IsName("Jumping@loop") && bodyStat.normalizedTime >=0.55f){
				animator.CrossFade("Blend_Movement",0.0f,0,0);
				if(canJump){
					TriggerJump ();
					canJump = false;
				}
			}
		}


		/// <summary>
		/// 变换跑道
		/// </summary>
		public void Turn(){

		}

		/// <summary>
		/// 响应速度值的改变
		/// </summary>
		/// <param name="spd">速度值</param>
		private void OnVelocityChange(float spd){
			Debug.Log("SpeedChange");
			if(spd > 2) {
				a_blendMovement = 1.0f;
				a_moveSpeedMultiple = spd/8f+1f;
			}else{
				a_blendMovement = Mathf.Lerp(0,1f,spd/2f);
				a_moveSpeedMultiple = 1.5f;
			}
		}


		private void TurnLeft(){
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


        public int coinCount = 0;
        void OnEatCoin() {
            coinCount++;
            Debug.Log("EatCoin");
        }


		/// <summary>
		/// Jumping@loop  Animation Event
		/// </summary>
		public void ChangeJumpFlag(){
			canJump = true;
		}

    }
}

