using System;
using UnityEngine;

namespace SlipAndJump.Board {
    public class ProxyBoard {
        private Boolean[][] _proxyBoard;

        
        public Boolean[] this[int i]
        {
            get { return _proxyBoard[i]; }
        }
        public int Length => _proxyBoard.Length;
        public ProxyBoard(int size) {
            _proxyBoard = new Boolean[size][];
            for (int i = 0; i < size; i++) {
                _proxyBoard[i] = new bool[size];
            }
        }


        public void Clear() {
            for (int i = 0; i < _proxyBoard.Length; i++) {
                for (int j = 0; j < _proxyBoard.Length; j++) {
                    _proxyBoard[i][j] = false;
                }
            }
        }

        public void Mark(Vector2Int coord) {
            _proxyBoard[coord.x][coord.y] = true;
        }
    }
}