using System;
using SlipAndJump.BoardMovers;
using SlipAndJump.BoardMovers.Enemies;
using UnityEngine;

namespace SlipAndJump.Board {
    public class SpawnerNode : BoardNode {
        public PlatformNode forwardNode;
        public MapDirections forwardDirection;


        public void Awake() {
            landingPosition = transform;
        }

        public Enemy Spawn(Enemy enemyPrefab) {
            Enemy e = Instantiate(enemyPrefab);
            e.facing = forwardDirection;
            e.currentNode = this;
            e.transform.position = landingPosition.position;
            e.transform.Rotate(new Vector3(0, (int) e.facing * 90, 0));
            e.PrepareTurn();
            return e;
        }
    }
}