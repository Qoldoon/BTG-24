using System.Linq;

namespace EnemyAI
{
    public class IdleState : IState
    {
        public void React(EnemyBehaviour script)
        {

        }

        public IState ChangeState(Sightings sightings)
        {
            var player = sightings.PlayerSighting();
            if (Sighting.IsRecent(player, 0.1f))
            {
                return new AttackState(sightings);
            }

            var enemy = sightings.AllySighting();
            if (Sighting.IsRecent(enemy, 0.1f) && enemy.Target.GetComponent<EnemyBehaviour>().IsAggro)
            {
                return new FollowState(sightings);
            }
            
            var sound = sightings.Listen();
            if (Sound.IsRecent(sound, 0.1f))
                return new InvestigateState(sightings);
            
            return this;
        }

        public override string ToString()
        {
            return "Idle";
        }
    }
}