using System;
using System.Collections;
using ElRaccoone.Tweens;
using MotionAI.Core.POCO;
using SlipAndJump.Board;
using UnityEngine;

namespace SlipAndJump {
    public class PlayerMover : BoardMover.BoardMover {
        public AnimationCurve curve;

        public bool canMove;
        [Range(0.1f, 1)] public float jumpDuration = 0.5f;
        [Range(2, 5)] public float jumpHeight = 3;

        public override void Start() {
            base.Start();
            canMove = true;
            currentNode = board.StartNode;
            transform.position = currentNode.landingPosition.position;

        }
        
        public void MoveForward() {
            PlatformNode next = ForwardPlatform();
            if (next) {
                StartCoroutine(JumpTo(next));
                currentNode = next;
            }
        }

        public void Turn(bool ccw) {
            MapDirections next = DirectionHelpers.GetNeighbor(facing, ccw);
            facing = next;
            transform.Rotate(Vector3.up, ccw ? -90 : 90);
        }

        IEnumerator JumpTo(PlatformNode next) {
            canMove = false;
            float time = 0f;
            Vector3 start = transform.position;
            Vector3 end = next.landingPosition.position;
            while (time < jumpDuration) {
                time += Time.deltaTime;


                float linearT = time / jumpDuration;
                float heightT = curve.Evaluate(linearT);

                float height = Mathf.Lerp(0f, jumpHeight, heightT); // change 3 to however tall you want the arc to be

                transform.position = Vector3.Lerp(start, end, linearT) + Vector3.up * height;

                yield return null;
            }

            canMove = true;
        }
    }
}