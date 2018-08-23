using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {
	public class CamearFollow : MonoBehaviour {

        public float faraway = 3;
        public float height = 6;
		public float smooth = 2;

        public float lookDownAngle = 40f;

        private Orientation toward;

		private ChanController follow;

        private Vector3 camTargetPos;


        void OnSpeedChange(float speed) {
            
        }


        private void Start() {
            //SpeedController.Instance.SpeedChangeAction += OnSpeedChange;
            follow = GameObject.FindGameObjectWithTag("Player").GetComponent<ChanController>();
        }




        private void Update() {
            smooth = Mathf.Max( follow.speed / 2.5f,2);
            toward = follow.moveToward;
            Quaternion roatx = Quaternion.Euler(lookDownAngle, 0, 0);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, DirectionUtil.TowardToQuaternion(toward)* roatx, Time.deltaTime * smooth);
            //transform.LookAt(follow.transform, camTargetPos);
            switch (toward) {
                case Orientation.North:
                    camTargetPos = follow.transform.localPosition + Vector3.up*height+Vector3.back*faraway;
                    break;
                case Orientation.East:
                    camTargetPos = follow.transform.localPosition + Vector3.up * height + Vector3.left * faraway;
                    break;
                case Orientation.South:
                    camTargetPos = follow.transform.localPosition + Vector3.up * height + Vector3.forward * faraway;
                    break;
                case Orientation.West:
                    camTargetPos = follow.transform.localPosition + Vector3.up * height + Vector3.right * faraway;
                    break;
                default:
                    break;  
            }


            transform.localPosition = Vector3.Lerp(transform.localPosition, camTargetPos, Time.deltaTime * smooth);

           
		}




	}
}