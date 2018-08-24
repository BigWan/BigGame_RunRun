using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml;

namespace RunRun {
    public class GameManager : UnitySingleton<GameManager> {

        public int levelID;

        public int GetLeveleID() {
            return levelID;
        }


        public LevelData[]  levelDatas;

        public RoadSectionData[] endLessDatas;

        public LevelManager levelManager;


        public float startSpeed {
            get {
                if (levelID >= 0)
                    return levelDatas[levelID].startSpeed;
                else
                    return 5f;
                
            }
        }

        public float coinRate {
            get {
                if (levelID >= 0)
                    return levelDatas[levelID].coinRate;
                else
                    return 0.5f;
            }
        }

        public float magenetRate {
            get {
                if (levelID >= 0)
                    return levelDatas[levelID].magnetRate;
                else
                    return 0.25f;
            }
        }

        public RoadSectionData[] sectionDatas {
            get {
                if (levelID >= 0)
                    return levelDatas[levelID].sections;
                else
                    return endLessDatas;
            }
        }


        public int levelCount {
            get {
                return levelDatas.Length;
            }
        }

        public float levelLength {
            get {
                if (levelID >= 0)
                    return levelDatas[levelID].length;
                else
                    return Mathf.Infinity;
            }
        }


        void Start() {
            DontDestroyOnLoad(this);
        }

        public void StartEndLessGame() {
            this.levelID = -1;

            SceneManager.LoadScene("RunRun");
        }

        public void StartLevel(int levelID) {
            this.levelID = levelID;
            SceneManager.LoadScene("RunRun");
        }


        /// <summary>
        /// 从json读取数据
        /// </summary>
        void LoadLevelData() {

        }

        public void WinGame() {
            levelManager.WinGame();
        }

        public void ExitLevel() {
            SceneManager.LoadScene("Start");
        }


        private void OnGUI() {
            if(GUI.Button(new Rect(30, 30, 100, 30), "abc")) {
                LoadLevelData();
            }
        }


        public void MoveLeft() {
            levelManager.roleSystem.MoveLeft();

        }
        public void MoveRight() {
            levelManager.roleSystem.MoveRight();
        }


    }


}
