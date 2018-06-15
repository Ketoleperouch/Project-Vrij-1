using UnityEngine;

public class Pickup : MonoBehaviour {

    public float restoration = 35f;
    public WeaponSway sway;
    public int pickedUpLayer;
    public LayerMask playerLayer;
    public AudioClip pickupSound;

    [SerializeField] private bool pickedUp = false;

    private Rigidbody rb;
    private Collider col;
    private Light lit;

    private void Start()
    {
        sway = GetComponent<WeaponSway>();
        sway.enabled = false;

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        lit = GetComponent<Light>();
    }

    private void Update()
    {
        if (pickedUp)
        {
            //lit.enabled = false;
            gameObject.layer = pickedUpLayer;
            rb.isKinematic = true;
            col.enabled = false;
            sway.enabled = true;
            Collider[] inRange = Physics.OverlapSphere(transform.position, 1f);

            for (int i = 0; i < inRange.Length; i++)
            {
                FlameBehaviour flame = inRange[i].GetComponentInChildren<FlameBehaviour>();
                if (flame)
                {
                    Instantiate(flame.healParticles, flame.transform);
                    flame.vitality += restoration;
                    flame.vitality = Mathf.Clamp(flame.vitality, 0, 100);
                    GetComponentInParent<PlayerController>().isHolding = false;
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            gameObject.layer = 0;
            lit.enabled = true;
            col.enabled = true;
            sway.enabled = false;

            Collider[] playerInRange = Physics.OverlapSphere(transform.position, 2f, playerLayer);

            if (playerInRange.Length > 0 && playerInRange[0].GetComponent<PlayerController>() && Input.GetMouseButtonDown(0))
            {
                PickUp(playerInRange[0].GetComponent<PlayerController>());
            }
        }
    }

    private void PickUp(PlayerController player)
    {
        if (player.isHolding)
        {
            return;
        }
        player.isHolding = true;
        pickedUp = true;
        AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        Instantiate(gameObject, player.hand.position, transform.rotation, player.hand);
        Destroy(gameObject);
    }

}
