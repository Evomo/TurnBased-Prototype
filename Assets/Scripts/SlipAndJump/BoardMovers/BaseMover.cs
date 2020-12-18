using System.Collections;
using SlipAndJump.Board;
using SlipAndJump.Commands;
using UnityEngine;

namespace SlipAndJump.BoardMovers {
    public abstract class BaseMover : BoardEntity {
        public MovementPattern movementPattern;
        public AnimationCurve curve;
        public MapDirections facing;

        [Range(2, 5)] public float jumpHeight = 0;
        public bool canMove;
        protected float JumpDuration;

        public virtual void Start() {
            JumpDuration = TurnHandler.Instance.turnDuration;
            Board.onTurn.AddListener(() => canMove = true);
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
                    next = Board.GetPlatform(coordinates);
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

        public virtual void HandleCollision() {
            //TODO
            Debug.Log("Collided ");
        }

        #region Movement Coroutines

        

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
        }

        protected IEnumerator RotateLerp(int cwSteps) {
            canMove = false;
            float time = 0f;
            Quaternion start = transform.rotation;
            Quaternion end = start * Quaternion.Euler(0, cwSteps * 90, 0);


            while (time < JumpDuration) {
                time += Time.deltaTime;


                float linearT = time / JumpDuration;

                transform.rotation = Quaternion.Lerp(start, end, linearT);

                yield return null;
            }

            // canMove = true;
        }
        #endregion

    }
}