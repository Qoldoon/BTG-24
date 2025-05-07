using UnityEngine;

namespace EnemyAI
{
    public class FollowState : IState
    {
        private Sightings sightings;
        private bool done = false;

        public FollowState(Sightings sightings)
        {
            this.sightings = sightings;
        }
        public void React(EnemyBehaviour script)
        {
            var ally = sightings.AllySighting();
            script.FollowAlly(ally);
            done = Vector2.Distance(script.movementTarget.position, script.transform.position) < 1f;
        }

        public IState ChangeState(Sightings sightings)
        {
            var player = sightings.PlayerSighting();
            if (Sighting.IsRecent(player, 0.1f))
            {
                return new AttackState(sightings);
            }
            
            var enemy = sightings.AllySighting();
            if(done || !Sighting.IsRecent(enemy, 5f))
            {
                return new IdleState();
            }
            
            return this;
        }

        public override string ToString()
        {
            return $"Following done:{done}";
        }
    }
}