using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public Transform flame;

    public enum State { Patrol, Pursuing};
    public State state;

    public GameObject[] waypoints; // way points/amount of waypoints
    int currentWP = 0;

    public float rotSpeed; //rotation speed
    public float speed; // move speed
    float accuracyWP = 10.0f; // accuracy when following way points

    public int attackTrigger;
    public bool canAttack;
    public bool isDead;


    // Use this for initialization
    void Start()
    {
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead == false)
        {
            Vector3 direction = flame.position - transform.position;
            direction.y = 0;
            float angle = Vector3.Angle(direction, transform.forward);

            //Start patrolling cuz no flame around
            if (state == State.Patrol && waypoints.Length > 0)
            {
                if (Vector3.Distance(waypoints[currentWP].transform.position, transform.position) < accuracyWP)
                {
                    //Use this if you want them to follow each WP randomly
                    currentWP = Random.Range(0, waypoints.Length);

                    //Use this if you want them to follow each WP orderly
                    if (currentWP >= waypoints.Length)
                    {
                        currentWP = 0;
                    }
                }//Go to WP direction
                direction = waypoints[currentWP].transform.position - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
                transform.Translate(0, 0, Time.deltaTime * speed);// Z axis gets speed
            }

            //Start persuing/attacking
            if (Vector3.Distance(flame.position, transform.position) < 30 && (angle < 50 || state == State.Pursuing))
            {
                state = State.Pursuing;
                //Turn towards the flame
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);

                if (direction.magnitude > 10) //If you are futher than 2 continue persuit.
                {
                    transform.Translate(0, 0, Time.deltaTime * speed);
                    print("Attackerino");
                }
                else
                { //Else Attack
                    if (canAttack == true)
                    {
                        print("Attacking");
                    }

                }

            }
            else
            { // If there is no persuit than continue walking
                print("Patrolling");
                state = State.Pursuing;
            }
        }

    }// End Update loop

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Flame"))
        {
            canAttack = true;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Flame"))
        {
            canAttack = false;
        }
    }


    public void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }
}