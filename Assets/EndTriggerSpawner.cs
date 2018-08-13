using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {

    /// <summary>
    /// 在跑道终点生成终点碰撞器
    /// </summary>
    public class EndTriggerSpawner : MonoBehaviour {

        public bool isEnd;
        public EndTrigger endTriggerPerfab;


        private Vector3 position;
        private Quaternion rotation;

        public void SpawnEnd() {
            EndTrigger endTrigger =  Instantiate<EndTrigger>(endTriggerPerfab);
            endTrigger.transform.SetParent(transform);
            endTrigger.transform.localPosition = position;
            endTrigger.transform.localRotation = rotation;
            
            endTrigger.gameObject.SetActive(true);

        }

        public void SetEndPositionAndRoation(Vector3 pos,Quaternion rotation) {
            this.position = pos;
            this.rotation = rotation;
        }

    }
}
