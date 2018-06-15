using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    public Text logText;
    public string displayText = "Type your text here";
    public int displayTime;
    public bool destroyOnClose = false;

    public bool isDisplaying { get; set; }

    private void Start()
    {
        logText.text = "";
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E) && isDisplaying)
        {
            Close();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isDisplaying)
        {
            StartCoroutine(Popup());
        }
    }

    private void Close()
    {
        isDisplaying = false;
        logText.text = "";
        if (destroyOnClose)
        {
            Destroy(this);
        }
    }

    private IEnumerator Popup()
    {
        isDisplaying = true;
        logText.text = displayText;
        yield return new WaitForSeconds(displayTime);
        Close();
    }
}