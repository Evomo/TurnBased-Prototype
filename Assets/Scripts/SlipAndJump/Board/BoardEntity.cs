using System;
using SlipAndJump.Board.Platform;
using UnityEngine;

namespace SlipAndJump.Board {
    public class BoardEntity : MonoBehaviour {
        public BoardNode currentNode;
        private protected MapBoard Board;

        private void Awake() {
            Board = FindObjectOfType<MapBoard>();
        }



        public virtual bool CollidesWith(BoardEntity other, bool nextTurn = false) {
            return currentNode == other.currentNode;
        }
    }
}