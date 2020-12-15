using System;
using MotionAI.Core.Controller;
using MotionAI.Core.Models.Generated;
using MotionAI.Core.POCO;
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
                DelegateCommand.VoidDel toEnqueue = null;
                switch (msg.typeID) {
                    case MovementEnum.turn_90_left:
                        toEnqueue = TurnLeft;
                        // player.Turn(true);
                        break;
                    case MovementEnum.turn_90_right:
                        toEnqueue = TurnRight;
                        // player.Turn(false);
                        break;
                    case MovementEnum.hop_single:
                        toEnqueue = player.Move;
                        // player.MoveForward();
                        break;
                }

                _invoker.EnqueueCommand(new DelegateCommand(toEnqueue));
                _invoker.ProcessTurn();
            }
        }
    }
}