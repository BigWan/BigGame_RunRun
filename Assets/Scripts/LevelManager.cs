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

        public float roleSpawnHeight = 5f;


        public bool isEndLess {
            get {
                return levelID == -1;
            }
        }

        [Header("Systems")]
        public GameUIRoot uiSystem;
        public Track trackSystem;
        public ChanController roleSystem;
        public SpeedController speedController;


        // 预先生成跑道完成
        public UnityAction PreSpawnEndAction;

        // 角色入场
        public UnityAction AfterRoleEntingAction;


        private void RegEvents() {
            roleSystem.speedController.TargetSpeedChangeAction += (speed) => uiSystem.SetSpeed(speed);

            roleSystem.EatCoinAction += (coin) => uiSystem.SetCoin(coin); ;
            roleSystem.EntingFinishAction += () => roleSystem.StartRun();

            PreSpawnEndAction += () => roleSystem.StartEngting();
        }

        
        public void Init() {
            uiSystem = FindObjectOfType<GameUIRoot>();
            trackSystem = FindObjectOfType<Track>();
            roleSystem = FindObjectOfType<ChanController>();


            speedController = FindObjectOfType<SpeedController>();



            levelID = GameManager.Instance.GetLeveleID();
            GameManager.Instance.levelManager = this;

            trackSystem.datas = GameManager.Instance.sectionDatas;
            speedController.startSpeed = GameManager.Instance.startSpeed;
            trackSystem.coinRate = GameManager.Instance.coinRate;
            trackSystem.maxLength = GameManager.Instance.levelLength;

            RegEvents();
        }

        private void Awake() {
            Init();
        }

        private void Start() {
            StartGame();
        }

        public void StartGame() {
            uiSystem.Init();
            trackSystem.PreSpawn();
            roleSystem.transform.localPosition = Vector3.up * roleSpawnHeight;
            PreSpawnEndAction?.Invoke();
        }

        public void StopPlayer() {
            roleSystem.StopSmooth();
        }


        private void Update() {
            uiSystem?.setDistance(roleSystem.moveDistance);
        }
    }
}
