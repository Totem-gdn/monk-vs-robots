using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using TotemEntities;
using UnityEngine;

public class TotemManager : MonoBehaviour
{
    public TotemMockDB TotemMockDB { get; private set; }
    public static TotemManager Instance { get; private set; }

    public TotemUser currentUser;
    public TotemAvatar currentAvatar;
    public TotemSpear currentSpear;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            TotemMockDB = new TotemMockDB();
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetCurrentUser(string userName)
    {
        currentUser = TotemMockDB.UsersDB.GetUser(userName);
    }
}
