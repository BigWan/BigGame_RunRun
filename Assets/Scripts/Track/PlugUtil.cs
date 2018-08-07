using System;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {


    /// <summary>
    /// Block配对管理单例
    /// </summary>
    public class PlugUtil:UnitySingleton<PlugUtil> {

        /// <summary>
        /// 配对关系配置表
        /// </summary>
        public PlugAdaptor config;

        /// <summary>
        /// 所有的block
        /// </summary>
        public List<Block> allBlocks;


        /// <summary>
        /// 筛选Block类型
        /// </summary>
        /// <param name="inType"></param>
        /// <returns></returns>
        public List<PlugType> GetAdaptorBlockType(PlugType inType) {
            if (inType == PlugType.Grass) {
                return config.outGrass;
            } else if (inType == PlugType.GrassWithEnd) {
                return config.outGrassWithEnd;
            } else if (inType == PlugType.BridgeWidePlug) {
                return config.outBridgeWidePlug;
            } else if (inType == PlugType.BridgeSmallPlug) {
                return config.outBridgeSmallPlug;
            } else if (inType==PlugType.Gap) {
                return config.outGap;
            } else {
                throw new IndexOutOfRangeException();
            }
        }
        
        /// <summary>
        /// 获取所有配对的Blocks列表
        /// </summary>
        /// <param name="inType"></param>
        /// <returns></returns>
        public List<Block> GetAdaptorBlocks(PlugType inType) {

            List<PlugType> types = GetAdaptorBlockType(inType);

            List<Block> result = new List<Block>();

            foreach (var block in allBlocks) {
                if (block.enterPlug == null) Debug.Log(block.name);
                if (types.Contains(block.enterPlug.plugType)) {
                    result.Add(block);
                }
            }

            return result;
        }
        

        /// <summary>
        /// 根据输入类型随机选择一个可行的Block
        /// </summary>
        /// <param name="inType"></param>
        /// <returns></returns>
        public Block GetRandomAdaptorBlock(PlugType inType) {
            List<Block> lst = GetAdaptorBlocks(inType);

            if (lst.Count <= 0) throw new Exception();
            return lst[UnityEngine.Random.Range(0, lst.Count)];      
        }

    }
}