using System.Collections;
using SlipAndJump.Board;
using SlipAndJump.Commands;
using UnityEngine;

namespace SlipAndJump.BoardMovers {
    public abstract class BaseMover : MonoBehaviour {
        private protected MapBoard board;

        public BoardNode currentNode;
        public MapDirections facing;

        public MovementPattern movementPattern;
        public AnimationCurve curve;

        private protected float JumpDuration;
        [Range(2, 5)] public float jumpHeight = 0;
        public bool canMove;

        public virtual void Start() {
            board = FindObjectOfType<MapBoard>();
            JumpDuration = TurnHandler.Instance.turnDuration;
        }


        public PlatformNode GetPlatformAfterMovement() {
            PlatformNode next = null;
            if (currentNode is SpawnerNode) {
                SpawnerNode c = currentNode as SpawnerNode;
                next = c.forwardNode;
            }
            else {
                PlatformNode bn = currentNode as PlatformNode;
                Vector2Int coordinates = bn.Coordinates;
                foreach (MovementOptions currentMovement in movementPattern.moves) {
                    Vector2Int delta = currentMovement == MovementOptions.Forward
                        ? DirectionHelpers.DirectionsDelta(facing)
                        : DirectionHelpers.DirectionsDelta(DirectionHelpers.GetNeighbor(facing,
                            currentMovement == MovementOptions.Left));

                    coordinates = coordinates + delta;
                    next = board.GetPlatform( coordinates);
                }
            }

            return next;
        }

        public virtual void Move() {
            PlatformNode next = GetPlatformAfterMovement();
            if (next) {
                StartCoroutine(JumpTo(next));
                currentNode = next;
            }
        }

        protected IEnumerator JumpTo(PlatformNode next) {
            canMove = false;
            float time = 0f;
            Vector3 start = transform.position;
            Vector3 end = next.landingPosition.position;
            while (time < JumpDuration) {
                time += Time.deltaTime;


                float linearT = time / JumpDuration;
                float heightT = curve.Evaluate(linearT);

                float height = Mathf.Lerp(0f, jumpHeight, heightT); // change 3 to however tall you want the arc to be

                transform.position = Vector3.Lerp(start, end, linearT) + Vector3.up * height;

                yield return null;
            }

            canMove = true;
        }
    }
}