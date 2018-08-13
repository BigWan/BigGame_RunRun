using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {

    public class EndTrigger : MonoBehaviour {

        // Use this for initialization
        private void OnTriggerEnter(Collider other) {

            // 如果是玩家进入这个碰撞
            if (other.CompareTag("Player")) {
                SpeedController.Instance.SpeedTo(0);
            }
        }
    }
}
