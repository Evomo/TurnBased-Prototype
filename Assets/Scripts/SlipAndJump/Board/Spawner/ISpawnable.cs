using SlipAndJump.Board.Platform;

namespace SlipAndJump.Board.Spawner {
    public interface ISpawnable<T> {
        T Spawn(BoardNode spawnPosition);
    }
}