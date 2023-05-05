using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableChild : MonoBehaviour
{
    public void MagicTrigger(){
        //GameFlow.GF.magicCount++;
        //if (GameFlow.GF.magicCount <= 2 || GameFlow.GF.magicCount == 5 || GameFlow.GF.magicCount == 8){
            //Debug.Log("trigger");
        GameFlow.GF.DisableObjects();
        for (int x = 0; x < this.gameObject.transform.childCount; x++){
            this.gameObject.transform.GetChild(x).gameObject.SetActive(true);
        }
        //}
    }
}
