using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {


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
        /// 所处的段落引用
        /// </summary>
        public RoadSection parentSection;


        /// <summary>
        /// 是否被用过
        /// </summary>
        public bool hasUsed;

        /// <summary>
        /// 块的连接点的位置,在Block内部,不计算Block本身的位置和旋转
        /// </summary>
        private Vector3 jointInsidePosition {
            get {
                return exitPlugs[0].transform.localPosition;
            }
        }

        public Vector3 jointWorldPosition {
            get {
                return exitPlugs[0].transform.position;
            }
        }

        
        /// <summary>
        /// 块的外部坐标,计算了块的当前旋转和坐标得到(坐标系为Section内部)
        /// </summary>
        public Vector3 jointOutsidePosition {
            get { return transform.localRotation * jointInsidePosition + transform.localPosition; } 
        }


        /// <summary>
        /// Block的拐弯方向
        /// TODO: 有两个拐弯方向的Block如何处理
        /// </summary>
        public TurnDirection turnDirection {
            get {
                return exitPlugs[0].direction;
            }
        }

        
        /// <summary>
        /// 在内部生产金币
        /// </summary>
        public void SpawnCoin() {
            CoinSpawner spawner = Instantiate(coinSpawnerPrefab) as CoinSpawner;
            spawner.transform.SetParent(transform);

            List<Vector3> ablePoses = new List<Vector3>();

            if (((int)enterPlug.shape & (int)PlugShape.C) == (int)PlugShape.C) {
                ablePoses.Add( new Vector3(0, 0, 0.5f));
            }
            if(((int)enterPlug.shape & (int)PlugShape.L) == (int)PlugShape.L) {
                ablePoses.Add (new Vector3(-1,0,0.5f));
            }
            if(((int)enterPlug.shape & (int)PlugShape.R) == (int)PlugShape.R) {
                ablePoses.Add(new Vector3(1, 0, 0.5f));
            }
            spawner.transform.localPosition = ablePoses[Random.Range(0, ablePoses.Count)];

            if (length > 5) {
                spawner.length = 4;
                spawner.count = 4;
            } else {
                spawner.length = 2;
                spawner.count = 2;
            }

            spawner.SpawnCoin();
        }
        

        /// <summary>
        /// 自销毁
        /// </summary>
        public void SelfDestroy() {
            if (hasUsed) {
                StartCoroutine(DestroyCoroutine());
            }

        }

        IEnumerator DestroyCoroutine() {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }


        private void OnTriggerExit(Collider other) {
            Debug.Log(other.name);

        }


    }

}
