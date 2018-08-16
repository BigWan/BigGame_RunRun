using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {

    /// <summary>
    /// 在跑道终点生成终点碰撞器
    /// </summary>
    public class EndTriggerSpawner : MonoBehaviour {

        public bool isEnd;
        public TrackEndTrigger endTriggerPerfab;


        private Vector3 position;
        private TurnDirection direction;

        public void SpawnEnd() {
            TrackEndTrigger endTrigger =  Instantiate<TrackEndTrigger>(endTriggerPerfab);
            endTrigger.transform.SetParent(transform);
            endTrigger.transform.localPosition = position;
            // TODO: 同步Ender的旋转为Block的旋转
            endTrigger.transform.localRotation = Quaternion.identity;
            
            endTrigger.gameObject.SetActive(true);

        }

        public void SetEndPositionAndRoation(Vector3 pos,TurnDirection direction) {
            this.position = pos;
            this.direction = direction;
        }

    }
}
