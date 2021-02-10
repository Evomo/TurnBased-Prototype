using SlipAndJump.BoardMovers;
using SlipAndJump.Commands;
using UnityEngine;

namespace SlipAndJump.Board.Platform {
    public enum PlatformType {
        Normal,
        Ice,
        Fire
    }

    
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SlipAndJump/PlatformEffects", order = 1)]
    public  class PlatformEffects : ScriptableObject {
        public  ParticleSystem particles;
        public PlatformType platformType;
        [Range(1, 10)] public float spawnWeight;
        
        public void DoEffect(PlatformNode platform, PlayerMover mover) {
            switch (platformType) {
                case PlatformType.Ice:
                    TurnHandler.Instance.EnqueueCommand(new ActionCommand(mover.Move), TurnType.Player);
                    break;
                case PlatformType.Fire:
                    //TODO damage player
                    break;
                default:
                    break;
            }
        }
    }
}