using System.Collections;
using SlipAndJump.Board;
using UnityEngine;

namespace SlipAndJump.BoardMovers {
    public abstract class BaseMover : MonoBehaviour {
        private protected MapBoard board;

        public BoardNode currentNode;
        public MapDirections facing;
        public bool canMove;

        public AnimationCurve curve;

        [Range(0.1f, 1)] public float jumpDuration = 0.5f;
        [Range(2, 5)] public float jumpHeight = 0;

        public virtual void Start() {
            board = FindObjectOfType<MapBoard>();
        }


        public virtual void HandleCollision(BaseMover collidedWith) {
            Debug.Log("Collided ");
        }

        public PlatformNode ForwardPlatform() {
            if (currentNode is SpawnerNode) {
                SpawnerNode c = currentNode as SpawnerNode;
                return c.forwardNode;
            }
            else {
                PlatformNode bn = currentNode as PlatformNode;
                Vector2Int coordinates = bn.coordinates;
                Vector2Int delta = DirectionHelpers.DirectionsDelta(facing);

                return board.GetPlatform(coordinates + delta);
            }
        }

        public virtual void Move() {
            PlatformNode next = ForwardPlatform();
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