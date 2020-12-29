using System.Collections.Generic;
using MotionAI.Core.Util;
using SlipAndJump.BoardMovers.Enemies;
using SlipAndJump.Collectables;
using UnityEngine;

namespace SlipAndJump.Board.Spawner {
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SlipAndJump/SpawnPlan", order = 1)]

    public class SpawnPlan : ScriptableObject {
        public List<Enemy> enemies;
        public List<Collectable> collectables;

        public Enemy SampleEnemy() {
            return enemies.RandomElement();
        }

        public Collectable SampleCollectable() {
            return collectables.RandomElement();
        }
    }
}