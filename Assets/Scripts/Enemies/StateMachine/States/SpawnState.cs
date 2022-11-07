
using Opsive.Shared.Events;

public class SpawnState : BaseState
{
    private void OnEnable()
    {
        EventHandler.ExecuteEvent(stateMachine.gameObject, "Spawn");
        IsCompleted = true;
    }
}
