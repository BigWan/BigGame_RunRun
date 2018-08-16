using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class test2 : MonoBehaviour {


    //public Vector3 e;

    //[Header("Result")]
    //public Quaternion q;


    //[Header("乘法")]
    //public Quaternion q1;
    //public Quaternion q2;

    //public Quaternion rq;
    //public Vector3 re;
    //private void Update() {
    //    q = Quaternion.Euler(e) ;

    //    rq = q1 * q2;
    //    re = rq.eulerAngles;
    //    Debug.Log(-3 % 4);
    //}

    public Vector3 position;
    public Vector3 rotation;



    public Vector3 endPosition;




    private void Update() {

        endPosition =   Quaternion.Euler(rotation) * position;
        //c = RunRun.TurnDirectionUtil.Turn(a,b);


    }

}
