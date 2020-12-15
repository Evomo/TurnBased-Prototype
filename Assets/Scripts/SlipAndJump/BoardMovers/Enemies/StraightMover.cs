using SlipAndJump.Board;
using UnityEngine;

namespace SlipAndJump.BoardMovers.Enemies {
    public class StraightMover : Enemy {
        public override void Move() {
            PlatformNode next = ForwardPlatform();
            base.Move();
            if (next == null) {
                //TODO add animation
                Debug.Log("should destroy");
                Destroy(gameObject);
            }
        }
    }
}