using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunRun {
    public class CoinSpawner : MonoBehaviour {

        public AnimationCurve[] coinCurves;

        public float length;

        public int count;


        public Item coinPrefab;

        public void SpawnCoin() {
            if (count <= 1) return;
            float h = 0;
            float step = 1f / count;
            AnimationCurve coinCurve;// = coinCurves[Random.Range(0, coinCurves.Length)];
            if (length > 3) {
                coinCurve = coinCurves[0];
            } else {
                coinCurve = coinCurves[1];
            }
            for (int i = 0; i <= count; i++) {
                h = coinCurve.Evaluate(i * step);
                var coin = Instantiate(coinPrefab);// GameObject.CreatePrimitive(PrimitiveType.Cube);
                coin.transform.SetParent(transform);
                coin.transform.localPosition = new Vector3(0, h, length * step * i);

            }
        }

    }
}
