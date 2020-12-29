using System.Collections.Generic;
using MotionAI.Core.Util;
using SlipAndJump.BoardMovers.Enemies;
using SlipAndJump.Collectables;
using UnityEngine;

namespace SlipAndJump.Board.Spawner {
    [RequireComponent(typeof(MapBoard))]
    [DisallowMultipleComponent]
    public class SpawnerManager : Singleton<SpawnerManager> {
        private MapBoard _board;
        private TurnHandler _turnHandler;

        public SpawnPlan spawnPlan;
        private List<Enemy> _enemies;
        private List<Collectable> _collectables;
        
        public List<Enemy> Enemies => _enemies;
        public List<Collectable> Collectables => _collectables;

        private void Start() {
            _enemies = new List<Enemy>();
            _collectables = new List<Collectable>();
            _board = GetComponent<MapBoard>();
            _turnHandler = GetComponent<TurnHandler>();
            _board.onTurn.AddListener(() => Spawn());
        }

        private void Spawn() {
            // if (_turnHandler.turnNumber % 2 == 0) {
            Enemy e = _board.spawnerNodes.RandomElement().Spawn(spawnPlan.SampleEnemy());
            _enemies.Add(e);
            // }

            //TODO spawn collectables in a sm0rt way
            // if (_board.goal == null) {
            // PlatformNode n = _board.Platforms.ToList().RandomElement().ToList().RandomElement();
            // Instantiate(collectables.RandomElement()).Spawn(n);
            // }
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