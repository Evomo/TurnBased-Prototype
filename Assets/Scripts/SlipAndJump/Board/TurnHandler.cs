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
    public enum TurnType {
        Player,
        Enemy
    }

    [DisallowMultipleComponent]
    [RequireComponent(typeof(MapBoard))]
    [RequireComponent(typeof(SpawnerManager))]
    public class TurnHandler : Singleton<TurnHandler> {
        private Queue<ICommand> _playerBuffer, _enemyBuffer, _currentBuffer;
        private MapBoard _board;
        private SpawnerManager _spawnerManager;
        public bool processingTurn;
        [Range(0.1f, .5f)] public float turnDuration = 0.5f;
        public int turnNumber;

        private void Awake() {
            _playerBuffer = new Queue<ICommand>();
            _enemyBuffer = new Queue<ICommand>();
            _board = GetComponent<MapBoard>();
            _spawnerManager = GetComponent<SpawnerManager>();
        }


        private IEnumerator EmptyQueue(TurnType bufferIdx = TurnType.Enemy) {
            _currentBuffer = bufferIdx == TurnType.Player ? _playerBuffer : _enemyBuffer;
            while (_currentBuffer.Count > 0) {
                try {
                    ICommand c = _currentBuffer.Dequeue();
                    c.Execute();
                }
                catch (MissingReferenceException) { }
            }

            yield return null;
        }

        public void ProcessTurn() {
            StartCoroutine(TurnCoroutine());
        }


        private IEnumerator TurnCoroutine() {
            processingTurn = true;
            yield return StartCoroutine(EmptyQueue(TurnType.Player));
            yield return new WaitForSeconds(turnDuration);
            yield return StartCoroutine(EmptyQueue());
            yield return StartCoroutine(CheckCollisions());
            turnNumber++;
            processingTurn = false;
            yield return null;
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
            yield return StartCoroutine(EmptyQueue());


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

            bool shouldWait = _playerBuffer.Count > 0;
            yield return StartCoroutine(EmptyQueue());

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

        public void EnqueueCommand(ICommand command, TurnType t = TurnType.Enemy) {
            if (command != null) {
                if (t == TurnType.Player) {
                    _playerBuffer.Enqueue(command);
                }
                else {
                    _enemyBuffer.Enqueue(command);
                }
            }
        }
    }
}