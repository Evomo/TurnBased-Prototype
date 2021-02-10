using System;
using SlipAndJump.Board;
using SlipAndJump.Board.Platform;
using SlipAndJump.Board.Spawner;
using SlipAndJump.BoardMovers;
using SlipAndJump.Commands;
using UnityEngine;

namespace SlipAndJump.Collectables {
    public class Collectable : BoardEntity, ISpawnable<Collectable> {
        private bool _collected = false;

        public int scoreValue = 1; 
        public void Update() {
            transform.Rotate(Vector3.up, 30 * Time.deltaTime);
        }

        public virtual void HandleEffect(PlayerMover player) {
            player.Score += scoreValue;

            //TODO pool 
            SpawnerManager.Despawn(this);
            Destroy(gameObject);
        }

        public Collectable Spawn(BoardNode n) {
            PlatformNode pn;
            if (n.TryGetComponent(out pn)) {
                transform.position = n.landingPosition.position;
                currentNode = pn;
                return this;
            }

            SpawnerManager.Despawn(this);
            return null;
        }

        public void Collect(PlayerMover player) {
            if (!_collected) {
                TurnHandler.Instance.EnqueueCommand(new PlayerActionCommand(HandleEffect, player));
            }

            _collected = true;
        }
    }
}