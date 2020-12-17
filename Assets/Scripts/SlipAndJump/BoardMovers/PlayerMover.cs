using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlipAndJump.BoardMovers {
    public class PlayerMover : BaseMover {
        public override void Start() {
            base.Start();
            canMove = true;
            currentNode = board.StartNode;
            transform.position = currentNode.landingPosition.position;
        }

        public void Turn(bool ccw) {
            MapDirections next = DirectionHelpers.GetNeighbor(facing, ccw);
            facing = next;
            StartCoroutine(RotateLerp(ccw ? -1 : 1));
            // transform.Rotate(Vector3.up, ccw ? -90 : 90);
        }



    }
}