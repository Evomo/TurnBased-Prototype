using System.Collections.Generic;
using SlipAndJump.Board;
using UnityEngine;

namespace SlipAndJump.Commands {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MapBoard))]
    public class TurnHandler : MonoBehaviour {
        private Queue<ICommand> _commandBuffer;
        private MapBoard _board;
        public int turnNumber;

        private void Awake() {
            _commandBuffer = new Queue<ICommand>();
            _board = GetComponent<MapBoard>();
        }


        public void ProcessTurn() {
            while (_commandBuffer.Count > 0) {
                ICommand c = _commandBuffer.Dequeue();
                c.Execute();
            }

            _board.onTurn.Invoke();
            turnNumber++;
        }


        public void EnqueueCommand(ICommand command) {
            if (command != null) {
                _commandBuffer.Enqueue(command);
            }
        }
    }
}