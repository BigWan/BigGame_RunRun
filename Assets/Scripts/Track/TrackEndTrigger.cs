using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {

    public class TrackEndTrigger : MonoBehaviour {

        private void OnTriggerEnter(Collider other) {

            // 如果是玩家进入这个碰撞
            if (other.CompareTag("Player")) {
                // 先减速
                LevelManager.Instance.StopPlayer();
                LevelManager.Instance.WinGame();
            }
        }



    }
}
