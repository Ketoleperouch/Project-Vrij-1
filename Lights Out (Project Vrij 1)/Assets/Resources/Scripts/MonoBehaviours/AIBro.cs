using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBro : MonoBehaviour {
    public Transform flame;

    string state = "patrol";
    public GameObject[] waypoints; // way points/amount of waypoints
    int currentWP = 0;

    public float rotSpeed; //rotation speed
    public float speed ; // move speed
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
            Vector3 direction = flame.position - this.transform.position;
            direction.y = 0;
            float angle = Vector3.Angle(direction, this.transform.forward);

            //Start patrolling cuz no flame around
            if (state == "patrol" && waypoints.Length > 0)
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
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
                this.transform.Translate(0, 0, Time.deltaTime * speed);// Z axis gets speed
            }

            //Start persuing/attacking
            if (Vector3.Distance(flame.position, this.transform.position) < 30 && (angle < 50 || state == "pursuing"))
            {
                state = "pursuing";
                //Turn towards the flame
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);

                if (direction.magnitude > 10) //If you are futher than 2 continue persuit.
                {
                    this.transform.Translate(0, 0, Time.deltaTime * speed);
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
                print("patroling");
                state = "patrol";
            }
        }

    }// End Update loop

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "flame")
        {
            canAttack = true;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "flame")
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
