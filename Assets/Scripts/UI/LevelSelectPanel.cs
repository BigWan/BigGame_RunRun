using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RunRun;
using UnityEngine.EventSystems;

public class LevelSelectPanel : MonoBehaviour {

    public int itemCount = 15;

    public LevelSelectItem prefab;

    private List<LevelSelectItem> items;


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
            item.GetComponent<Button>().onClick.AddListener(MainPanel.Instance.StartGame); //= MainPanel.Instance.StartLevel(1);
            items.Add(item);
        }
    }

    
}
