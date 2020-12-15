using SlipAndJump.Board;
using UnityEngine;

namespace SlipAndJump.BoardMover {
    public class BoardMover : MonoBehaviour {
        public MapBoard board;

        public BoardNode currentNode;
        public MapDirections facing;


        public virtual void Start() {
            board = FindObjectOfType<MapBoard>();
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
    }
}