using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace RunRun {

    [CreateAssetMenu(fileName ="New SectionData",menuName = "创建跑道段落")]
    public class RoadSectionData : ScriptableObject {

        public List<SpawnBlockCommand> commands;

    }

    
}
