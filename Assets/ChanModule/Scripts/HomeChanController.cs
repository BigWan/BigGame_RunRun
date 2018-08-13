using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



namespace RunRun {


    /// <summary>
    /// 家园的角色控制器
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class HomeChanController : MonoBehaviour {



        public AnimationClip[] idClips;


        private Animator animator;

        private void Awake() {
            animator = GetComponent<Animator>();

            Debug.Assert(animator != null, "没有找到Animator组件");
        }

        public void RandomIdle() {
            if (Random.value > 0.5f) {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Standing@loop")) {
                    int index = Random.Range(0, idClips.Length);
                    animator.CrossFade(idClips[index].name, 0.15f);
                }
            }
        }


        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                if (!EventSystem.current.IsPointerOverGameObject())
                    RandomIdle();
            }
        }
    }
}
