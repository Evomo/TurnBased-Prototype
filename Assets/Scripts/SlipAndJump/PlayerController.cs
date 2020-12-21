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
        
        [SerializeField] private TurnHandler _invoker;


        public void TurnLeft() {
            player.Turn(true);
        }

        public void TurnRight() {
            player.Turn(false);
        }

        protected override void HandleMovement(EvoMovement msg) {
            if (player.canMove) {
                Action toEnqueue = null;
                switch (msg.typeID) {
                    case MovementEnum.turn_90_left:
                        toEnqueue = TurnLeft;
                        break;
                    case MovementEnum.turn_90_right:
                        toEnqueue = TurnRight;
                        break;
                    case MovementEnum.hop_single:
                        toEnqueue = player.Move;
                        break;
                }

                _invoker.EnqueueCommand(new ActionCommand(toEnqueue));
                _invoker.ProcessTurn();
            }
        }
    }
}