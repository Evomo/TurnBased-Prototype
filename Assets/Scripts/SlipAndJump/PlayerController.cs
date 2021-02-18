using System;
using MotionAI.Core.Controller;
using MotionAI.Core.Models.Generated;
using MotionAI.Core.POCO;
using SlipAndJump.Board;
using SlipAndJump.BoardMovers;
using SlipAndJump.Commands;
using UnityEngine;

namespace SlipAndJump {
    public class PlayerController : MotionAIController {
        public PlayerMover player;

        [SerializeField] private TurnHandler turnHandler;

        protected override void HandleMovement(EvoMovement msg) {
            if (player.canMove) {
                Action toEnqueue = null;
                switch (msg.typeID) {
                    case MovementEnum.turn_90_left:
                        player.Turn(true);
                        break;
                    case MovementEnum.turn_90_right:
                        player.Turn(false);
                        break;
                    case MovementEnum.hop_single:
                        toEnqueue = player.Move;
                        break;
                    case MovementEnum.duck:
                        toEnqueue = player.InteractWithPlatform;
                        break;
                }


                if (!turnHandler.processingTurn && toEnqueue != null) {
                    turnHandler.EnqueueCommand(new ActionCommand(toEnqueue), TurnType.Player);
                    turnHandler.ProcessTurn();
                }
            }
        }
    }
}