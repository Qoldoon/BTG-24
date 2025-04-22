namespace EnemyAI
{
    public class AttackState : IState
    {
        private Sightings sightings;

        public AttackState(Sightings sightings)
        {
            this.sightings = sightings;
        }
        public void React(Behaviour script)
        {
            script.AttackPlayer(sightings.PlayerSighting());
        }
        public IState ChangeState(Sightings sightings)
        {
            var player = sightings.PlayerSighting();
            if (!Sighting.IsRecent(player, 0.1f))
            {
                return new ChaseState(sightings);
            }

            return this;
        }
        
        public override string ToString()
        {
            return $"Attacking player:{sightings.PlayerSighting()}";
        }
    }
}