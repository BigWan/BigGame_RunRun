using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelHeader : MonoBehaviour {

    public Button pause;
    public Text level;
    public Text score;
    public Text coin;
    public Image chanFace;


    void Init() {
        level.text = "00";

        score.text = "000";

        coin.text = "0";

    }

    private void Awake() {
        Init();
    }




}
