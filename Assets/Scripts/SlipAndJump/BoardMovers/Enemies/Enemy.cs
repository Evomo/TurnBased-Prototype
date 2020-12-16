using System.Security.Cryptography;
using SlipAndJump.Board;
using SlipAndJump.Commands;
using UnityEngine;
using UnityEngine.XR;

namespace SlipAndJump.BoardMovers.Enemies {
    public class Enemy : BaseMover {
        public PlatformNode next;
        public int hitpoints = 5;

        public override void Start() {
            base.Start();
            board.onTurn.AddListener(() => PrepareTurn());
        }

        public void PrepareTurn() {
            next = GetPlatformAfterMovement();

            TurnHandler.Instance.EnqueueCommand(new DelegateCommand(Move));
        }

        public override void Move() {
            if (next) {
                StartCoroutine(JumpTo(next));
                currentNode = next;
            }
            else {
                CollisionBounce();
            }
        }

        public void HandleDestroy() {
            board.enemies.Remove(this);
            Destroy(gameObject);
        }

        public void CollisionBounce() {
            if (--hitpoints > 0) {
                facing = DirectionHelpers.Inverse(facing);
                PrepareTurn();
            }
            else {
                TurnHandler.Instance.EnqueueCommand(new DelegateCommand(HandleDestroy));
            }
        }
    }
}