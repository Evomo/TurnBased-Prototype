using System;
using System.Security.Cryptography;
using SlipAndJump.Board;
using SlipAndJump.Commands;
using UnityEngine;
using UnityEngine.XR;

namespace SlipAndJump.BoardMovers.Enemies {
    public enum BounceType {
        Reflect,
        Left,
        Right
    }

    public class Enemy : BaseMover {
        public PlatformNode next;
        public int hitpoints = 5;
        private int _rotationSteps = 0;

        [Tooltip("Should the enemy get the inverse direction or its neighbor?")]
        public BounceType bounceDir;

        public override void Start() {
            base.Start();
            board.onTurn.AddListener(() => PrepareTurn());
        }

        private void Update() {
            Vector2Int dir = DirectionHelpers.DirectionsDelta(facing);
            Debug.DrawRay(transform.position, new Vector3(dir.x, 0, dir.y), Color.magenta);
        }

        public void PrepareTurn(int steps = 0) {
            next = GetPlatformAfterMovement();
            _rotationSteps = steps;
            TurnHandler.Instance.EnqueueCommand(new DelegateCommand(Move));
        }

        public override void Move() {
            if (next) {
                StartCoroutine(JumpTo(next));
                StartCoroutine(RotateLerp(_rotationSteps));
                currentNode = next;
            }
            else {
                HandleCollision();
            }
        }

        public void HandleDestroy() {
            board.enemies.Remove(this);
            Destroy(gameObject);
        }

        public override void HandleCollision() {
            if (--hitpoints > 0) {
                int nextRotSteps = 0;
                if (bounceDir == BounceType.Reflect) {
                    facing = DirectionHelpers.Inverse(facing);
                    nextRotSteps = 2;
                }
                else {
                    facing = DirectionHelpers.GetNeighbor(facing, bounceDir == BounceType.Left);
                    nextRotSteps = bounceDir == BounceType.Left ? -1 : 1;
                }

                PrepareTurn(nextRotSteps);
            }
            else {
                TurnHandler.Instance.EnqueueCommand(new DelegateCommand(HandleDestroy));
            }
        }
    }
}