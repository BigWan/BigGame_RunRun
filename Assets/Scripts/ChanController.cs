using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityChan;


[RequireComponent(typeof(Animator))]

public class ChanController : MonoBehaviour {

	// 从静止到加速的过程
	public float accelerationTime = 1f;

	public float startSpeedMultiple = 1.5f;


	[Header("动画状态机相关参数")]
	// 动画状态机参数封装
	[SerializeField]
	private float _moveSpeedMultiple;
	public float moveSpeedMultiple{
		get{return _moveSpeedMultiple;}
		set{
			_moveSpeedMultiple = value;
			animator.SetFloat("moveSpeedMultiple",_moveSpeedMultiple); 
		}
	}

	[SerializeField]
	private float _blendMovement;
	public float blendMovement{
		get{return _blendMovement;}
		set{
			_blendMovement = value;
			animator.SetFloat("blendMovement",_blendMovement);
		}
	}

	[SerializeField]
	private bool _running;
	public bool running{
		get{return _running;}
		set{
			_running = value;
			animator.SetBool("running",_running);
		}
	}

	[SerializeField]
	private bool _salute;
	public bool salute{
		get{return _salute;}
		set{
			_salute = value;
			animator.SetBool("salute",_salute);
		}
	}

	[SerializeField]
	private bool _fail;
	public bool fail{
		get{return _fail;}
		set{
			_fail = value;
			animator.SetBool("fail",_fail);
		}
	}

	[SerializeField]
	private bool _falling;
	public bool falling{
		get{return _falling;}
		set{
			_fail = value;
			animator.SetBool("falling",falling);
		}
	}

	[Space(15)]
	public int i;
	// componets
	private Animator animator;
	private FaceUpdate face;
	private AutoBlinkforSD blink;
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
		moveSpeedMultiple = startSpeedMultiple;
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.W)) {
			StartRun();
		}
		if(Input.GetKeyDown(KeyCode.S)){
			running = false;
		}
		if (Input.GetKeyDown(KeyCode.J)) {
			SpeedUp();
		}
		if(Input.GetKeyDown(KeyCode.Space)){
			Jump();
		}

	}

	public void StartRun(){		
		running = true;
		StartCoroutine(AccelerateFoward());
		// animator.SetBool("bRunning",true);
	}

	// 向前加速过程
	public IEnumerator AccelerateFoward(){		
		WaitForSeconds delay = new WaitForSeconds(accelerationTime/5f);
		for (int i = 0; i < 5; i++) {
			yield return delay;
			blendMovement +=0.1f;
		}
	}

	// 重力加速过程

	


	public void SpeedUp(){

		moveSpeedMultiple += 0.1f;
		// animator.SetFloat("fMovementSpeed",moveSpeed) ;
	}

	public void Jump(){
		if(bodyStat.IsName("Blend_Movement")){
			TriggerJump();
			// animator.SetBool("bJumping",true);
		}
	}



}
