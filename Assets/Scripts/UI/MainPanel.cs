using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RunRun {

    public class MainPanel : MonoBehaviour {

        public Button btn_Setting;
        public Button btn_StartGame;
        public Button btn_LeveleSelect;

        public SettingPanel settingPanel;

        public LevelSelectPanel levelSelector;


        public void ShowSettingPanel() {
            settingPanel.gameObject.SetActive(true);
        }

        public void ShowSelectLevel() {
            levelSelector.gameObject.SetActive(true);
        }


        public void StartGame() {
            GameManager.Instance.StartEndLessGame();
        }

        public void StartLevel(int levelID) {
            GameManager.Instance.StartLevel(levelID);
        }


        private void Start() {
            settingPanel.gameObject.SetActive(false);
            levelSelector.gameObject.SetActive(false);
        }
    }

}

