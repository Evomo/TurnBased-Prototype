using System.Collections.Generic;
using MotionAI.Core.Util;
using SlipAndJump.BoardMovers.Enemies;
using SlipAndJump.Commands;
using UnityEngine;

namespace SlipAndJump.Board {
    [RequireComponent(typeof(MapBoard))]
    public class Spawner : MonoBehaviour {
        private MapBoard _board;
        private TurnHandler _turnHandler;
        [SerializeField] private List<Enemy> enemies;

        private void Start() {
            _board = GetComponent<MapBoard>();
            _turnHandler = GetComponent<TurnHandler>();
            _board.onTurn.AddListener(() => Spawn());
        }

        private void Spawn() {
            // if (_turnHandler.turnNumber % 2 == 0) {
               Enemy e =  _board.spawnerNodes.RandomElement().Spawn(enemies.RandomElement());
               _board.enemies.Add(e);
            // }
        }
    }
}