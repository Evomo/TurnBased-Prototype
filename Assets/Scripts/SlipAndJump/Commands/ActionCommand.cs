using System;
using System.Reflection;
using UnityEngine;

namespace SlipAndJump.Commands {
    public class ActionCommand : ICommand {
        private Action toExecute;

        public ActionCommand(Action method) {
            toExecute = method;
        }

        public void Execute() {
            toExecute();
        }
    }
}