using Opsive.Shared.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineController : MonoBehaviour
{
    [SerializeField] private Animator shrineAnimator;
    [SerializeField] private ShrineHealthController shrineHealthController;

    public Collider shrineCollider;

    public static ShrineController Instance { get; private set; }

    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        EventHandler.RegisterEvent("GameRestarted", OnGameRestarted);
    }

    public void OnShrineDestroyed()
    {
        //To Do: Play destruction animation
        shrineCollider.enabled = false;
        EventHandler.ExecuteEvent("GameEnded", GameEndedType.Lose);
    }

    private void OnGameRestarted()
    {
        //To Do: Set visual state to repaired form
        shrineCollider.enabled = true;
        shrineHealthController.InitializeHp();
    }

    private void OnDestroy()
    {
        Instance = null;
        EventHandler.UnregisterEvent("GameRestarted", OnGameRestarted);
    }
}
