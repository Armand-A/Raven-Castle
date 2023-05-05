using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    Animator anm;

    Vector3 s = new Vector3 (0.02f, 0, 0);

    float time = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        anm = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shake(){
        anm.enabled = false;
        //this.transform.position += new Vector3 (1f, 0, 0);
        StartCoroutine(Motion());
    }

    IEnumerator Motion(){
        this.transform.position += s;
        yield return new WaitForSeconds(time);
        this.transform.position -= s;
        yield return new WaitForSeconds(time);
        this.transform.position += s;
        yield return new WaitForSeconds(time);
        this.transform.position -= s;
        yield return new WaitForSeconds(time);
        this.transform.position += s;
        yield return new WaitForSeconds(time);
        this.transform.position -= s;
        yield return new WaitForSeconds(time);
        anm.enabled = true;
        GameFlow.GF.preventClick = false;
    }
}
