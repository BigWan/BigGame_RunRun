using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {

	/// <summary>
	/// 跑道往人的反向运动
	/// </summary>
	public class TrackMotion : MonoBehaviour {


        /// <summary>
        /// 角色的移动速度
        /// </summary>
        public Vector3 roleDirection;

		private void FixedUpdate() {
			transform.Translate(-roleDirection * Time.fixedDeltaTime * SpeedController.Instance.currentVelocity);
		}
	}
}