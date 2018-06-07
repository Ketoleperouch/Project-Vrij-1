using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour {

    public GameObject popUp1;
    public GameObject popUp2;
    public GameObject popUp3;
    public int TheTime = 5;
    

    // Use this for initialization
    void Start () {
        popUp1.SetActive(false);
        popUp2.SetActive(false);
        popUp3.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            ClosePopUp();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            OpenPopUp();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            ClosePopUp();
        }
    }

    void OpenPopUp()
    {
        popUp1.SetActive(true);
        popUp2.SetActive(true);
        popUp3.SetActive(true);
        StartCoroutine(Timer());
    }

    public void ClosePopUp()
    {
        Destroy(gameObject);
        popUp1.SetActive(false);
        popUp2.SetActive(false);
        popUp3.SetActive(false);
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(TheTime);
        ClosePopUp();
    }
}
