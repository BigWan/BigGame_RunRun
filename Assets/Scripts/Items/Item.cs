using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {

	/// <summary>
	/// 玩家搜集的道具，包括金币，磁铁，血量道具等
	/// </summary>
	[RequireComponent(typeof(SphereCollider))]
	public abstract class Item : MonoBehaviour {

		public float dieDelay = 1.0f;

        private Transform target;

		public void StartDisappear(Transform target){
			StartCoroutine(DisappearCoroutine(target));
		}
		/// <summary>
		/// 碰撞后消失
		/// </summary>
		protected IEnumerator DisappearCoroutine(Transform target){
			
			for (int i = 0; i < 15; i++) {
                transform.localScale = transform.localScale * 0.95f;
                yield return null;
			}			
			Destroy(gameObject);
		}



        private void OnTriggerEnter(Collider other) {
            ItemCollector ic = other.GetComponent<ItemCollector>();

            if (ic is ItemCollector) {
                if (ic.hasMagnet) {
                    StartFollow(other.transform);
                }                 
                 StartDisappear(other.transform);
            }
        }

        public void StartFollow(Transform target) {
            this.target = target;
            transform.SetParent(target.parent,true);
            
        }


        private void Update() {
            if (target != null) {
                transform.localPosition = Vector3.Lerp(transform.localPosition, target.localPosition,0.25f);
            }
        }

    }
}