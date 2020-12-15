using System;
using System.Collections.Generic;
using MotionAI.Core.Util;
using UnityEngine;

namespace SlipAndJump.Commands {
    [DisallowMultipleComponent]
    public class CommandInvoker : Singleton<CommandInvoker> {
        private Queue<ICommand> _commandBuffer;

        private void Awake() {
            _commandBuffer = new Queue<ICommand>();
        }


        public void RunCommands() {
            while (_commandBuffer.Count > 0) {
                ICommand c = _commandBuffer.Dequeue();
                c.Execute();
            }
        }

        private void Update() {
           RunCommands(); 
        }

        public void EnqueueCommand(ICommand command) {
            if (command != null) {
                _commandBuffer.Enqueue(command);
            }
        }

        public static void AddCommand(ICommand command) {
            CommandInvoker.Instance.EnqueueCommand(command);
        }
    }
}