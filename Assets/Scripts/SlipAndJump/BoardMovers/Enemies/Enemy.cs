using System;
using System.Security.Cryptography;
using SlipAndJump.Board;
using SlipAndJump.Board.Spawner;
using SlipAndJump.Commands;
using SlipAndJump.Util;
using UnityEngine;
using UnityEngine.XR;

namespace SlipAndJump.BoardMovers.Enemies {
    public enum BounceType {
        Reflect,
        Left,
        Right
    }

    public class Enemy : BaseMover, ISpawnable<Enemy> {
        public int hitpoints = 5;
        private int _rotationSteps = 0;
        public float collisionDepth = 1;

        [Tooltip("Should the enemy get the inverse direction or its neighbor?")]
        public BounceType bounceDir;

        #region Unity

        public override void Start() {
            base.Start();

            Board.onTurn.AddListener(() => PrepareTurn());
        }

        private void Update() {
            if (next) {
                Debug.DrawLine(transform.position, next.landingPosition.position, Color.magenta);
            }
        }

        #endregion

        #region Turns

        public void PrepareTurn(int steps = 0, float colDepth = 1) {
            collisionDepth = colDepth;
            JumpDuration = TurnHandler.Instance.turnDuration / collisionDepth;
            next = GetPlatformAfterMovement();
            _rotationSteps = steps;

            TurnHandler.Instance.EnqueueCommand(new ActionCommand(Move));
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

        #endregion

        #region Spawn
    
        public Enemy Spawn(BoardNode spawnerNode) {
            SpawnerNode sp;
            if (spawnerNode.TryGetComponent(out sp)) {
                facing = sp.forwardDirection;
                currentNode = spawnerNode;
                transform.position = spawnerNode.landingPosition.position;
                transform.Rotate(new Vector3(0, (int) facing * 90, 0));
                PrepareTurn();
                return this;
            }

            SpawnerManager.Despawn(this);
            
            return null;
        }

        #endregion

        #region Collision

        public void HandleDestroy() {
            SpawnerManager.Despawn(this);
            Destroy(gameObject);
        }

        public void HandleCollision(bool sameType) {
            BounceType temp = bounceDir;
            if (sameType) {
                bounceDir = (BounceType) (((int) bounceDir + collisionDepth) % 3);
                next = GetPlatformAfterMovement();
            }

            HandleCollision();
            bounceDir = temp;
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

                PrepareTurn(nextRotSteps, collisionDepth + 1);
            }
            else {
                TurnHandler.Instance.EnqueueCommand(new ActionCommand(HandleDestroy));
            }
        }

        #endregion


    }
}