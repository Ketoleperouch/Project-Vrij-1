using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float clampAngle = 80f;
    public float moveSpeed = 3f;
    public float jumpForce = 250f;
    public float sensitivity = 150f;
    public GameObject m_playerCam;
    public float crouchSpeed = 5;
    public float crouchColHeight = 1.2f;
    public float crouchColY = -0.85f;
    public Vector3 originOffset = Vector3.zero;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayers;

    private CapsuleCollider playerCol;
    private Vector3 move;
    private float originalColHeight;
    private float originalColY;
    private Rigidbody rb;
    private bool grounded = true;
    private Vector3 referenceDirectionsX;
    private Vector3 referenceDirectionsZ;
    private bool transformSet = false;
    private float xRot;
    private float x = 0;
    private float y = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerCol = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        originalColHeight = playerCol.height;
        originalColY = playerCol.center.y;
    }

    private void Update()
    {
        Move();
        VerticalLook();
        CheckGrounded();
        CheckInteractables();
    }

    private void Move()
    {
        if (grounded)
        {
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            x *= 0.9f;
            y *= 0.9f;
        }

        var rotation = Input.GetAxis("Mouse X") * sensitivity;

        move = new Vector3(x, 0, y).normalized;
        move *= moveSpeed;

        transform.Rotate(Vector3.up, rotation * Time.deltaTime);

        //Jump:
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(new Vector3(0, jumpForce * rb.mass, 0));
        }

        //Crouch:
        if (Input.GetKey(KeyCode.LeftControl))
        {
            m_playerCam.transform.localPosition = new Vector3(0, Mathf.Lerp(m_playerCam.transform.localPosition.y, -0.5f, Time.deltaTime * crouchSpeed), 0);
            playerCol.height = crouchColHeight;
            playerCol.center = new Vector3(playerCol.center.x, crouchColY, playerCol.center.z);
            move *= 0.5f;
        }
        else
        {
            m_playerCam.transform.localPosition = new Vector3(0, Mathf.Lerp(m_playerCam.transform.localPosition.y, 0f, Time.deltaTime * crouchSpeed), 0);
            playerCol.height = originalColHeight;
            playerCol.center = new Vector3(playerCol.center.x, originalColY, playerCol.center.z);
        }

        //Finalize Movement:
        if (grounded)
        {
            transformSet = false;
            referenceDirectionsZ = Vector3.zero;
            transform.Translate(new Vector3(move.x, grounded ? 0 : rb.velocity.y * Time.deltaTime, move.z) * Time.deltaTime);
        }
        else
        {
            //Create reference transform to store directions and simulate uncontrollable jump.
            if (!transformSet)
            {
                referenceDirectionsX = transform.right;
                referenceDirectionsZ = transform.forward;
                transformSet = true;
            }
            transform.position += (referenceDirectionsX * move.x) * Time.deltaTime;
            transform.position += (referenceDirectionsZ * move.z) * Time.deltaTime;
        }
    }

    private void VerticalLook()
    {
        var mouseX = (-Input.GetAxis("Mouse Y")) * sensitivity;
        xRot += mouseX * Time.deltaTime;

        Quaternion localRotation = Quaternion.Euler(xRot, 0, 0);
        Camera.main.transform.localRotation = localRotation;

        xRot = Mathf.Clamp(xRot, -clampAngle, clampAngle);
    }

    private void CheckGrounded()
    {
        grounded = Physics.Raycast(transform.position + originOffset, Vector3.down, groundCheckRadius, groundLayers);
    }

    private void CheckInteractables()
    {
        RaycastHit hit;
        Vector3 screenPoint = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(screenPoint, Camera.main.transform.forward, out hit, 2.5f))
        {
            if (hit.collider.isTrigger && hit.collider.CompareTag("Interactable"))
            {
                //Prompt Interaction UI element
                Debug.Log("Press E to interact!");
                if (Input.GetKeyDown(KeyCode.E))
                    hit.collider.GetComponent<InteractableBase>().Activate();
            }
        }
    }

}
