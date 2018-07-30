using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {

	/// <summary>
	/// 跑道后退功能
	/// 跑酷模型中，人位置保持不变，场景向后移动
	/// </summary>
	public class MoveTrackBack : MonoBehaviour {

		public float cms;
		private void FixedUpdate() {
			cms = SpeedController.Instance.currentVelocity;
			transform.Translate(Vector3.back*
			cms * Time.fixedDeltaTime);
		}
	}
}