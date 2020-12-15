using System;

namespace SlipAndJump.Board {
    public class SpawnerNode : BoardNode{
        public PlatformNode forwardNode;


        
        public void Awake() {
            this.landingPosition = transform;
        }
    }
}