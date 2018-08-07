using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RunRun {

    /// <summary>
    /// 入口形状
    /// </summary>
    public class EnterPlug : MonoBehaviour {

        public PlugType plugType;

        [Space(15)]

        public PlugShape shape;


        private void OnDrawGizmos() {

            Gizmos.color = Color.red;

            Vector3 center = Vector3.zero;

            if (((int)shape & (int)PlugShape.L) == (int)PlugShape.L) {
                Gizmos.DrawWireSphere(transform.TransformPoint(Vector3.left), 0.3f );
            }
            if (((int)shape & (int)PlugShape.R) == (int)PlugShape.R) {
                Gizmos.DrawWireSphere(transform.TransformPoint(Vector3.right), 0.3f );
            }
            if (((int)shape & (int)PlugShape.C) == (int)PlugShape.C) {
                Gizmos.DrawWireSphere(transform.TransformPoint(Vector3.zero), 0.3f );
            }
        }


    }
}