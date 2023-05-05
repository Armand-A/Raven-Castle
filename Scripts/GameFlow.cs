using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public static GameFlow GF;

    public AudioSource MergeSfx, SwapSfx;

    public GameObject GemGrid, WitchContainer, CTA, SuperSfx;

    GameObject trig1, trig2, trig3;

    TapHandler trig1_TH, trig2_TH, trig3_TH;

    //Vector3[] origPos = new Vector3[17];
    Animator[] A = new Animator[5];
    Animator[] B = new Animator[5];
    Animator[] C = new Animator[5];
    Animator[] D = new Animator[5];

    //Animator targetGem = null; 

    //Vector3 targetPos;

    public int state = 0;

    public int magicCount = 0;
    public bool canClickCTA, preventClick = false;

    void Start()
    {
        GF = this; 

        trig1 = GemGrid.transform.GetChild(16).gameObject;
        trig1_TH = trig1.GetComponent<TapHandler>();
        trig2 = GemGrid.transform.GetChild(17).gameObject;
        trig2_TH = trig2.GetComponent<TapHandler>();
        trig3 = GemGrid.transform.GetChild(18).gameObject;
        trig3_TH = trig3.GetComponent<TapHandler>();
    }

    void SetupGrid(){
        int row = 0, i = 1;
        for (int childIndex = 0; childIndex < 16; childIndex++){
            GameObject gem = GemGrid.transform.GetChild(childIndex).gameObject;
            Animator gem_ANM = gem.GetComponent<Animator>();
            //origPos[childIndex+1] = gem.transform.position;
            //Debug.Log(row + " " + i + " " +childIndex);
            switch(row){
                case 0:
                    A[i] = gem_ANM;
                    break;
                case 1:
                    B[i] = gem_ANM;
                    break;
                case 2:
                    C[i] = gem_ANM;
                    break;
                case 3:
                    D[i] = gem_ANM;
                    break;
                default:
                    break;
            }
            
            if (i<4) i++;
            else {
                row++;
                i = 1;
            } 

        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !preventClick){
            Vector2 c = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (canClickCTA){
                Luna.Unity.Playable.InstallFullGame();
            } else if (state == 1 && trig1_TH.checkTouch(c)){
                state = 2;
                trig1.SetActive(false);
                B[3].Play("Swap", 0, 0);
                B[4].Play("Swap", 0, 0); 
                StartCoroutine(FirstMatch());
            } else if (state == 3 && trig2_TH.checkTouch(c)){
                state = 4;
                trig2.SetActive(false);
                B[4].Play("SecondSwap", 0, 0);
                C[3].Play("Swap", 0, 0);
                StartCoroutine(SecondMatch());
            } else if (state == 5 && trig3_TH.checkTouch(c)){
                state = 6;
                trig3.SetActive(false);
                A[2].Play("Swap", 0, 0);
                B[2].Play("Swap", 0, 0);
                StartCoroutine(ThirdMatch());
            } else if (state == 1 || state == 3 || state == 5){
                int row = 0, i = 1;
                Animator clickedGem = null;
                for (int gemIndex = 0; gemIndex < 16; gemIndex++){
                    switch(row){
                        case 0:
                            if (A[i].gameObject.GetComponent<TapHandler>().checkTouch(c)){
                                clickedGem = A[i];
                            }
                            break;
                        case 1:
                            if (B[i].gameObject.GetComponent<TapHandler>().checkTouch(c)){
                                clickedGem = B[i];
                            }
                            break;
                        case 2:
                            if (C[i].gameObject.GetComponent<TapHandler>().checkTouch(c)){
                                clickedGem = C[i];
                            }
                            break;
                        case 3:
                            if (D[i].gameObject.GetComponent<TapHandler>().checkTouch(c)){
                                clickedGem = D[i];
                            }
                            break;
                        default:
                            break;
                    }

                    if (clickedGem != null){
                        shakeGem(clickedGem);
                        break;
                    }
                    
                    if (i<4) i++;
                    else {
                        row++;
                        i = 1;
                    } 
                }
            }
        }
    }

    public void GrantControl(){
        SetupGrid();
        WitchContainer.transform.GetChild(0).gameObject.SetActive(false);
        B[3].gameObject.GetComponent<BoxCollider2D>().enabled = false;
        B[4].gameObject.GetComponent<BoxCollider2D>().enabled = false;
        state = 1;
    }

    void shakeGem(Animator clickedGem){
        preventClick = true;
        clickedGem.gameObject.GetComponent<Gem>().Shake();
        SwapSfx.Play();
        //gem.Play("WrongTouch");
    }


    IEnumerator FirstMatch(){
        yield return new WaitForSeconds(0.5f);
        B[1].Play("Combine", 0, 0);
        B[4].Play("Combine", 0, 0);
        yield return new WaitForSeconds(0.3f);
        B[1].Play("Hide", 0, 0);
        B[2].Play("Hide", 0, 0);
        B[4].Play("Hide", 0, 0);
        GemGrid.transform.GetChild(19).gameObject.SetActive(true);
        MergeSfx.Play();
        yield return new WaitForSeconds(0.2f);
        this.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        B[1].Play("PrepRespawn", 0, 0);
        B[4].Play("PrepRespawn", 0, 0);
        yield return new WaitForSeconds(0.8f);
        //Fallback
        /*
        B[1].gameObject.GetComponent<UpdateSprite>().ChangeSprite();
        B[2].gameObject.GetComponent<UpdateSprite>().ChangeSprite();
        B[4].gameObject.GetComponent<UpdateSprite>().ChangeSprite();
        */
        B[1].Play("Respawn", 0, 0);
        B[2].Play("Respawn", 0, 0);
        B[4].Play("Respawn", 0, 0);
        yield return new WaitForSeconds(0.5f);
        B[3].gameObject.GetComponent<BoxCollider2D>().enabled = true;
        C[3].gameObject.GetComponent<BoxCollider2D>().enabled = false;
        state = 3;
        trig2.SetActive(true);
    }

    IEnumerator SecondMatch(){
        yield return new WaitForSeconds(0.5f);
        B[1].Play("Combine", 0, 0);
        C[3].Play("Combine", 0, 0);
        yield return new WaitForSeconds(0.3f);
        B[1].Play("Hide", 0, 0);
        B[2].Play("Hide", 0, 0);
        C[3].Play("Hide", 0, 0);
        GemGrid.transform.GetChild(20).gameObject.SetActive(true);
        MergeSfx.Play();
        yield return new WaitForSeconds(0.2f);
        this.gameObject.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        B[1].Play("PrepRespawn", 0, 0);
        C[3].Play("PrepRespawn", 0, 0);
        yield return new WaitForSeconds(0.8f);
        //Fallback
        /*
        B[1].gameObject.GetComponent<UpdateSprite>().ChangeSprite();
        B[2].gameObject.GetComponent<UpdateSprite>().ChangeSprite();
        C[3].gameObject.GetComponent<UpdateSprite>().ChangeSprite();
        */
        B[1].Play("Respawn", 0, 0);
        B[2].Play("Respawn", 0, 0);
        C[3].Play("Respawn", 0, 0);
        yield return new WaitForSeconds(0.5f);
        B[4].gameObject.GetComponent<BoxCollider2D>().enabled = true;
        C[3].gameObject.GetComponent<BoxCollider2D>().enabled = true;
        A[2].gameObject.GetComponent<BoxCollider2D>().enabled = false;
        B[2].gameObject.GetComponent<BoxCollider2D>().enabled = false;
        state = 5;
        trig3.SetActive(true);
    }

    IEnumerator ThirdMatch(){
        yield return new WaitForSeconds(0.5f);
        A[2].Play("Combine", 0, 0);
        B[3].Play("Combine", 0, 0);
        yield return new WaitForSeconds(0.3f);
        A[2].Play("Hide", 0, 0);
        B[3].Play("Hide", 0, 0);
        C[3].Play("Hide", 0, 0);
        GemGrid.transform.GetChild(21).gameObject.SetActive(true);
        MergeSfx.Play();
        yield return new WaitForSeconds(0.2f);
        this.gameObject.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
        A[2].Play("PrepRespawn", 0, 0);
        B[3].Play("PrepRespawn", 0, 0);
        C[3].Play("PrepRespawn", 0, 0);
        yield return new WaitForSeconds(0.8f);
        //Fallback 
        /*
        A[2].gameObject.GetComponent<UpdateSprite>().ChangeSprite();
        B[3].gameObject.GetComponent<UpdateSprite>().ChangeSprite();
        C[3].gameObject.GetComponent<UpdateSprite>().ChangeSprite();
        */
        A[2].Play("Respawn", 0, 0);
        B[3].Play("Respawn", 0, 0);
        C[3].Play("Respawn", 0, 0);
        yield return new WaitForSeconds(0.75f);
        state = 7;
        A[2].Play("Hide", 0, 0);
        A[4].Play("Hide", 0, 0);
        B[1].Play("Hide", 0, 0);
        B[3].Play("Hide", 0, 0);
        C[3].Play("Hide", 0, 0);
        C[4].Play("Hide", 0, 0);
        GemGrid.transform.GetChild(22).gameObject.SetActive(true);
        MergeSfx.Play();

        yield return new WaitForSeconds(0.03f);
        SuperSfx.SetActive(true);
        yield return new WaitForSeconds(0.03f);
        SuperSfx.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.03f);
        SuperSfx.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.03f);
        SuperSfx.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);

        yield return new WaitForSeconds(0.2f);
        this.gameObject.transform.GetChild(3).GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        GemGrid.GetComponent<Animator>().Play("GridExit", 0, 0);
        yield return new WaitForSeconds(0.25f);
        WitchContainer.GetComponent<Animator>().Play("WitchMovePartial", 0, 0);
        WitchContainer.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(2.55f);
        WitchContainer.transform.GetChild(1).gameObject.SetActive(false);
        CTA.SetActive(true);
        WitchContainer.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 15;
        WitchContainer.transform.GetChild(0).gameObject.SetActive(true);
        preventClick = false;
        canClickCTA = true;
    }

    public void DisableObjects(){
        switch (state){
            case 2:
                this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                break;
            case 4:
                this.gameObject.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
                break;
            case 6:
                this.gameObject.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                break;
            case 7:
                this.gameObject.transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
}
