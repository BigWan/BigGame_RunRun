using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RunRun {
    public class PanelScoredcard : MonoBehaviour {

        public Button btnEndGame;

        private void Awake() {
            btnEndGame.onClick.AddListener(GameManager.Instance.ExitLevel);
        }

    }
}
