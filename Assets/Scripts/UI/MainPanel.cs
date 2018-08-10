using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainPanel : MonoBehaviour {

    public Button btn_Setting;
    public Button btn_StartGame;



    public SettingPanel settingPanel;


    public void ShowSettingPanel() {
        settingPanel.gameObject.SetActive(true);
    }

    public void StartGame() {
        SceneManager.LoadScene("RunRun");
    }


    private void Start() {
        settingPanel.gameObject.SetActive(false);
    }
}

