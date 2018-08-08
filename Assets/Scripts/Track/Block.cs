using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {


    /// <summary>
    ///  跑道的基本构成单位
    /// </summary>
    public class Block : MonoBehaviour {

		/// <summary>
		/// 段落长度（3.75的倍数）
		/// </summary>
		public float length;

        /// <summary>
        /// 该Block是否变道
        /// </summary>
        public bool isTurn;

        [Space(30)]
        
        /// <summary>
        /// 入口接口
        /// </summary>
        public EnterPlug enterPlug;

        /// <summary>
        /// 出口接口
        /// </summary>
        public ExitPlug[] exitPlugs;


        /// <summary>
        /// 生成出来的块
        /// </summary>
        public List<Block> spawnedList = new List<Block>();

        /// <summary>
        /// 是否被用过
        /// </summary>
        public bool isUsed;

        /// <summary>
        /// 是否完全生成
        /// </summary>
        public bool hasFullySpawned {
            get {
                foreach (var plug in exitPlugs) {
                    if (!plug.hasSpawned)
                        return false;
                }
                return true;
            }
        }



        ///// <summary>
        ///// 在连接点生成块
        ///// </summary>
        //public List<Block> SpawnNextPlug(Transform parent) {
        //    if (spawnedList.Count > 0) {
        //        Debug.Log("spawnedList 已经有数据了额", transform);
        //        return null;
        //    }
        //    foreach (var plug in exitPlugs) {
        //        Block newb = plug.SpawnNextBlock(transform.localRotation);
        //        newb.transform.SetParent(parent);
        //        spawnedList.Add(newb);
        //    }
        //    return spawnedList;
        //}




#if UNITY_EDITOR


        private void OnDrawGizmos() {
            //Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(transform.TransformPoint(Vector3.zero), 0.2f);
        }
#endif
    }

}
