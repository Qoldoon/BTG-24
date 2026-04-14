using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EnemyAI
{
    public class LookState : IState
    {
        private Sightings sightings;
        private float duration;
        private bool _started;

        public LookState(Sightings sightings)
        {
            this.sightings = sightings;
        }

        public void React(EnemyBehaviour script)
        {
            duration += Time.deltaTime;

            if (!_started)
            {
                script.BeginTracking(sightings.PlayerSighting());
                _started = true;
            }

            script.Track(sightings.PlayerSighting());
        }




        public IState ChangeState(Sightings sightings)
        {
            var player = sightings.PlayerSighting();
            if (Sighting.IsRecent(player, 0.1f))
                return new AttackState(sightings);

            if (Sound.IsRecent(sightings.Listen(), 0.2f))
                return new InvestigateState(sightings);

            var enemy = sightings.AllySighting();
            if (Sighting.IsRecent(enemy, 0.1f) && enemy.Target.GetComponent<EnemyBehaviour>().IsAggro)
                return new FollowState(sightings);

            if (duration > 12f)
                return new IdleState();

            return this;
        }
        
        public override string ToString()
        {
            return $"Looking around";
        }
    }
}