using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RunRun {
    
    public enum PlugShape {
        None = 0,
        R = 1,
        C = 2,
        L = 4,
        RC = 3,
        LC = 6,
        LR = 5,
        LCR = 7
    }



    public enum PlugType {
        Grass,
        GrassWithEnd,
        BridgeWidePlug,
        BridgeSmallPlug,
        Gap
    }

    /// <summary>
    /// Plug的基类
    /// </summary>
    public class ExitPlug : MonoBehaviour {

        /// <summary>
        /// 插座的类型（类型之间有兼容关系）
        /// </summary>
        public PlugType plugType;

        [Space(15)]
        /// <summary>
        /// 插座形状，LCR
        /// </summary>
        public PlugShape shape;

        /// <summary>
        /// 判断Plug是否已经产出，避免重复产出
        /// </summary>
        public bool hasSpawned = false;

        /// <summary>
        /// 朝向，直线，左，右
        /// </summary>
        public TurnDirection direction;


        /// <summary>
        /// 点
        /// </summary>
        private Vector3[] positions;


        public Vector3 pos;

        /// <summary>
        /// 计算插口点的相对坐标
        /// </summary>
        private void CalcPositions() {            
            if (direction == TurnDirection.Straight) {
                positions = new Vector3[3] {
                    new Vector3(-1,0,0),  // L
                    new Vector3(0,0,0),   // C
                    new Vector3(1,0,0)    // R
                };
            }else if(direction==TurnDirection.Left) {
                positions = new Vector3[3] {
                    new Vector3(0,0,-1),
                    new Vector3(0,0,0),
                    new Vector3(0,0,1)
                };
            }else if (direction==TurnDirection.Right) {
                positions = new Vector3[3] {
                    new Vector3(0,0,-1),
                    new Vector3(0,0,0),
                    new Vector3(0,0,1),
                };
            } else {
                Debug.LogError("<b>Direction 不能为Back</b>");
            }
        }


        private void OnDrawGizmos() {
            CalcPositions();
            Gizmos.color = Color.green;

            Vector3 center = Vector3.zero;


            if (((int)shape & (int)PlugShape.L) == (int)PlugShape.L) {
                Gizmos.DrawWireSphere(transform.TransformPoint(positions[0]), 0.2f );
            }
            if (((int)shape & (int)PlugShape.R) == (int)PlugShape.R) {
                Gizmos.DrawWireSphere(transform.TransformPoint(positions[2]), 0.2f );
            }
            if (((int)shape & (int)PlugShape.C) == (int)PlugShape.C) {
                Gizmos.DrawWireSphere(transform.TransformPoint(positions[1]), 0.2f );
            }
        }


        ///// <summary>
        ///// 返回改变的旋转值,左转,右转分别改变90°
        ///// </summary>
        ///// <returns></returns>
        //public Quaternion yaw {
        //    get {
        //        if (direction == TurnDirection.Left) {
        //            return Quaternion.Euler(0, -90, 0);
        //        } else if (direction == TurnDirection.Right) {
        //            return Quaternion.Euler(0, 90, 0);
        //        } else {
        //            return Quaternion.identity;
        //        }
        //    }
        //}

       


    
        ///// <summary>
        ///// 生成跑道块
        ///// </summary>
        //public Block SpawnNextBlock(Quaternion rotation,bool forceRefresh = false) {

        //    if(spawned != null) {
        //        if (forceRefresh) {
        //            Destroy(spawned.gameObject);
        //        } else {
        //            return spawned;
        //        }
        //    }
        //    Block next = PlugUtil.Instance.GetRandomAdaptorBlock(plugType);

        //    spawned = GameObject.Instantiate<Block>(next) as Block;

        //    spawned.transform.localPosition = transform.position;

        //    spawned.transform.localRotation = getRotation() * rotation;
            
        //    hasSpawned = true;
        //    //spawned.transform.SetParent(transform.parent);
        //    return spawned;
        //}
    }
}