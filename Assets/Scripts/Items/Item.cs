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
		public void StartDisappear(){
			StartCoroutine(DisappearCoroutine());
		}
		/// <summary>
		/// 碰撞后消失
		/// </summary>
		protected IEnumerator DisappearCoroutine(){
			
			for (int i = 0; i < 10; i++) {
				transform.localScale = Vector3.one * (10-i)*0.1f;
				yield return  new WaitForSeconds(dieDelay/10f);
			}			
			Destroy(gameObject);
		}



	}
}