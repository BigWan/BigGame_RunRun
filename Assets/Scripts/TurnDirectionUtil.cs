using UnityEngine;

/// <summary>
/// 拐弯方向
/// </summary>
public enum TurnDirection {
    Straight = 0,   // 前进
    Right = 1,       // 右拐
    Back = 2,        // 后
    Left = 3       // 左拐
}




[System.Serializable]
public class TurnDirectionUtil {

    private static readonly Vector3[] turnEulers = new Vector3[] {
        new Vector3(0,0,0),
        new Vector3(0,90,0),
        new Vector3(0,180,0),
        new Vector3(0,-90,0)
    };

    private static readonly Vector3[] turnVector3 = new Vector3[] {
        new Vector3(0,0,1),
        new Vector3(1,0,0),
        new Vector3(0,0,-1),
        new Vector3(-1,0,0)
    };


    /// <summary>
    /// 返回拐弯后的方向
    /// </summary>
    public static TurnDirection Turn(TurnDirection source, TurnDirection turn) {
        return (TurnDirection)(((int)source + (int)turn) % 4);
    }

    /// <summary>
    /// 转为欧拉角
    /// </summary>
    /// <returns></returns>
    public static Vector3 ToEuler(TurnDirection source) {
        return turnEulers[(int)source];
    }

    public static Vector3 ToVector3(TurnDirection source) {
        return turnVector3[(int)source];
    }

    /// <summary>
    /// 转为四元数旋转
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static Quaternion ToQuaternion(TurnDirection source) {
        return Quaternion.Euler(ToEuler(source));
    }


}

