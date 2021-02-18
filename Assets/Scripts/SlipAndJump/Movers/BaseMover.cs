using System.Collections;
using SlipAndJump.Board;
using SlipAndJump.Board.Platform;
using SlipAndJump.Commands;
using SlipAndJump.Util;
using UnityEngine;
using UnityEngine.Events;

namespace SlipAndJump.BoardMovers {
    public class NodeChangeEvent : UnityEvent<PlatformNode>{};

    public abstract class BaseMover : BoardEntity {
        public int hitpoints = 5;
        public MovementPattern movementPattern;
        public AnimationCurve curve;
        public MapDirections facing;
        public PlatformNode next;
        public NodeChangeEvent onNodeChange;
        [Range(2, 5)] public float jumpHeight = 0;
        public bool canMove;
        protected float JumpDuration;


        public virtual void Start() {
            onNodeChange = new NodeChangeEvent();
            JumpDuration = TurnHandler.Instance.turnDuration;
            Board.onTurn.AddListener(() => canMove = true);
        }


        public PlatformNode GetPlatformAfterMovement() {
            PlatformNode tmpNext = null;
            if (currentNode is SpawnerNode) {
                SpawnerNode c = currentNode as SpawnerNode;
                tmpNext = c.forwardNode;
            }
            else {
                PlatformNode bn = currentNode as PlatformNode;
                Vector2Int coordinates = bn.Coordinates;
                foreach (MovementOptions currentMovement in movementPattern.moves) {
                    Vector2Int delta = currentMovement == MovementOptions.Forward
                        ? facing.DirectionsDelta()
                        : facing.GetNeighbor(currentMovement == MovementOptions.Left).DirectionsDelta();

                    coordinates = coordinates + delta;
                    tmpNext = Board.GetPlatform(coordinates);
                }
            }

            return tmpNext;
        }


        public virtual void Move() {
            PlatformNode tmpNext = GetPlatformAfterMovement();
            if (tmpNext) {
                StartCoroutine(JumpTo(tmpNext));
                currentNode = tmpNext;
                onNodeChange.Invoke(tmpNext);
            }
        }

        
        //Collision with enemy 
        public abstract void HandleCollision();

        #region Movement Helpers

        public override bool CollidesWith(BoardEntity other, bool nextTurn = false) {
            BaseMover bm;
            if (other.gameObject.TryGetComponent(out bm) && nextTurn) {
                return bm.next == next;
            }

            return base.CollidesWith(other);
        }

        protected IEnumerator JumpTo(PlatformNode tmpNext) {
            canMove = false;
            float time = 0f;
            Vector3 start = transform.position;
            Vector3 end = tmpNext.landingPosition.position;
            while (time < JumpDuration) {
                time += Time.deltaTime;


                float linearT = time / JumpDuration;
                float heightT = curve.Evaluate(linearT);

                float height = Mathf.Lerp(0f, jumpHeight, heightT); // change 3 to however tall you want the arc to be

                transform.position = Vector3.Lerp(start, end, linearT) + Vector3.up * height;

                yield return null;
            }
        }

        protected IEnumerator RotateLerp(int cwSteps) {
            float time = 0f;
            Quaternion start = transform.rotation;
            Quaternion end = start * Quaternion.Euler(0, cwSteps * 90, 0);


            while (time < JumpDuration) {
                time += Time.deltaTime;


                float linearT = time / JumpDuration;

                transform.rotation = Quaternion.Lerp(start, end, linearT);

                yield return null;
            }

        }

        #endregion
    }
}