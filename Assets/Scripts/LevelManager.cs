using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RunRun {
    
    /// <summary>
    /// 管卡流程管理
    /// 关卡流程:
    ///  * 加载场景,初始化UI
    ///  * 预生成跑道
    ///  * 加载角色入场
    ///  * 角色落地开始跑
    ///  * 游戏结束
    /// </summary>
    public class LevelManager : UnitySingleton<LevelManager> {

        [Header("关卡ID")]
        public int levelID;

        public Transform roleSpawnPosition;

        



        [Header("Systems")]
        public GameUIRoot uiSystem;
        public Track trackSystem;
        public ChanController roleSystem;


        [Header("Events")]
        public UnityEvent OnPreSpawnEnd;
        public UnityEvent AfterRoleEngting;
        public UnityEvent OnWinGame;



        private void Start() {
            Init();
        }


        void Init() {
            uiSystem.Init();

            trackSystem.PreSpawn();
            RoleComein();
        }
        

        /// <summary>
        /// 角色入场
        /// </summary>
        public void RoleComein() {
            roleSystem.transform.localPosition = roleSpawnPosition.localPosition;
            StartCoroutine(RoleEnting());
        }

        /// <summary>
        /// 开始跑
        /// </summary>
        public void StartRun() {
            roleSystem.StartRun();
        }


        /// <summary>
        /// 角色入场
        /// </summary>
        /// <returns></returns>
        IEnumerator RoleEnting() {

            //chan.transform.localPosition

            roleSystem.GetComponent<Animator>().Play("TopOfJump", 0);

            roleSystem.GetComponent<Animator>().SetBool("onGround", false);

            while (!Mathf.Approximately(roleSystem.transform.localPosition.magnitude,0)) {
                yield return null;
                roleSystem.transform.localPosition = Vector3.MoveTowards(roleSystem.transform.localPosition, Vector3.zero, 0.25f);
            }
            yield return null;
            roleSystem.GetComponent<Animator>().SetBool("onGround", true);
            yield return new WaitForSeconds(0.9f);
            AfterRoleEngting?.Invoke();
        }


        void LoseGame() {
            
        }

        public void WinGame() {
            OnWinGame?.Invoke();
        }



    }
}
