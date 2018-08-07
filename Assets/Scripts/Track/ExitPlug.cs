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

    /// <summary>
    /// 出口Plug的相对方向()
    /// </summary>
    public enum PlugDirection {
        Straight = 0,   // 前进
        Left = 1,       // 左拐
        Right = 2       // 右拐
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
        /// 生成的下一块
        /// </summary>
        private Block spawned;

        /// <summary>
        /// 判断Plug是否已经产出，避免重复产出
        /// </summary>
        public bool hasSpawned = false;

        /// <summary>
        /// 朝向，直线，左，右
        /// </summary>
        public PlugDirection plugDirection;


        /// <summary>
        /// 点
        /// </summary>
        private Vector3[] positions;


        /// <summary>
        /// 计算插口点的坐标（Local）
        /// </summary>
        private void CalcPositions() {            
            if (plugDirection == PlugDirection.Straight) {
                positions = new Vector3[3] {
                    new Vector3(-1,0,0),  // L
                    new Vector3(0,0,0),   // C
                    new Vector3(1,0,0)    // R
                };
            }else if(plugDirection == PlugDirection.Left) {
                positions = new Vector3[3] {
                    new Vector3(0,0,-1),
                    new Vector3(0,0,0),
                    new Vector3(0,0,1)
                };
            }else if (plugDirection == PlugDirection.Right) {
                positions = new Vector3[3] {
                    new Vector3(0,0,-1),
                    new Vector3(0,0,0),
                    new Vector3(0,0,1),
                };
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



    
        /// <summary>
        /// 生成跑道块
        /// </summary>
        public Block SpawnNextBlock(Quaternion rotation,bool forceRefresh = false) {

            if(spawned != null) {
                if (forceRefresh) {
                    Destroy(spawned.gameObject);
                } else {
                    return spawned;
                }
            }
            Block next = PlugUtil.Instance.GetRandomAdaptorBlock(plugType);

            spawned = GameObject.Instantiate<Block>(next) as Block;

            spawned.transform.localPosition = transform.position;
            if (plugDirection == PlugDirection.Left) {
                spawned.transform.localRotation = (Quaternion.Euler(0, -90, 0) * rotation);
            }
            if (plugDirection == PlugDirection.Right) {
                spawned.transform.localRotation = (Quaternion.Euler(0, +90, 0) * rotation);
            }
            if (plugDirection == PlugDirection.Straight) {
                spawned.transform.localRotation = rotation;
            }
            hasSpawned = true;
            //spawned.transform.SetParent(transform.parent);
            return spawned;
        }
    }
}