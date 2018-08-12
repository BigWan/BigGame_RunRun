#if UNITY_EDITOR

using UnityEngine;

public class UIDebug : MonoBehaviour {

    public bool isDebug;

    Vector3[] fourCorners = new Vector3[4];
    RectTransform[] rts;

    void OnDrawGizmos() {
        if (!isDebug) return;        
        
        rts = GetComponentsInChildren<RectTransform>();

        foreach (var rt in rts) {            
            rt.GetWorldCorners(fourCorners);
            Gizmos.color = Color.blue;
            for (int i = 0; i < 4; i++) {                
                Gizmos.DrawLine(fourCorners[i], fourCorners[(i + 1) % 4]);
            }

        }
    }


}

#endif
