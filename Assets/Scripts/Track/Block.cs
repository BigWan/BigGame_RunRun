using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {

    /// <summary>
    /// 道路的类型,左中右三道,跑道出口入口是一个类型
    /// </summary>
    public enum RoadType {
        None = 0,
        R = 1,
        C = 2,
        L = 4,
        RC = 3,
        LC = 6,
        LR = 5,
        LCR = 7
    }

    /// <summary>
    ///  跑道的基本构成单位
    /// </summary>
    public class Block : MonoBehaviour {

        // Inspector 配置

		/// <summary>
		/// 段落长度（3.75的倍数）
		/// </summary>
		public float length;

        public CoinSpawner coinSpawnerPrefab;

        /// <summary>
        /// 跑道的类型
        /// </summary>
        public RoadType roadType;

        /// <summary>
        /// 转弯的相对方向
        /// </summary>
        public TurnDirection turnDirection;

        /// <summary>
        /// 出口坐标
        /// </summary>
        public Vector3 exitPositions;



        [Space(30)]


        /// <summary>
        /// 本体的真实朝向
        /// </summary>
        public Orientation orientation;



        /// <summary>
        /// 所处的段落引用
        /// </summary>
        public RoadSection parentSection;


        public Vector3 GetExitWorldPosition() {
            return transform.TransformPoint(exitPositions);
        }




        public Orientation getTurnToward() {
            return DirectionUtil.Turn(orientation, turnDirection);
        }


        /// <summary>
        /// 块的外部坐标,计算了块的当前旋转和坐标得到(坐标系为Section内部)
        /// </summary>
        public Vector3 exitOutsidePosition {
            get { return transform.localRotation * exitPositions + transform.localPosition; } 
        }

        
        /// <summary>
        /// 在内部生产金币
        /// </summary>
        public void SpawnCoin() {
            CoinSpawner spawner = Instantiate(coinSpawnerPrefab) as CoinSpawner;
            spawner.transform.SetParent(transform,false);

            List<Vector3> ablePoses = new List<Vector3>();

            if (((int)roadType & (int)RoadType.C) == (int)RoadType.C) {
                ablePoses.Add( new Vector3(0, 0, 0.5f));
            }
            if(((int)roadType & (int)RoadType.L) == (int)RoadType.L) {
                ablePoses.Add (new Vector3(-1,0,0.5f));
            }
            if(((int)roadType & (int)RoadType.R) == (int)RoadType.R) {
                ablePoses.Add(new Vector3(1, 0, 0.5f));
            }
            spawner.transform.localPosition = ablePoses[Random.Range(0, ablePoses.Count)];
            spawner.length = length - 1f;
            spawner.SpawnCoin();
        }
        

        /// <summary>
        /// 自销毁
        /// </summary>
        public void SelfDestroy() {
            StartCoroutine(DestroyCoroutine());

            IEnumerator DestroyCoroutine() {
                yield return new WaitForSeconds(3);
                Destroy(gameObject);
            }

        }


         

        private void OnDrawGizmos() {
            // 入口
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.TransformPoint(Vector3.left), 0.3f);
            Gizmos.DrawWireSphere(transform.TransformPoint(Vector3.right), 0.3f);
            Gizmos.DrawWireSphere(transform.TransformPoint(Vector3.zero), 0.3f);

            // 出口
            Gizmos.color = Color.red;
            if (turnDirection == TurnDirection.Left|| turnDirection == TurnDirection.Right) {
                Gizmos.DrawWireSphere(transform.TransformPoint(exitPositions+ Vector3.forward) , 0.3f);
                Gizmos.DrawWireSphere(transform.TransformPoint(exitPositions), 0.3f);
                Gizmos.DrawWireSphere(transform.TransformPoint(exitPositions+ Vector3.back), 0.3f);
            } else {
                Gizmos.DrawWireSphere(transform.TransformPoint(Vector3.left + Vector3.forward * length), 0.3f);
                Gizmos.DrawWireSphere(transform.TransformPoint(Vector3.right + Vector3.forward * length) , 0.3f);
                Gizmos.DrawWireSphere(transform.TransformPoint(Vector3.zero + Vector3.forward * length) , 0.3f);
            }

        }

    }

}
