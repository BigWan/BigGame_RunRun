using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {

	/// <summary>
	///  跑道的基本构成单位
	/// </summary>
	public class Block : MonoBehaviour {
		
		// Theme theme;
		// Rule  rule;

		/// <summary>
		/// 段落长度
		/// </summary>
		public float length;

        /// <summary>
        /// 连接点的信息
        /// </summary>
		public List<JoinPoint> joinPoints;

        /// <summary>
        /// 在连接点生成块
        /// </summary>
        public void SpawnNextAtJoinPoint() {
            if (joinPoints.Count <= 0) return;
            foreach (var joint in joinPoints) {
                joint.SpawnBlock();
            }
        }






#if UNITY_EDITOR
        [ContextMenu("获取Joint")]
        private void GetJoinPoint() {
            joinPoints.Clear();
            GetComponentsInChildren<JoinPoint>(joinPoints);
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            //Gizmos.DrawIcon(transform.localPosition, "build_zone.png",true);
            Gizmos.DrawWireCube(transform.localPosition, new Vector3(0.3f, 0.3f, 1.1f));
        }
#endif
    }

}
