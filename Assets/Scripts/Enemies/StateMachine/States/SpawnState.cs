using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnState : BaseState
{
    private void OnEnable()
    {
        //Play spawn animation
        IsCompleted = true;
    }
}
