using System;
using SlipAndJump.BoardMovers;

namespace SlipAndJump.Board {
    public class SpawnerNode : BoardNode{
        public PlatformNode forwardNode;
        public MapDirections forwardDirection;

        
        public void Awake() {
            this.landingPosition = transform;
        }

        public void Spawn(BaseMover enemyPrefab) {
            BaseMover e = Instantiate(enemyPrefab);
            e.facing = forwardDirection;
            e.currentNode = this;
            e.transform.position = landingPosition.position;
        }
    }
}