using System;
using SlipAndJump.Util;
using UnityEngine.Events;

namespace SlipAndJump.BoardMovers {
    
    [Serializable]
    public class  RotateEvent : UnityEvent<bool>{}
    public class PlayerMover : BaseMover {
        private int _score;
        public RotateEvent onRotate;
        public int Score {
            get { return _score;}
            set {
                _score = value;
            }
        }
        public override void Start() {
            base.Start();
            canMove = true;
            currentNode = Board.StartNode;
            transform.position = currentNode.landingPosition.position;
        }

        public void Turn(bool ccw) {
            MapDirections next = DirectionHelpers.GetNeighbor(facing, ccw);
            facing = next;
            StartCoroutine(RotateLerp(ccw ? -1 : 1));
            // transform.Rotate(Vector3.up, ccw ? -90 : 90);
            onRotate.Invoke(ccw);
        }
        
    }
}