using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RunRun;
using UnityEngine.EventSystems;

public class LevelSelectPanel : MonoBehaviour {

    public int itemCount;

    public LevelSelectItem prefab;

    private List<LevelSelectItem> items;


    private void OnEnable() {
        itemCount = GameManager.Instance.levelCount;
    }

    private void Start() {
        if (items != null) {
            foreach (var item in items) {
                Destroy(item.gameObject);
            }
            items.Clear();
        } else {
            items = new List<LevelSelectItem>();
        }


        for (int i = 1; i <=itemCount; i++) {
            LevelSelectItem item = GameObject.Instantiate(prefab);
            item.levelName = "关卡" + i.ToString();
            item.levelIndex = i;
            item.transform.SetParent(transform);
            item.text.text = item.levelName;
            item.RegEvent();
            //item.GetComponent<Button>().onClick += mainPanel.StartGame;
                
            items.Add(item);
        }
    }

    
}
