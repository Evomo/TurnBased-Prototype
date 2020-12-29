using System.Collections;
using System.Collections.Generic;
using MotionAI.Core.Util;
using SlipAndJump.Board.Spawner;
using SlipAndJump.BoardMovers;
using SlipAndJump.BoardMovers.Enemies;
using SlipAndJump.Collectables;
using SlipAndJump.Commands;
using UnityEngine;

namespace SlipAndJump.Board {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MapBoard))]
    [RequireComponent(typeof(SpawnerManager))]
    public class TurnHandler : Singleton<TurnHandler> {
        private Queue<ICommand> _commandBuffer;
        private MapBoard _board;
        private SpawnerManager _spawnerManager;
        [Range(0.1f, .5f)] public float turnDuration = 0.5f;
        public int turnNumber;

        private void Awake() {
            _commandBuffer = new Queue<ICommand>();
            _board = GetComponent<MapBoard>();
            _spawnerManager = GetComponent<SpawnerManager>();
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

            foreach (Collectable collectable in _spawnerManager.Collectables) {
                if (collectable.CollidesWith(player)) {
                    collectable.Collect(player);
                }
            }

            foreach (Enemy enemy in _spawnerManager.Enemies) {
                if (enemy.CollidesWith(player)) {
                    EnqueueCommand(new ActionCommand(enemy.HandleDestroy));
                    EnqueueCommand(new ActionCommand(() => _board.player.HandleCollision()));
                }
            }

            EmptyQueue();

            HashSet<Enemy> processed = new HashSet<Enemy>();
            bool enemyCollided = false;
            foreach (Enemy e1 in _spawnerManager.Enemies) {
                foreach (Enemy e in _spawnerManager.Enemies) {
                    if (e1 != e && e.CollidesWith(e1) && !processed.Contains(e)) {
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