using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {


    /// <summary>
    /// 工具类，配置Plug的对应关系
    /// </summary>
    [CreateAssetMenu(fileName = "Plug",menuName ="创造plug配置")]
    public  class PlugAdaptor:ScriptableObject {

        [Header("Grass")]
        public List<PlugType> outGrass;

        [Header("GrassWithEnd")]
        public List<PlugType> outGrassWithEnd;

        [Header("BridgeWidePlug")]
        public List<PlugType> outBridgeWidePlug;

        [Header("BridgeSmallPlug")]
        public List<PlugType> outBridgeSmallPlug;


        [Header("Gap")]
        public List<PlugType> outGap;
    }

}