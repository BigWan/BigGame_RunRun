using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RunRun {

    public class PanelMoveButton : MonoBehaviour {

        public Button moveLeft;
        public Button moveRight;
        
        private void Awake() {
            moveLeft.onClick.AddListener(GameManager.Instance.MoveLeft);
            moveRight.onClick.AddListener(GameManager.Instance.MoveRight);
        }

    }
}
