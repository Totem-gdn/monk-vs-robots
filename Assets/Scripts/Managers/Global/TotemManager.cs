using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using TotemEntities;
using UnityEngine;

public class TotemManager : MonoBehaviour
{
    public TotemMockDB TotemMockDB { get; private set; }
    public static TotemManager Instance { get; private set; }

    public bool userAuthenticated;
    public TotemAvatar currentAvatar;
    public TotemSpear currentSpear;

    public List<TotemAvatar> currentUserAvatars;
    public List<TotemSpear> currentUserSpears;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            TotemMockDB = new TotemMockDB();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
