using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour {

    public Button btn_Close;
    public Button btn_OK;
    public Slider sld_Music;

    // 关闭面板
    public void Hide() {
        gameObject.SetActive(false);
    }

    public void ChangeMusic() {

    }



}
