using System;
using System.Collections.Generic;
using System.Linq;
using MotionAI.Core.Util;
using SlipAndJump.Board.Platform;
using SlipAndJump.BoardMovers.Enemies;
using SlipAndJump.Collectables;
using UnityEngine;

namespace SlipAndJump.Board.Spawner {
    [RequireComponent(typeof(MapBoard))]
    [DisallowMultipleComponent]
    public class SpawnerManager : Singleton<SpawnerManager> {
        private MapBoard _board;

        private List<Enemy> _enemies;
        private List<Collectable> _collectables;
        private Transform _spawnPos;


        public List<Enemy> Enemies => _enemies;
        public List<Collectable> Collectables => _collectables;
        public SpawnPlan spawnPlan;
        private TurnHandler _turnHandler;
        public SpawnOptions enemySpawnOptions, collectableSpawnOptions;
        [Range(1, 10)] public int maxCollectablesOnScreen = 1;
        [Range(10, 50)] public int maxEnemiesOnScreen = 1;



        private void Start() {
            _enemies = new List<Enemy>();
            _collectables = new List<Collectable>();
            _board = GetComponent<MapBoard>();
            _turnHandler = GetComponent<TurnHandler>();
            _board.onTurn.AddListener(() => Spawn());
            GameObject spPos = new GameObject();
            spPos.transform.position = transform.position;
            _spawnPos = spPos.transform;
            _spawnPos.name = "Spawned Entities";
            _spawnPos = spPos.transform;
        }

        private void Spawn() {
            if (_turnHandler.turnNumber % enemySpawnOptions.spawnModulo == 0
                && _enemies.Count < maxEnemiesOnScreen) {
                int amountToSpawn = enemySpawnOptions.AmountToSpawn(_turnHandler.turnNumber);
                HashSet<SpawnerNode> availableNodes = new HashSet<SpawnerNode>();
                while (availableNodes.Count < amountToSpawn) {
                    availableNodes.Add(_board.spawnerNodes.RandomElement());
                }

                foreach (SpawnerNode spawnNode in availableNodes) {
                    Enemy e = spawnNode.Spawn(spawnPlan.SampleEnemy());
                    e.transform.SetParent(_spawnPos);
                    _enemies.Add(e);
                    if (_enemies.Count == maxEnemiesOnScreen) break;

                }
            }


            if (_turnHandler.turnNumber % collectableSpawnOptions.spawnModulo == 0 &&
                _collectables.Count < maxCollectablesOnScreen) {
                int amountToSpawn = collectableSpawnOptions.AmountToSpawn(_turnHandler.turnNumber);

                HashSet<PlatformNode> availableNodes = new HashSet<PlatformNode>();
                List<PlatformNode[]> nodeRows = _board.Platforms.ToList();
                while (availableNodes.Count < amountToSpawn) {
                    List<PlatformNode> row = nodeRows.RandomElement().ToList();

                    availableNodes.Add(row.RandomElement());
                }

                foreach (PlatformNode spawnNode in availableNodes) {
                    Collectable e = spawnNode.Spawn(spawnPlan.SampleCollectable());
                    e.transform.SetParent(_spawnPos);
                    _collectables.Add(e);

                    if (_collectables.Count == maxCollectablesOnScreen) break;
                }
            }
        }

        public void DespawnInternal(Enemy enemy) {
            //TODO maybe pooling ? 
            _enemies.Remove(enemy);
        }

        public void DespawnInternal(Collectable col) {
            //TODO maybe pooling ? 
            _collectables.Remove(col);
        }


        public static void Despawn(BoardEntity entity) {
            Enemy enemy;

            Collectable collectable;
            if (entity.TryGetComponent(out enemy)) Instance.DespawnInternal(enemy);
            if (entity.TryGetComponent(out collectable)) Instance.DespawnInternal(collectable);
        }
    }
}