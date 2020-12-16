using System.Collections.Generic;
using UnityEngine;

namespace SlipAndJump.BoardMovers {
    public enum MovementOptions {
        Forward,
        Left,
        Right
    }
    
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SlipAndJump/Pattern", order = 1)]
    public class MovementPattern : ScriptableObject {
        public List<MovementOptions> moves;
    }
}