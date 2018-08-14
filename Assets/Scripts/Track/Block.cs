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



        /// 本地点和本地旋转

        public Vector3 localPosition;
        public Quaternion localRotation;


        /// 位置偏移
        public Vector3 offset {
            get {
                return exitPlugs[0].transform.localPosition;
            }
        }
        
        /// <summary>
        /// 偏转角
        /// </summary>
        public Quaternion localYaw {
            get {
                return exitPlugs[0].localYaw;
            }
        }



        /// <summary>
        /// 是否被用过
        /// </summary>
        public bool hasUsed;


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
