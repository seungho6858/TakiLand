using System;
using System.Collections;
using System.Collections.Generic;
using EnumsNET;
using Mib;
using Mib.Data;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    public event Action<Formation, Formation> OnBattleStart;
    
    protected override void OnAwake()
    {
        
    }

    public void EndBattle()
    {
        
    }
}
