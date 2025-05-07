namespace EnemyAI
{
    public interface IState
    {
        public void React(EnemyBehaviour script);
        public IState ChangeState(Sightings sightings);
    }
}