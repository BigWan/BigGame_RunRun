using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityChan;


[RequireComponent(typeof(Animator))]

public class ChanController : MonoBehaviour {

	// 从静止到加速的过程
	public float accelerationTime = 1f;

	public float moveSpeed = 2f;


	public float startSpeedMultiple = 1.5f;


	[Header("动画状态机相关参数")]
	// 动画状态机参数封装
	[SerializeField]
	private float _moveSpeedMultiple;
	public float a_moveSpeedMultiple{
		get{return _moveSpeedMultiple;}
		set{
			_moveSpeedMultiple = value;
			animator.SetFloat("moveSpeedMultiple",_moveSpeedMultiple);
		}
	}

	[SerializeField]
	private float _blendMovement;
	public float a_blendMovement{
		get{return _blendMovement;}
		set{
			_blendMovement = value;
			animator.SetFloat("blendMovement",_blendMovement);
		}
	}

	[SerializeField]
	private bool _running;
	public bool a_running{
		get{return _running;}
		set{
			_running = value;
			animator.SetBool("running",_running);
		}
	}

	[SerializeField]
	private bool _salute;
	public bool a_salute{
		get{return _salute;}
		set{
			_salute = value;
			animator.SetBool("salute",_salute);
		}
	}

	[SerializeField]
	private bool _fail;
	public bool a_fail{
		get{return _fail;}
		set{
			_fail = value;
			animator.SetBool("fail",_fail);
		}
	}

	[SerializeField]
	private bool _falling;
	public bool a_falling{
		get{return _falling;}
		set{
			_fail = value;
			animator.SetBool("falling",a_falling);
		}
	}

	[Space(15)]
	public int i;
	// componets
	private Animator animator;
	private FaceManager face;
	private SpringManager spring;
	private RandomWind wind;
	private IKLookAt lookat;


	public void TriggerDamage(){
		animator.SetTrigger("trigDamaged");
	}
	public void TriggerGetDown(){
		animator.SetTrigger("trigGetDown");
	}
	public void TriggerJump(){
		animator.SetTrigger("trigJump");
	}


	// animationstat

	private AnimatorStateInfo bodyStat{
		get{return animator.GetCurrentAnimatorStateInfo(0);}
	}

	private AnimatorStateInfo hurtStat{
		get{
			return animator.GetCurrentAnimatorStateInfo(1);
		}
	}

	private AnimatorStateInfo faceStat{
		get{return animator.GetCurrentAnimatorStateInfo(2);}
	}

	private void Awake() {
		animator = GetComponent<Animator>();
		a_moveSpeedMultiple = startSpeedMultiple;
	}

	private void Update() {
		if(a_running)
			transform.Translate(Vector3.forward*Time.deltaTime*moveSpeed);
		if (Input.GetKeyDown(KeyCode.W)) {
			StartRun();
		}
		if(Input.GetKeyDown(KeyCode.S)){
			a_running = false;
		}
		if (Input.GetKeyDown(KeyCode.J)) {
			SpeedUp();
		}
		if(Input.GetKeyDown(KeyCode.Space)){
			Jump();
		}
	}

	public void StartRun(){
		a_running = true;
		StartCoroutine(AccelerateFoward());
	}

	// 向前加速过程
	public IEnumerator AccelerateFoward(){
		WaitForSeconds delay = new WaitForSeconds(accelerationTime/5f);
		a_blendMovement = 0f;
		a_moveSpeedMultiple = startSpeedMultiple;
		moveSpeed = 0f;
		for (int i = 0; i < 5; i++) {
			yield return delay;
			a_blendMovement +=0.1f;
			moveSpeed +=0.3f;
		}
	}


	public void SpeedUp(){
		a_moveSpeedMultiple += 0.1f;
		moveSpeed +=0.5f;
	}

	public void SpeedDown(){
	}

	public void Jump(){
		// 只有移动的时候才能跳
		if(bodyStat.IsName("Blend_Movement")){
			TriggerJump();
			// animator.SetBool("bJumping",true);
		}
	}



}
