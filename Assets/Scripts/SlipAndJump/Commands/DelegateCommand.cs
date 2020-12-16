﻿using System;
using System.Reflection;
using UnityEngine;

namespace SlipAndJump.Commands {
    public class DelegateCommand : ICommand {
        public delegate void VoidDel();

        private VoidDel toExecute;

        public DelegateCommand(VoidDel method) {
            toExecute = method;
        }

        public void Execute() {
                toExecute();
       }
    }
}