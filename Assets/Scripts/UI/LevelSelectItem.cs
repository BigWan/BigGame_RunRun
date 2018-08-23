using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectItem : MonoBehaviour {

    public string levelName = "DefaultName";

    public int levelIndex;

    public Text text;

    public Button btn;

    private void Awake() {
        btn = GetComponent<Button>();
    }


    public void RegEvent() {
        btn.onClick.AddListener(
            ()=> RunRun.GameManager.Instance.StartLevel(levelIndex-1)
        );
    }


}


