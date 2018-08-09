using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RunRun {

    /// <summary>
    /// 跑道
    /// </summary>
    public class Track : UnitySingleton<Track> {

        /// <summary>
        /// 开始和结束的固定块
        /// </summary>
        public RoadSectionData 
            startData,
            endData;

        /// <summary>
        /// 当前track的section配置表集合
        /// </summary>
        public RoadSectionData[] datas;

        public RoadSection sectionPrefab;

        /// <summary>
        /// 单个跑道重复出现的次数硬限制
        /// </summary>
        public int maxSectionRepeatCount;

        /// <summary>
        /// 跑道长度限制
        /// </summary>
        public float maxLength;

        /// <summary>
        /// 跑道长度
        /// </summary>
        public float currentLength;

        /// <summary>
        /// 出现金币的概率(每个Block出现金币的概率)
        /// </summary>
        public float coinRate = 0.5f;

        private int[] roadSectionCounts;

        /// <summary>
        /// 段落
        /// </summary>
        private List<RoadSection> sections;


        private Vector3 currentPosition;
        private Quaternion currentRotation;

        private void Awake() {
            Init();
        }


        public void Init() {
            // 计数器归零
            roadSectionCounts = new int[datas.Length];

            // 重置引用列表
            if(sections == null)
                sections = new List<RoadSection>();
            if (sections.Count > 0) {
                foreach (var section in sections) {
                    DestroyImmediate(section.gameObject);
                }
                sections.Clear();
            }
            currentLength = 0;
        }



        void SpawnOneSection(RoadSectionData data,float coinRate = 0) {
            RoadSection section = Instantiate<RoadSection>(sectionPrefab) as RoadSection;
            section.SetData(data);
            section.transform.SetParent(transform);
            section.transform.localPosition = currentPosition;
            section.transform.localRotation = currentRotation;
            section.Execute(coinRate);
            currentPosition = section.transform.localPosition + section.getEndPosition();
            currentRotation = section.transform.localRotation * section.getEndRoation();

            currentLength += section.getLength();
        }



        private bool finished;
        /// <summary>
        /// 一次只循环一个
        /// </summary>
        
        public void SpawnNextSection() {
            if (finished) return;
            if (Mathf.Approximately(currentLength, 0)) {
                SpawnOneSection(startData, 0);
                return;
            }

            if (currentLength < maxLength - 30) {
                int rndIndex = Random.Range(0, datas.Length);
                RoadSectionData data = datas[rndIndex];
                SpawnOneSection(data, coinRate);
                return;
            } else {
                finished = true;
                SpawnOneSection(endData, 0);
            }
        }


        public void SpawnAllSections(float coinRate = 0) {

            SpawnOneSection(startData,0);
            while (currentLength < maxLength-20) {
                int rndIndex = Random.Range(0, datas.Length);   
                RoadSectionData data = datas[rndIndex];
                SpawnOneSection(data,coinRate);
            }

            SpawnOneSection(endData, 0);           

        }

        private void OnGUI() {
            if(GUI.Button(new Rect(30, 30, 100, 30), "Spawn")) {
                //SpawnAllSections(coinRate);
                SpawnNextSection();
            }
        }


    }
}
