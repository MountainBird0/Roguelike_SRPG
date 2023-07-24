using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapUIManager : MonoBehaviour
{
    public static BattleMapUIManager instance;

    public DeployUIController deployUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning($"{GetType()} - Destory");
            Destroy(gameObject);
        }
    }





}
