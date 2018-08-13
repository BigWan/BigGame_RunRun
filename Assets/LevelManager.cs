using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RunRun {
    
    /// <summary>
    /// 管卡流程管理
    /// 关卡流程:
    ///  * 加载场景
    ///  * 预生成跑道
    ///  * 加载角色入场
    ///  * 角色落地开始跑
    ///  * 游戏结束
    /// </summary>
    public class LevelManager : MonoBehaviour {
        /// <summary>
        /// 关卡编号
        /// </summary>
        public int levelID;

        public Transform roleSpawnPosition;


        public Track thisTrack;
        public ChanController chan;



        [Space(020)]
        public UnityEvent OnPreSpawnEnd;
        public UnityEvent AfterRoleEngting;



        private void Awake() {
            PreSpawnSection();
        }




        public void PreSpawnSection() {
            thisTrack.PreSpawn();

            OnPreSpawnEnd?.Invoke();
        }


        public void RoleComein() {
            chan.transform.localPosition = roleSpawnPosition.localPosition;
            StartCoroutine(RoleEnting());
        }


        public void StartRun() {
            chan.StartRun();
        }


        /// <summary>
        /// 角色入场
        /// </summary>
        /// <returns></returns>
        IEnumerator RoleEnting() {

            //chan.transform.localPosition


            while (!Mathf.Approximately(chan.transform.localPosition.magnitude,0)) {
                yield return null;

                chan.transform.localPosition = Vector3.MoveTowards(chan.transform.localPosition, Vector3.zero, 0.25f);
            }

            yield return new WaitForSeconds(0.5f);

            AfterRoleEngting?.Invoke();
        }





    }
}
