using System;
using SlipAndJump.Board;
using SlipAndJump.BoardMovers;
using UnityEngine;

namespace SlipAndJump.Collectables {
    public class Collectable : BoardEntity {


        public void Update() {
            transform.Rotate(Vector3.up, 30 * Time.deltaTime);
        }

        public virtual void HandleEffect(PlayerMover player) {
            player.Score += 1;
            Board.goal = null;
            Debug.Log(1);
            //TODO pool 
            Destroy(gameObject);
        }

        public void Spawn(PlatformNode n) {
            transform.position = n.landingPosition.position;
            currentNode = n;
            Board.goal = this;
        }
    }
}