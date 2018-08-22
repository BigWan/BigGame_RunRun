using UnityEngine;

/// <summary>
/// 拐弯方向,相对方向
/// </summary>
public enum TurnDirection {
    Straight = 0,   // 前进
    Right = 1,       // 右拐
    Back = 2,        // 后
    Left = 3       // 左拐
}

/// <summary>
/// 真实的方向,东南西北
/// </summary>
public enum Orientation {
    North = 0,
    East = 1,
    South = 2,
    West= 3
}


[System.Serializable]
public class DirectionUtil {

    private static readonly Vector3[] TowardEulers = new Vector3[] {
        new Vector3(0,0,0),
        new Vector3(0,90,0),
        new Vector3(0,180,0),
        new Vector3(0,-90,0)
    };

    private static readonly Vector3[] TowardVector3s = new Vector3[] {
        new Vector3(0,0,1),
        new Vector3(1,0,0),
        new Vector3(0,0,-1),
        new Vector3(-1,0,0)
    };


    /// <summary>
    /// 返回拐弯后的方向
    /// </summary>
    public static Orientation Turn(Orientation source, TurnDirection turn) {
        return (Orientation)(((int)source + (int)turn) % 4);
    }

    public static TurnDirection TurnAdd(TurnDirection a,TurnDirection b) {
        return (TurnDirection)(((int)a + (int)(b)) % 4);
    }

    /// <summary>
    /// 真实朝向转为欧拉角
    /// </summary>
    /// <returns></returns>
    public static Vector3 TowardToEuler(Orientation source) {
        return TowardEulers[(int)source];
    }

    public static Vector3 TowardToVector3(Orientation source) {
        return TowardVector3s[(int)source];
    }


    public static Vector3 TurnToEulers(TurnDirection turn) {
        return TowardEulers[(int)turn];
    }

    public static Vector3 TurnToVector3(TurnDirection turn) {
        return TowardVector3s[(int)turn];
    }



    /// <summary>
    /// 真实朝向转为四元数旋转
    /// </summary>
    public static Quaternion TowardToQuaternion(Orientation source) {
        return Quaternion.Euler(TowardToEuler(source));
    }

    public static Quaternion TurnToQuaternion(TurnDirection turn) {
        return Quaternion.Euler(TurnToEulers(turn));
    }

}

