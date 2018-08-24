using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {
    public class CoinSpawner : MonoBehaviour {

        public float length;

        public int count;

        public Item coinPrefab;

        public List<Item> coins;

        public void SpawnCoin() {
            coins = new List<Item>();
            for (float i = 0; i < length; i=i+1f) {
                var coin = Instantiate(coinPrefab);// GameObject.CreatePrimitive(PrimitiveType.Cube);
                coin.transform.SetParent(transform);
                coin.transform.localPosition = new Vector3(0, 0.8f, i);
                coins.Add(coin);
            }
        }

    }
}
