using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;

    public abstract void Init();

    public abstract void ManagerTask();
}
