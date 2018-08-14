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

        ///// <summary>
        ///// 该Block是否变道
        ///// </summary>
        //public bool isTurn;

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
        /// 生成出来的块
        /// </summary>
        //private List<Block> spawnedList = new List<Block>();

        /// <summary>
        /// 是否被用过
        /// </summary>
        public bool hasUsed;


        

        ///// <summary>
        ///// 是否完全生成
        ///// </summary>
        //public bool hasFullySpawned {
        //    get {
        //        foreach (var plug in exitPlugs) {
        //            if (!plug.hasSpawned)
        //                return false;
        //        }
        //        return true;
        //    }
        //}



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
        
        public void SelfDestroy() {
            if (hasUsed) {
                StartCoroutine(DestroyCoroutine());
            }

        }

        IEnumerator DestroyCoroutine() {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }

    }

}
