namespace EnemyAI
{
    public interface IState
    {
        public void React(Behaviour script);
        public IState ChangeState(Sightings sightings);
    }
}