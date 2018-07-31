using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {


	/// <summary>
	/// 障碍物
	/// </summary>
	public class Obstacle : MonoBehaviour {
		public float dieDelay = 1.0f;
		public int damage;
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
			transform.localScale = Vector3.one;
			transform.localPosition += Vector3.forward * 20f;
		}


	}
}