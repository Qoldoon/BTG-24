using System.Linq;

namespace EnemyAI
{
    public class IdleState : IState
    {
        public void React(Behaviour script)
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
            if (Sighting.IsRecent(enemy, 0.1f))
            {
                return this; //FollowAlly
            }
            
            var sound = sightings.Sound();
            if (sound != null)
                return this; //Investigate
            
            return this;
        }
    }
}