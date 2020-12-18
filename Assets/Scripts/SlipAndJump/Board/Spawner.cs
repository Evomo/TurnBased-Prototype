using System.Collections.Generic;
using System.Linq;
using MotionAI.Core.Util;
using SlipAndJump.BoardMovers.Enemies;
using SlipAndJump.Collectables;
using SlipAndJump.Commands;
using UnityEngine;

namespace SlipAndJump.Board {
    [RequireComponent(typeof(MapBoard))]
    public class Spawner : MonoBehaviour {
        private MapBoard _board;
        private TurnHandler _turnHandler;
        [SerializeField] private List<Enemy> enemies;
        [SerializeField] private List<Collectable> _collectables;

        private void Start() {
            _board = GetComponent<MapBoard>();
            _turnHandler = GetComponent<TurnHandler>();
            _board.onTurn.AddListener(() => Spawn());
        }

        private void Spawn() {
            // if (_turnHandler.turnNumber % 2 == 0) {
            Enemy e = _board.spawnerNodes.RandomElement().Spawn(enemies.RandomElement());
            _board.enemies.Add(e);
            // }

            if (_board.goal == null) {
                PlatformNode n = _board.Platforms.ToList().RandomElement().ToList().RandomElement();

                Instantiate(_collectables.RandomElement()).Spawn(n);
            }
        }
    }
}