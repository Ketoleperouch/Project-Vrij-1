using UnityEngine;

public class Pickup : MonoBehaviour {

    public float restoration = 20f;

    private void Update()
    {
        Collider[] inRange = Physics.OverlapSphere(transform.position, 1f);

        for (int i = 0; i < inRange.Length; i++)
        {
            FlameBehaviour flame = inRange[i].GetComponentInChildren<FlameBehaviour>();
            if (flame)
            {
                flame.vitality += restoration;
                flame.vitality = Mathf.Clamp(flame.vitality, 0, 100);
                Destroy(gameObject);
            }
        }
    }

}
