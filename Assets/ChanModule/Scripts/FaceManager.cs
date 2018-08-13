using UnityEngine;
using System.Collections;


public class FaceManager : MonoBehaviour {

	public AnimationClip[] animations;

	Animator animator;

    private int count;

    private void Awake() {
		animator = GetComponent<Animator> ();
        count = animations.Length;
    }

    // 随机改变标签
    void RandomChangeFace(){
        int index = Random.Range(0, count);
        AnimationClip animation = animations[index];

        animator.CrossFade(animation.name,0);
	}

	void Update(){

		if (Input.GetMouseButtonDown(0)) {
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

