using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextAnimSequence : MonoBehaviour
{
    public GameObject obj;
    public void TriggerNextAnim(){
        obj.SetActive(true);
    }
}
