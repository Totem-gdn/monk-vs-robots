using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using TotemEntities;
using UnityEngine;

public class TotemManager : MonoBehaviour
{
    public static TotemManager Instance { get; private set; }

    public bool userAuthenticated;
    public TotemAvatar currentAvatar;
    public TotemSpear currentSpear;

    public List<TotemAvatar> currentUserAvatars = new List<TotemAvatar>();
    public List<TotemSpear> currentUserSpears = new List<TotemSpear>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
