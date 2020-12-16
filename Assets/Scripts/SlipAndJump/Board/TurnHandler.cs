using System.Collections;
using System.Collections.Generic;
using MotionAI.Core.Util;
using SlipAndJump.Board;
using SlipAndJump.BoardMovers.Enemies;
using UnityEngine;

namespace SlipAndJump.Commands {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MapBoard))]
    public class TurnHandler : Singleton<TurnHandler> {
        private Queue<ICommand> _commandBuffer;
        private MapBoard _board;
        public int turnNumber;
        [Range(0.1f, .5f)] public float turnDuration = 0.5f;

        private void Awake() {
            _commandBuffer = new Queue<ICommand>();
            _board = GetComponent<MapBoard>();
        }


        public void EmptyQueue() {
            while (_commandBuffer.Count > 0) {
                try {
                    ICommand c = _commandBuffer.Dequeue();
                    c.Execute();
                }
                catch (MissingReferenceException) { }
            }
        }

        public void ProcessTurn() {
            EmptyQueue();
            StartCoroutine(CheckCollisions());
            turnNumber++;
        }


        private IEnumerator CheckCollisions() {
            yield return new WaitForSeconds(turnDuration);
            foreach (Enemy enemy in _board.enemies) {
                if (enemy.currentNode == _board.player.currentNode) {
                    EnqueueCommand(new DelegateCommand(enemy.HandleDestroy));
                    EnqueueCommand(new DelegateCommand(_board.player.HandleCollision));
                }
            }

            HashSet<Enemy> processed = new HashSet<Enemy>();
            foreach (Enemy e1 in _board.enemies) {
                foreach (Enemy e in _board.enemies) {
                    if (e1 != e && e.currentNode == e1.currentNode && !processed.Contains(e)) {
                        processed.Add(e);
                        e.CollisionBounce();
                    }
                }
            }

            bool shouldWait = _commandBuffer.Count > 0;
            EmptyQueue();
            if (shouldWait) {
                yield return new WaitForSeconds(turnDuration);
            }

            _board.onTurn.Invoke();
        }

        public void EnqueueCommand(ICommand command) {
            if (command != null) {
                _commandBuffer.Enqueue(command);
            }
        }
    }
}