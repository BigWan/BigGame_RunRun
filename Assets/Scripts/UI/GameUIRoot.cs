using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RunRun {

    public struct UIData {
        public float speed;
        public int score;
        public int coin;
    }


    public class GameUIRoot : MonoBehaviour {

        public Button pauseButton;
        public Text levelText;
        public Text scoreText;
        public Text coinText;
        public RawImage chanFaceImage;

        static UIData data;


        public void Init() {
            data = new UIData {
                speed = 0,
                score = 0,
                coin = 0
            };
            UpdateData();
        }


        public void SetData(UIData data) {
            GameUIRoot.data = data;
            UpdateData();
        }

        /// <summary>
        /// 更新面板数据
        /// </summary>
        void UpdateData() {
            levelText.text = $"Speed:{data.speed:F0}";
            scoreText.text = $"Score:{data.score}";
            coinText.text = $"Coin:{data.coin}";            
        }


        public void SetCoin(int coin) {
            data.coin = coin;
            UpdateData();
        }

        public void SetSpeed(float speed) {
            data.speed = speed;
            UpdateData();
        }

        public void setDistance(float distance) {
            data.score = (int)distance;
            UpdateData();
        }
    }
}
