using UnityEngine;
using System.Collections.Generic;
namespace RunRun {
    
    /// <summary>
    /// 生产BLock的命令
    /// </summary>
    [System.Serializable]
    public class SpawnBlockCommand {

        /// <summary>
        /// 可以进行生产的Block型号
        /// </summary>
        public Block[] blocks;

        /// <summary>
        /// 数量区间
        /// </summary>
        public int min, max;

        public SpawnBlockCommand(Block[] blocks, int min,int max) {
            this.blocks = blocks;
            this.min = min;
            this.max = max;
        }


        public Block GetRandomBlock() {
            if (blocks != null)
                return blocks[Random.Range(0, blocks.Length)];
            else
                return null;
        }

        public int GetRandomCount() {
            if(max > min && min >= 0)
                return Random.Range(min, max);
            else 
                return 0;            
        }


        public override string ToString() {
            return "Command";
            //return  $"{{{string.Join(",", blockNames)}}},{{{range.start.ToString()},{range.end.ToString()}}}";                
        }
    }

}
