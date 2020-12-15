using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlipAndJump.BoardMovers {
    public class PlayerMover : BoardMovers.BaseMover {
        public override void Start() {
            base.Start();
            canMove = true;
            currentNode = board.StartNode;
            transform.position = currentNode.landingPosition.position;
        }

        public void Turn(bool ccw) {
            MapDirections next = DirectionHelpers.GetNeighbor(facing, ccw);
            facing = next;
            StartCoroutine(RotateLerp(ccw));
            // transform.Rotate(Vector3.up, ccw ? -90 : 90);
        }

        private IEnumerator RotateLerp(bool ccw) {
            canMove = false;
            float time = 0f;
            Quaternion start = transform.rotation;
            Quaternion end = start * Quaternion.Euler(0,  ccw ? -90 : 90,0);


            while (time < jumpDuration) {
                time += Time.deltaTime;


                float linearT = time / jumpDuration;

                transform.rotation = Quaternion.Lerp(start, end, linearT);

                yield return null;
            }

            canMove = true;
        }
    }
}