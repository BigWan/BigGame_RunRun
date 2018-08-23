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
        private Vector3 currentEndPosition;

        /// <summary>
        /// 当前朝向
        /// </summary>
        [SerializeField]
        private Orientation currentOrientation;

        private void Awake() {
            Init();
        }


        public void PreSpawn() {
            // 提前生成若干块
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
            currentEndPosition = Vector3.zero;
            currentOrientation = Orientation.North;
        }
        

        void SpawnOneSection(RoadSectionData data,float coinRate = 0,bool isEnd = false) {
            RoadSection section = Instantiate<RoadSection>(sectionPrefab) as RoadSection;
            section.transform.SetParent(transform);

            section.SetData(data,currentEndPosition, currentOrientation);

            // 生成Block
            section.Execute(coinRate);

            currentEndPosition = section.lastSectionTruePosition;

            currentOrientation = section.exitToward;

            // TODO: 生成终点碰撞器
            Vector3 endpos = (maxLength - currentLength) * new Vector3(0, 0, 1);

            if (isEnd) {
                section.SpawnEnd(endpos.z);
            }

            //currentEndLocalRotation = section.localRotation * section.localYaw;


            currentLength += section.getLength();
        }



        private bool finished;

        int sectionIndex;
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
                sectionIndex = Random.Range(0, datas.Length);
                RoadSectionData data = datas[sectionIndex];
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


    }
}
