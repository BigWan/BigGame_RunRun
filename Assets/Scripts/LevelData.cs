using System.Xml;
using UnityEngine;
using UnityEditor;
namespace RunRun {
    /// <summary>
    /// 关卡配置文件
    /// </summary>
    [System.Serializable]
    public class LevelData {
        public int levelID;
        public float startSpeed;
        public float speedLevelUp;
        public float coinRate;
        public float magnetRate;

        public float length;

        public RoadSectionData[] sections;
        
    }



}