using UnityEngine;

namespace EnemyAI
{
    public class ChaseState : IState
    {
        private Sightings sightings;
        private bool done = false;

        public ChaseState(Sightings sightings)
        {
            this.sightings = sightings;
        }
        public void React(Behaviour script)
        {
            script.Follow(sightings.PlayerSighting());
            done = Vector2.Distance(script.movementTarget.position, script.transform.position) < 0.1f;
        }

        public IState ChangeState(Sightings sightings)
        {
            var player = sightings.PlayerSighting();
            if (Sighting.IsRecent(player, 0.1f))
            {
                return new AttackState(sightings);
            }
            if(done || !Sighting.IsRecent(player, 5f))
            {
                return new IdleState();//look state
            }

            return this;
        }
    }
}