namespace SlipAndJump.Board.Spawner {
    public interface ISpawnable<T> {
        public T Spawn(BoardNode spawnPosition);
    }
}