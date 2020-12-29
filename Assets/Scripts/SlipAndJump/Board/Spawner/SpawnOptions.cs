using UnityEngine;

namespace SlipAndJump.Board.Spawner {
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SlipAndJump/SpawnOptions", order = 1)]

    public class SpawnOptions : ScriptableObject{
        public AnimationCurve spawnCurve;
        [Range(1, 10)] public int maxEntitiesSpawned;
        [Range(10, 100)] public int maxTurns;
        [Range(1, 10)] public int spawnModulo;


        public int AmountToSpawn(int currentTurn) {
            float normalizedTurn = Mathf.Clamp01((float) currentTurn / (float) maxTurns);

            return Mathf.Max(1, (int) (spawnCurve.Evaluate(normalizedTurn) * maxEntitiesSpawned));
        }
    }
}