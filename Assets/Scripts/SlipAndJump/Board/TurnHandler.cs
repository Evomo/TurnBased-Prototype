using System.Collections;
using System.Collections.Generic;
using MotionAI.Core.Util;
using SlipAndJump.BoardMovers;
using SlipAndJump.BoardMovers.Enemies;
using SlipAndJump.Commands;
using UnityEngine;

namespace SlipAndJump.Board {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MapBoard))]
    public class TurnHandler : Singleton<TurnHandler> {
        private Queue<ICommand> _commandBuffer;
        private MapBoard _board;
        
        [Range(0.1f, .5f)] public float turnDuration = 0.5f;
        public int turnNumber;

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
            PlayerMover player = _board.player;
            if (_board.goal) {
                if (_board.goal.Collides(player)) {
                    EnqueueCommand(new PlayerActionCommand(_board.goal.HandleEffect, _board.player));
                }
            }

            foreach (Enemy enemy in _board.enemies) {
                if (enemy.Collides(player)) {
                    EnqueueCommand(new ActionCommand(enemy.HandleDestroy));
                    EnqueueCommand(new ActionCommand(() => _board.player.HandleCollision()));
                }
            }

            EmptyQueue();

            HashSet<Enemy> processed = new HashSet<Enemy>();
            bool enemyCollided = false;
            foreach (Enemy e1 in _board.enemies) {
                foreach (Enemy e in _board.enemies) {
                    if (e1 != e && e.Collides(e1) && !processed.Contains(e)) {
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