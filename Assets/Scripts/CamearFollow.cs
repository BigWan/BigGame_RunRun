using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {
	public class CamearFollow : MonoBehaviour {
		public Transform follow;
		public Vector3 followV;
		public float smooth;
		private void Update() {

			transform.localPosition = Vector3.Lerp(transform.localPosition,follow.localPosition + followV,Time.deltaTime*smooth);
		}


	}
}