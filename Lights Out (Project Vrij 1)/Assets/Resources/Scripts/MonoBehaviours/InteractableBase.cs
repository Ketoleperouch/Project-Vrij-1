using UnityEngine;
using UnityEngine.Events;

public class InteractableBase : MonoBehaviour {

    public UnityEvent onActivate;
	
    public void Activate()
    {
        onActivate.Invoke();
    }

}
