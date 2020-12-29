using System;
using SlipAndJump.Board;
using SlipAndJump.Board.Spawner;
using SlipAndJump.BoardMovers;
using UnityEngine;

namespace SlipAndJump.Collectables {
    public class Collectable : BoardEntity, ISpawnable<Collectable> {
        public void Update() {
            transform.Rotate(Vector3.up, 30 * Time.deltaTime);
        }

        public virtual void HandleEffect(PlayerMover player) {
            player.Score += 1;
            Debug.Log(1);
            //TODO pool 
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
    }
}