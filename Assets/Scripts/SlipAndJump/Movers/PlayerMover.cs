using System;
using SlipAndJump.Board;
using SlipAndJump.Board.Platform;
using SlipAndJump.Util;
using UnityEngine;
using UnityEngine.Events;

namespace SlipAndJump.BoardMovers {
    [Serializable]
    public class RotateEvent : UnityEvent<bool> { }

    public class PlayerMover : BaseMover {
        private int _score;
        [HideInInspector]
        public RotateEvent onPreRotate;
        public Color color;
        public int Score {
            get { return _score; }
            set { _score = value; }
        }

        public override void Start() {
            base.Start();
            canMove = true;
            currentNode = Board.StartNode;
            transform.position = currentNode.landingPosition.position;
            onNodeChange.AddListener((node) => InteractWithNode(node));
        }

        private void InteractWithNode(PlatformNode node) {
            node.ActivateSpecialEffect(this);
        }


        public void Turn(bool ccw) {
            onPreRotate.Invoke(ccw);

            MapDirections next = facing.GetNeighbor(ccw);
            facing = next;
            StartCoroutine(RotateLerp(ccw ? -1 : 1));
            // transform.Rotate(Vector3.up, ccw ? -90 : 90);
        }

        public void InteractWithPlatform() {
            if (currentNode is PlatformNode) {
                ((PlatformNode) currentNode).MarkPlatform(this);
            }
        }
        
        
        //Collision with enemy 
        public override void HandleCollision() {
            //TODO
            Debug.Log("Collided ");
        }
    }
}