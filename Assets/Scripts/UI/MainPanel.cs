using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RunRun {

    public class MainPanel : UnitySingleton<MainPanel> {

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
            SceneManager.UnloadSceneAsync("Start");
            SceneManager.LoadScene("RunRun");
        }

        public void StartLevel(int levelID) {
            SceneManager.UnloadSceneAsync("Start");
            SceneManager.LoadScene("RunRun");
        }


        private void Start() {
            settingPanel.gameObject.SetActive(false);
            levelSelector.gameObject.SetActive(false);
        }
    }

}

