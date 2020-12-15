using System;
using System.Collections;
using ElRaccoone.Tweens;
using MotionAI.Core.POCO;
using UnityEngine;

namespace SlipAndJump {
    public class PlayerAnimal : MonoBehaviour {
        public MapGraph board;
        public AnimationCurve curve;

        public PlatformNode currentNode;
        public MapDirections facing;

        [Range(0.1f, 1)] public float jumpDuration = 0.5f;
        [Range(2, 5)] public float jumpHeight = 3;

        public void Start() {
            board = FindObjectOfType<MapGraph>();
            currentNode = board.StartNode;
            transform.position = currentNode.landingPosition.position;
        }
        
        public PlatformNode ForwardPlatform() {
            Vector2Int coordinates = currentNode.coordinates;
            Vector2Int delta = DirectionHelpers.DirectionsDelta(facing);

            return board.GetPlatform(coordinates + delta);
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
        }
    }
}