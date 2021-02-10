using System.Collections.Generic;
using System.Linq;
using IntervalTree;
using SlipAndJump.Board.Platform;
using UnityEngine;

namespace SlipAndJump.Board.Spawner {
    public class PlatformEffectSpawner : MonoBehaviour {

        private IntervalTree<float, PlatformEffects> tree;
        private float _total;
        void Start() {
            tree = new IntervalTree<float, PlatformEffects>();
            LoadTree();

        }

        private void LoadTree() {
            List<PlatformEffects> availableEffects =
                Resources.LoadAll<PlatformEffects>("Prefabs/PlatformEffects").ToList();
            
            availableEffects.OrderBy(x => x.spawnWeight);

            _total = 0;
            foreach (PlatformEffects effect in availableEffects) {
                float next = _total + effect.spawnWeight;
                tree.Add(_total, next, effect);
                _total = next;
            }
        }

        public PlatformEffects GetRandomEffect() {
            float r = Random.Range(0, _total);
            return tree.Query(r).First();
        }
    }
}