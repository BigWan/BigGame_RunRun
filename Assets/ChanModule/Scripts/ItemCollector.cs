using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {

    /// <summary>
    /// 道具搜集器
    /// </summary>
    [RequireComponent(typeof(SphereCollider))]
    public class ItemCollector : MonoBehaviour {

        /// <summary>
        /// 是否有磁铁
        /// </summary>
        
        private bool m_hasMagnet;
        public bool hasMagnet {
            get { return m_hasMagnet; }
            set {
                m_hasMagnet = value;
                SetRadius();
            }
        }

        public System.Action EatCoin;
        public System.Action EatMagnet;

        private SphereCollider col;

        public GameObject ps;

        private void Awake() {
            col = GetComponent<SphereCollider>();
           
        }

        private void SetRadius() {
            col.radius = hasMagnet ? 3f : 0.5f;
            if (hasMagnet) {
                ps.SetActive(true);
            } else {
                ps.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other) {
            if(other.tag == "_Item") {
                Item item = other.GetComponent<Item>();
                if (item is Coin) {                    
                    EatCoin?.Invoke();
                }else if(item is Magnet) {
                    EatMagnet?.Invoke();
                }
            }
        }


        private void Update() {
            if (Input.GetKeyDown(KeyCode.K)) {
                hasMagnet = !hasMagnet;
            }
        }

    }
}
