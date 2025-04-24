using UnityEngine;

namespace EnemyAI
{
    public class InvestigateState : IState
    {
        private Sightings sightings;
        private bool done = false;

        public InvestigateState(Sightings sightings)
        {
            this.sightings = sightings;
        }
        public void React(Behaviour script)
        {
            script.Investigate(sightings.Sound());
            done = Vector2.Distance(script.movementTarget.position, script.transform.position) < 0.6f;
        }

        public IState ChangeState(Sightings sightings)
        {
            var player = sightings.PlayerSighting();
            if (Sighting.IsRecent(player, 0.1f))
            {
                return new AttackState(sightings);
            }

            if (done)
                return new LookState(sightings);

            return this;
        }
        
        public override string ToString()
        {
            return $"Investigation done:{done}";
        }
    }
}