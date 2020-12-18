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
            HandleCollisions();
            turnNumber++;
        }


        private void HandleCollisions() {
            StartCoroutine(CheckCollisions());
        }

        private IEnumerator CheckCollisions(float depth = 1) {
            yield return new WaitForSeconds(turnDuration / depth);
            if (_board.goal) {
                if (_board.goal.currentNode == _board.player.currentNode) {
                    EnqueueCommand(new PlayerActionCommand(_board.goal.HandleEffect, _board.player));
                
                } 
            }

            foreach (Enemy enemy in _board.enemies) {
                if (enemy.currentNode == _board.player.currentNode) {
                    EnqueueCommand(new ActionCommand(enemy.HandleDestroy));
                    EnqueueCommand(new ActionCommand(() => _board.player.HandleCollision()));
                }
            }

            EmptyQueue();

            HashSet<Enemy> processed = new HashSet<Enemy>();
            bool enemyCollided = false;
            foreach (Enemy e1 in _board.enemies) {
                foreach (Enemy e in _board.enemies) {
                    if (e1 != e && e.currentNode == e1.currentNode && !processed.Contains(e)) {
                        processed.Add(e);
                        //change reflection type if it's the same type
                        bool reflectionOverride = !processed.Contains(e1) && e.next == e1.next;
                        enemyCollided = true;
                        e.HandleCollision(reflectionOverride);
                    }
                }
            }

            bool shouldWait = _commandBuffer.Count > 0;
            EmptyQueue();
            if (shouldWait) {
                yield return new WaitForSeconds(turnDuration);
            }

            if (enemyCollided) {
                yield return StartCoroutine(CheckCollisions(depth + 1));
            }
            else {
                _board.onTurn.Invoke();
            }
        }

        public void EnqueueCommand(ICommand command) {
            if (command != null) {
                _commandBuffer.Enqueue(command);
            }
        }
    }
}