using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {

	/// <summary>
	/// 物体自转
	/// </summary>
	public class SelfRotation : MonoBehaviour {

		/// <summary>
		/// 默认旋转角
		/// </summary>
		public Vector3 defaultRotation;


		/// <summary>
		/// 旋转角速度，区分正负
		/// </summary>
		/// <returns></returns>
		public float roationSpeed;
		// Use this for initialization


		void Start () {
			transform.eulerAngles = defaultRotation;
		}

		// Update is called once per frame
		void Update () {
			
			transform.localEulerAngles +=Vector3.up*roationSpeed*Time.deltaTime;
		}
		private void FixedUpdate() {
		}
	}
}