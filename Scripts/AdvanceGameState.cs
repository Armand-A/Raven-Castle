using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceGameState : MonoBehaviour
{
    public void TriggerGameStart(){
        GameFlow.GF.GrantControl();
    }
}
