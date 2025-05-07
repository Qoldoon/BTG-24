using System.Collections.Generic;
using UnityEngine;

namespace EnemyAI
{
    public class LookState : IState
    {
        private Sightings sightings;
        private float duration;
        Vector3 goDirection;
        Vector3 goPosition;

        public LookState(Sightings sightings)
        {
            this.sightings = sightings;
            var playerSighting = sightings.PlayerSighting();
            if (playerSighting == null)
            {
                duration = 8;
                return;
            }
            goDirection = playerSighting.Velocity;
            goPosition = playerSighting.Position;
        }
        public void React(EnemyBehaviour script)
        {
            if(duration < 8)
            {
                var list = sightings.WallSearch();
                var bigAngle = float.MaxValue;
                Sighting s = null;
                Vector3 vector;
                foreach (var wall in list)
                {
                    vector = wall.Target.transform.position - goPosition;
                    var angle = Vector2.Angle(vector, goDirection);
                    if (angle < bigAngle)
                    {
                        bigAngle = angle;
                        s = wall;
                    }
                }

                if (Vector2.Distance(script.transform.position, goPosition) < 1f)
                {
                    goPosition += goDirection;
                    var walldir = s.Target.transform.position - goPosition;
                    goDirection = (walldir + goDirection) * 0.5f;
                    duration++;
                }

                script.ToPosition(goPosition, goDirection);
            }
            else
            {
                script.LookAround();
                duration += Time.deltaTime;
            }
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
            if (Sighting.IsRecent(enemy, 0.1f) && enemy.Target.GetComponent<EnemyBehaviour>().IsAggro)
            {
                return new FollowState(sightings);
            }

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