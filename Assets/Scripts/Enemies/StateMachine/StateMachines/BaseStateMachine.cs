using Opsive.Shared.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventHandler = Opsive.Shared.Events.EventHandler;

public class BaseStateMachine : MonoBehaviour
{
    public EnemyHpController hpController;
    public Collider mainCollider;

    [NonSerialized] public Transform currentTargetTransform;
    [NonSerialized] public Collider currentTargetCollider;
    [NonSerialized] public bool isStateMachineRunning = false;
    [NonSerialized] public RobotsSpawner spawnerRoot;

    [SerializeField] private List<BaseState> avaliableStates;
    [SerializeField] private BaseState startState;
    [SerializeField] private List<Transform> checkpointsPath;
    [SerializeField] private SoundsManager robotSoundsManager;

    private List<BaseTransition> currentStateTransitions;
    private Animator robotAnimator;

    public BaseState CurrentState { get; private set; }
    public int CurrentCheckpointIndex { get; private set; }
    public Transform CurrentCheckpoint { get; private set; }

    public List<Transform> CheckpointsPath
    {
        get
        {
            return checkpointsPath;
        }
    }

    public SoundsManager RobotSoundsManager
    {
        get
        {
            return robotSoundsManager;
        }
    }

    private void Awake()
    {
        TryGetComponent(out robotAnimator);
        InitializeStateTransitions();
        ResetCheckpoints();
        EventHandler.RegisterEvent(gameObject, "Death", OnDeath);
        EventHandler.RegisterEvent<GameEndedType>("GameEnded", OnGameEnded);
    }

    private void Update()
    {
        if (isStateMachineRunning)
        {
            if (CurrentState.IsCompleted)
            {
                CheckTransitions();
            }
        }
    }

    private void ActivateState(BaseState stateToActivate)
    {
        StopAllCoroutines();
        CurrentState = stateToActivate;
        CurrentState.enabled = true;
        currentStateTransitions = CurrentState.stateTransitions;
    }

    private void CheckTransitions()
    {
        foreach (var transition in currentStateTransitions)
        {
            if (transition.IsInitializedSuccessfully)
            {
                var (isMetRequierements, newTransitionState) = transition.CheckTransitionRequirements();
                if (isMetRequierements)
                {
                    SetNewState(newTransitionState);
                    break;
                }
            }
        }
    }

    private void SetNewState(BaseState newState)
    {
        CurrentState.enabled = false;
        ActivateState(newState);
    }

    private void InitializeStateTransitions()
    {
        foreach(var state in avaliableStates)
        {
            foreach(var transition in state.stateTransitions)
            {
                transition.InitializeTransition(this);
            }
        }
    }

    private void OnDeath()
    {
        var dieState = avaliableStates.Find(state => state is DieState);

        if (dieState != null)
        {
            SetNewState(dieState);
        }
        else
        {
            Debug.LogError($"DieState not set on {name} enemy!");
            CurrentState.enabled = false;
            isStateMachineRunning = false;
        }
    }

    private void OnGameEnded(GameEndedType gameEndedType)
    {
        ResetStateMachine();
        ResetCheckpoints();

        if (gameEndedType == GameEndedType.Lose && robotAnimator != null)
        {
            robotAnimator.SetTrigger(Constants.VICTORY_ANIMATION_TRIGGER);
        }
    }

    private void ResetCheckpoints()
    {
        CurrentCheckpointIndex = 0;
        if (checkpointsPath.Count > 0)
        {
            CurrentCheckpoint = checkpointsPath[0];
        }
    }

    private void OnDestroy()
    {
        EventHandler.UnregisterEvent(gameObject, "Death", OnDeath);
        EventHandler.UnregisterEvent<GameEndedType>("GameEnded", OnGameEnded);
    }

    public void ActivateStartState()
    { 
        robotAnimator.enabled = false;
        robotAnimator.enabled = true;
        ActivateState(startState);
        isStateMachineRunning = true;
    }

    public void ResetStateMachine()
    {
        isStateMachineRunning = false;
        CurrentState.enabled = false;
        currentTargetTransform = null;
        currentTargetCollider = null;
        CurrentState = startState;
    }

    public void CheckpointReached()
    {
        CurrentCheckpointIndex++;
        if (CurrentCheckpointIndex < checkpointsPath.Count && checkpointsPath.Count > 0)
        {
            CurrentCheckpoint = checkpointsPath[CurrentCheckpointIndex];
        }
    }
}
