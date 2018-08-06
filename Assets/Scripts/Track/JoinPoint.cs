using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {

	/// <summary>
	/// 连接点
	/// </summary>
	[System.Serializable]
	public class JoinPoint :MonoBehaviour {

		/// <summary>
		/// 接壤方向
		/// </summary>
		public enum JoinDirection {
			Straight  = 0,  // 直线
			Left = 1,       // 左拐
			Right = 2       // 右拐
		}

		/// <summary>
		/// 拼接的方向
		/// </summary>
		public JoinDirection joinDirection;

		/// <summary>
		/// 连接点可以拼接上的Block索引
		/// </summary>
		public Block[] nextValidBlocks;

        public Block b;

        public void SpawnBlock() {
            if (b != null) Destroy(b.gameObject);
            b = Instantiate<Block>(nextValidBlocks[Random.Range(0, nextValidBlocks.Length)]) as Block;
            //b.transform.SetParent(transform);
            b.transform.position = transform.position;
        }

#if UNITY_EDITOR 
        private void OnDrawGizmos() {
            Gizmos.color = Color.green;
            //Gizmos.DrawIcon(transform.localPosition, "build_zone.png",true);
            Vector3 gizmosSize = Vector3.one*0.2f;
            if (joinDirection == JoinDirection.Straight)
                gizmosSize += new Vector3(0, 0, 0.8f);
            else
                gizmosSize += new Vector3(0.8f, 0, 0);
            Gizmos.DrawWireCube(transform.TransformPoint(Vector3.zero), gizmosSize);
        }
#endif


    }
}