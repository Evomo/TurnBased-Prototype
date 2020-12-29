using System;
using System.Collections.Generic;
using System.Linq;
using MotionAI.Core.Util;
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
        public TurnHandler turnHandler;
        public SpawnOptions enemySpawnOptions, collectableSpawnOptions;
        [Range(1, 10)] public int maxCollectablesOnScreen = 1;
        [Range(10, 50)] public int maxEnemiesOnScreen = 1;

        #region Internal Classes

        [Serializable]
        public class SpawnOptions {
            public AnimationCurve spawnCurve;
            [Range(1, 10)] public int maxEntitiesSpawned;
            [Range(10, 100)] public int maxTurns;
            [Range(1, 10)] public int spawnModulo;


            public int AmountToSpawn(int currentTurn) {
                float normalizedTurn = Mathf.Clamp01((float) currentTurn / (float) maxTurns);

                return Mathf.Max(1, (int) (spawnCurve.Evaluate(normalizedTurn) * maxEntitiesSpawned));
            }
        }

        #endregion

        private void Start() {
            _enemies = new List<Enemy>();
            _collectables = new List<Collectable>();
            _board = GetComponent<MapBoard>();
            turnHandler = GetComponent<TurnHandler>();
            _board.onTurn.AddListener(() => Spawn());
            GameObject spPos = new GameObject();
            spPos.transform.position = transform.position;
            _spawnPos = spPos.transform;
            _spawnPos.name = "Spawned Entities";
            _spawnPos = spPos.transform;
        }

        private void Spawn() {
            if (turnHandler.turnNumber % enemySpawnOptions.spawnModulo == 0
                && _enemies.Count < maxEnemiesOnScreen) {
                int amountToSpawn = enemySpawnOptions.AmountToSpawn(turnHandler.turnNumber);
                Debug.Log($"Spawning {amountToSpawn} enemies");
                HashSet<SpawnerNode> availableNodes = new HashSet<SpawnerNode>();
                while (availableNodes.Count < amountToSpawn) {
                    availableNodes.Add(_board.spawnerNodes.RandomElement());
                }

                foreach (SpawnerNode spawnNode in availableNodes) {
                    Enemy e = spawnNode.Spawn(spawnPlan.SampleEnemy());
                    e.transform.SetParent(_spawnPos);
                    _enemies.Add(e);
                }
            }


            if (turnHandler.turnNumber % collectableSpawnOptions.spawnModulo == 0 &&
                _collectables.Count < maxCollectablesOnScreen) {
                int amountToSpawn = collectableSpawnOptions.AmountToSpawn(turnHandler.turnNumber);

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