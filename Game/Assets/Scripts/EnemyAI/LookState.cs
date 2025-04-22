namespace EnemyAI
{
    public class LookState : IState
    {
        public void React(Behaviour script)
        {
            script.LookAround();
        }

        public IState ChangeState(Sightings sightings)
        {
            var player = sightings.PlayerSighting();
            if (Sighting.IsRecent(player, 0.1f))
            {
                return new AttackState(sightings);
            }

            var sound = sightings.Sound();
            if (Sighting.IsRecent(sound, 0.2f))
                return new InvestigateState(sightings);
            
            var enemy = sightings.AllySighting();
            if (Sighting.IsRecent(enemy, 0.1f) && enemy.Target.GetComponent<Behaviour>().IsAggro)
            {
                return new FollowState(sightings);
            }

            if (!Sighting.IsRecent(player, 4f))
                return new IdleState();
            
            return this;
        }
        
        public override string ToString()
        {
            return $"Looking around";
        }
    }
}