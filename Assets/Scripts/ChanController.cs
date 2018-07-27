using System.Collections;
using System.Collections.Generic;
using UnityChan;
using UnityEngine;

namespace RunRun {

	[RequireComponent (typeof (Animator))]

	public class ChanController : MonoBehaviour {

		// 从静止到加速的时间
		public float accelerationTime = 1f;

		// public float moveSpeed = 2f;
		public float currentSpeed;
		// 加速度值
		public float accelateSpeed = 10f;

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

		[Space (15)]
		public int i;
		// componets
		private Animator animator;
		private FaceManager face;
		private SpringManager spring;
		private RandomWind wind;
		private IKLookAt lookat;
		private ChanSpeedController spdCon;

		public void TriggerDamage () {
			animator.SetTrigger ("trigDamaged");
		}
		public void TriggerGetDown () {
			animator.SetTrigger ("trigGetDown");
			a_running = false;
			spdCon.Stop ();
		}
		public void TriggerJump () {
			animator.SetTrigger ("trigJump");
		}

		// animationstat

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

		private void Awake () {
			animator = GetComponent<Animator> ();
			spdCon = GetComponent<ChanSpeedController> ();
			a_moveSpeedMultiple = startSpeedMultiple;
		}

		private void Update () {
			if (a_running)
				transform.Translate (Vector3.forward * Time.deltaTime * spdCon.currentVelocity);
			if (Input.GetKeyDown (KeyCode.W)) {
				StartRun ();
			}
			if (Input.GetKeyDown (KeyCode.S)) {
				a_running = false;
				spdCon.Stop ();
			}
			if (Input.GetKeyDown (KeyCode.J)) {
				SpeedUp ();
			}
			if (Input.GetKeyDown (KeyCode.Space)) {
				Jump ();
			}
			if (Input.GetKeyDown (KeyCode.U)) {
				TriggerGetDown ();
			}
		}

		public void StartRun () {
			if (bodyStat.IsName ("Standing@loop") || bodyStat.IsName ("DownToUp")) {
				a_running = true;
				// StartCoroutine(AccelerateFoward());
			}
		}

		private void FixedUpdate () {
			if (a_running) {
				currentSpeed += accelateSpeed * Time.fixedDeltaTime;
				currentSpeed = Mathf.Clamp (currentSpeed, 0f, spdCon.currentVelocity);
				transform.Translate (Vector3.forward * Time.fixedDeltaTime * currentSpeed);
			}
		}

		public void SpeedUp () {
			if (a_running) {
				a_moveSpeedMultiple += 0.1f;
				spdCon.SpeedUp (0.5f);
			}
		}

		public void SpeedDown () { }

		public void Jump () {
			// 只有移动的时候才能跳
			if (bodyStat.IsName ("Blend_Movement")) {
				TriggerJump ();
				// animator.SetBool("bJumping",true);
			}
		}

	}

}