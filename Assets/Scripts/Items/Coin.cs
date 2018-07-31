using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {

	/// <summary>
	/// 金币
	/// </summary>
	public class Coin : Item {

		/// <summary>
		/// 一个硬币加多少钱
		/// </summary>
		public int golds;

				
		protected new IEnumerator DisappearCoroutine(){
			WaitForSeconds delay = new WaitForSeconds(dieDelay/30);
			for (int i = 0; i < 30; i++) {
				yield return delay;
				transform.localScale = Vector3.one * (30-i)*0.03f;
			}
		}

	}
}