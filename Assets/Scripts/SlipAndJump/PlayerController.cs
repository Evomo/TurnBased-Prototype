using MotionAI.Core.Controller;
using MotionAI.Core.Models.Generated;
using MotionAI.Core.POCO;

namespace SlipAndJump {
    public class PlayerController : MotionAIController {
        public PlayerAnimal player;


        protected override void HandleMovement(EvoMovement msg) {
            switch (msg.typeID) {
                case MovementEnum.turn_90_left:
                    player.Turn(true);
                    break;
                case MovementEnum.turn_90_right:
                    player.Turn(false);
                    break;
                case MovementEnum.hop_single:
                    player.MoveForward();
                    break;
            }
        }
    }
}