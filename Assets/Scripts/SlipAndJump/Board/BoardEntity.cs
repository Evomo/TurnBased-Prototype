using System;
using UnityEngine;

namespace SlipAndJump.Board {
    public class BoardEntity : MonoBehaviour {
        public BoardNode currentNode;
        private protected MapBoard Board;

        private void Awake() {
            Board = FindObjectOfType<MapBoard>();
        }
    }
}