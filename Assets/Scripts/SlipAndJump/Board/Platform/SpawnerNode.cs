﻿using System;
using SlipAndJump.Board.Platform;
using SlipAndJump.BoardMovers;
using SlipAndJump.BoardMovers.Enemies;
using SlipAndJump.Util;
using UnityEngine;

namespace SlipAndJump.Board {
    public class SpawnerNode : BoardNode {
        public PlatformNode forwardNode;
        public MapDirections forwardDirection;


        public void Awake() {
            landingPosition = transform;
        }

        public Enemy Spawn(Enemy enemyPrefab) {
            Enemy e = Instantiate(enemyPrefab);
            return e.Spawn(this);

        }
    }
}