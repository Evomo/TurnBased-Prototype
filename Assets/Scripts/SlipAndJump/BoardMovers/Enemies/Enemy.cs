namespace SlipAndJump.BoardMovers.Enemies {
    public class Enemy : BaseMover {
        public override void Start() {
            base.Start();
            board.onTurn.AddListener((() => this.Move()));
        }
    }
}