using UnityEngine;
using System.Collections;
using Valve.VR;

public class Player : MonoBehaviour
{

    private TrackManager trackManagerInstance;
    Segment current;
    Vector3 movement;

    Quaternion steering;
    Vector3 velocity = Vector3.zero;
    Vector3 gravity = Vector3.zero;
    float fallSpeed = 0;

    float distance = 0;
    public float groundHeight;
    public bool onGround = true;

    float turnGoal;
    float turnRate = 0;
    public float maxTurnRate;
    public float turnAcceleration;

    float speed = 0;
    float minSpeed = 2f;
    public float maxSpeed;
    public float acceleration;

    float h, v;

    public void Initialize()
    {
        current = TrackManager.instance.current;
        transform.position = current.ArcToWorld(new ArcPoint(20, current.radius)).getFlat();
    }

    void Update()
    {
        
        if (!GameManager.instance.paused) {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
            bool stateRH = true; 
            bool stateLH = true;
            // bool stateLH = SteamVR_Actions.vRace_Accelerate[SteamVR_Input_Sources.LeftHand].state;
            // bool stateRH = SteamVR_Actions.vRace_Accelerate[SteamVR_Input_Sources.RightHand].state;
            if (stateLH || stateRH)
            {
                // Debug.Log(v);
            }
            Move(h, v);

            CheckBounds();
            UpdateDistance();
        }
    }

    void Move(float h, float v)
    {
        turnGoal = h * maxTurnRate;
        if(Mathf.Abs(turnGoal) < 0.2f)
        {
            turnGoal = 0;
        }

        if (onGround)
        {
            if (turnRate < turnGoal)
            {
                turnRate += turnAcceleration;
            }
            else if (turnRate > turnGoal)
            {
                turnRate -= turnAcceleration;
            }
            if (turnGoal == 0)
            {
                turnRate /= 1.5f;
            }

            turnRate = Mathf.Clamp(turnRate, -maxTurnRate, maxTurnRate);

            steering = Quaternion.AngleAxis(turnRate, Vector3.up);
        }

        if(v > 0) {
            if (speed <= maxSpeed)
            {
                speed += v * acceleration;
            }
        } else if (speed > minSpeed) {
            speed -= acceleration/2;
            // if(Mathf.Abs(speed) < minSpeed + 0.05f)
            // {
            //     speed = minSpeed;
            // }
        }

        velocity = (transform.rotation * Vector3.forward).normalized * speed;
        if (!onGround)
        {
            fallSpeed -= 0.04f;
            gravity.Set(0, fallSpeed, 0);
            velocity = velocity + gravity;
            transform.rotation = transform.rotation * Quaternion.AngleAxis(0.6f, Vector3.right);
        }

        transform.rotation = transform.rotation * steering;
        transform.position = transform.position + velocity * Time.deltaTime;
    }

    void CheckBounds()
    {
        if (!current.ContainsPoint(transform.position, 0.05f))
        {
            if (current.next != null && current.next.ContainsPoint(transform.position, 0.05f)){
                TrackManager.instance.IncrementArc();
                current = TrackManager.instance.current;
            }else{
                onGround = false;
            }
        }
    }

    void UpdateDistance()
    {
        if (!onGround)
        {
            return;
        }
        distance = current.GetDistance(current.WorldToArc(transform.position).t);
        groundHeight = distance * TrackManager.instance.slope;
        TrackManager.instance.OffsetHeight(groundHeight);
    }
}