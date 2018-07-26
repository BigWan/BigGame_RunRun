using UnityEngine;
using System.Collections;


public class FaceManager : MonoBehaviour {

	public AnimationClip[] animations;

	Animator animator;

	void Start (){
		animator = GetComponent<Animator> ();
	}

	// 随机改变标签
	void RandomChangeFace(){
		animator.CrossFade(animations[Random.Range(0,animations.Length)].name,0);
	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.Z)) {
			RandomChangeFace();
		}
	}


	// 动画事件帧回调
	public void OnCallChangeFace (string str){
		int ichecked = 0;
		foreach (var animation in animations) {
			if (str == animation.name) {
				animator.CrossFade (str,0.15f,2);
				break;
			} else if (ichecked <= animations.Length) {
				ichecked++;
			} else {
				animator.CrossFade("face_default@sd_hmd",0);
			}
		}
	}

}

