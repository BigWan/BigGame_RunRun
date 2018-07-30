using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {
	public class CamearFollow : MonoBehaviour {
		public Transform follow;
		public Vector3 followV;
		public Vector3 spd;
		private void Update() {
			transform.localPosition = Vector3.SmoothDamp(transform.localPosition, follow.localPosition + followV,ref spd,2f,1f);
		}
	}
}