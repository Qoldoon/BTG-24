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
            var player = sightings.PlayerSighting();
            script.Chase(player);

            if (Sighting.IsRecent(sightings.Sound(), 0.1f))
            {
                player.Position = sightings.Sound().Position;
            }
            
            done = Vector2.Distance(script.movementTarget.position, script.transform.position) < 0.1f;
        }

        public IState ChangeState(Sightings sightings)
        {
            var player = sightings.PlayerSighting();
            if (Sighting.IsRecent(player, 0.1f))
            {
                return new AttackState(sightings);
            }
            
            var sound = sightings.Sound();
            if (Sighting.IsRecent(sound, 0.1f))
                return new InvestigateState(sightings);
            
            if(done || !Sighting.IsRecent(player, 5f))
            {
                return new LookState(sightings);
            }

            return this;
        }
        
        public override string ToString()
        {
            return $"Chasing {sightings.PlayerSighting()}";
        }
    }
}