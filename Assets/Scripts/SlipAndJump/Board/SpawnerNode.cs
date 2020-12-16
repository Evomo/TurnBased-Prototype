using System;
using SlipAndJump.BoardMovers;
using SlipAndJump.BoardMovers.Enemies;

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
            e.PrepareTurn();
            return e;
        }
    }
}