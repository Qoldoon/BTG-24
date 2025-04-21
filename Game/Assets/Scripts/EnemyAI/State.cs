using EnemyAI;

public class State
{
    public IState state;
    Sightings sightings;

    public State(Sightings sightings)
    {
        this.sightings = sightings;
        state = new IdleState();
    }
}