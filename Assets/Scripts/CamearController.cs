using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {
	public class CamearController : MonoBehaviour {

		// 高度
		public float height = 1.5f;

		// 摄像机距离跟随点的距离
		public float mindis = 3f;
		public float maxdis = 4f;

		public float currentDis = 0f;
		// 人的移动速度
		public float roleSpeed = 5f;

		// 摄像机恢复的(加)速度(角度每秒)
		public float angleReviveAcc = 500f;

		// 摄像机移动的加速度
		public float disAcc = 5f;

		public Transform lookat;
		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {
			roleSpeed = lookat.GetComponent<ChanSpeedController> ().currentVelocity;
			CalcCameraDistance ();
			transform.localPosition = new Vector3 (
				lookat.localPosition.x,
				height,
				lookat.transform.localPosition.z - currentDis
			);
		}

		// 计算摄像机离观察点的距离
		void CalcCameraDistance () {
			float a = roleSpeed * 0.5f + 1f;
			currentDis = currentDis + (a - 1.5f) * Time.deltaTime;
			// currentDis = Mathf.Clamp(currentDis,mindis,maxdis);

		}
	}
}