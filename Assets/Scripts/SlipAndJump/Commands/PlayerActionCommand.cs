using System;
using SlipAndJump.BoardMovers;

namespace SlipAndJump.Commands {
    public class PlayerActionCommand : ICommand {
        public delegate void Action(PlayerMover player);

        private Action toExecute;
        private PlayerMover _player;

        public PlayerActionCommand(Action method, PlayerMover p) {
            toExecute = method;
            _player = p;
        }

        public void Execute() {
            toExecute(_player);
        }
    }
}