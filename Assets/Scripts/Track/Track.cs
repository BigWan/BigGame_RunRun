using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace RunRun {

    /// <summary>
    /// 跑道
    /// </summary>
    public class Track : UnitySingleton<Track> {

        [Header("Section Data")]
        /// <summary>
        /// 当前track的section配置表集合
        /// </summary>
        public RoadSectionData[] datas;

        /// <summary>
        /// 开始和结束的固定块
        /// </summary>
        public RoadSectionData 
            startData,
            endData;


        [Header("Prefab Ref")]
        public RoadSection sectionPrefab;

        [Header("Config")]
        /// <summary>
        /// 跑道长度限制
        /// </summary>
        public float maxLength;

        /// <summary>
        /// 出现金币的概率(每个Block出现金币的概率)
        /// </summary>
        public float coinRate = 0.5f;

        [Header("Info")]
        /// <summary>
        /// 跑道长度
        /// </summary>
        public float currentLength;


        /// <summary>
        /// 段落
        /// </summary>
        private List<RoadSection> sections;

        /// <summary>
        /// 当前结束点的位置
        /// </summary>
        [SerializeField]        
        private Vector3 jointInsidePosition;

        /// <summary>
        /// 当前结束点的在Track内部的朝向
        /// </summary>
        [SerializeField]
        private TurnDirection jointInsideDirection;

        private void Awake() {
            Init();
        }

        //private void Start() {
        //    PreSpawn();
        //}


        public void PreSpawn() {
            Debug.Log("Pre");
            // 提前生成若干格块
            int preSpawnCount = 2;
            for (int i = 0; i < preSpawnCount; i++) {
                SpawnNextSection();
            }
        }

        public void Init() {
            // 重置引用列表
            if(sections == null)
                sections = new List<RoadSection>();
            if (sections.Count > 0) {
                foreach (var section in sections) {
                    Destroy(section.gameObject);
                }
                sections.Clear();
            }
            currentLength = 0;
            jointInsidePosition = Vector3.zero;
            jointInsideDirection = TurnDirection.Straight;
        }
        

        void SpawnOneSection(RoadSectionData data,float coinRate = 0,bool isEnd = false) {
            RoadSection section = Instantiate<RoadSection>(sectionPrefab) as RoadSection;
            section.transform.SetParent(transform);

            section.SetData(data,jointInsidePosition, jointInsideDirection);

            // 生成Block
            section.Execute(coinRate);

            jointInsidePosition = section.jointOutsidePosition;

            jointInsideDirection = section.jointOutsideDirection;

            // TODO: 生成终点碰撞器
            //Vector3 endpos = (maxLength - currentLength) * new Vector3(0, 0, 1);

            //if (isEnd) {
            //    section.SpawnEnd(endpos.z);
            //}

            //currentEndLocalRotation = section.localRotation * section.localYaw;


            currentLength += section.GetLength();
        }



        private bool finished;

        /// <summary>
        /// 单步生成
        /// </summary>        
        public void SpawnNextSection() {
            if (finished) return;
            if (Mathf.Approximately(currentLength, 0)) {
                SpawnOneSection(startData, 0);
                return;
            }

            if (currentLength < maxLength - 10) {
                int rndIndex = Random.Range(0, datas.Length);
                RoadSectionData data = datas[rndIndex];
                SpawnOneSection(data, coinRate);
                return;
            } else {
                finished = true;
                SpawnOneSection(endData, 0,true);
            }
        }

        /// <summary>
        /// 生成所有
        /// </summary>
        /// <param name="coinRate"></param>
        public void SpawnAllSections(float coinRate = 0) {

            SpawnOneSection(startData,0);
            while (currentLength < maxLength-20) {
                int rndIndex = Random.Range(0, datas.Length);   
                RoadSectionData data = datas[rndIndex];
                SpawnOneSection(data,coinRate);
            }

            SpawnOneSection(endData, 0);           
        }

        //#if UNITY_EDITOR

        //private void OnGUI() {
        //    if(GUI.Button(new Rect(30, 30, 100, 30), "Spawn")) {
        //        //SpawnAllSections(coinRate);
        //        SpawnNextSection();
        //    }
        //}

        //#endif

    }
}
