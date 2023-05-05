using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    public Sprite[] newSprite;
    public void ChangeSprite(){
        switch (GameFlow.GF.state){
            case 2: case 6:
                this.GetComponent<SpriteRenderer>().sprite = newSprite[0];
                break;
            case 4:
                this.GetComponent<SpriteRenderer>().sprite = newSprite[1];
                break;
        }
        
    }
}
