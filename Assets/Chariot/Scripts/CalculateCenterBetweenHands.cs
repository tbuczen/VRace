using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateCenterBetweenHands : MonoBehaviour
{

    public GameObject leftHand;
    public GameObject rightHand;

    public float gravity = 0.04f;
    // Update is called once per frame
    void Update()
    {
        Vector3 p =  LerpByDistance(leftHand.transform.position,rightHand.transform.position,1);
        p.y = p.y - gravity;
        this.transform.position = p;
    }
    
    private Vector3 LerpByDistance(Vector3 A, Vector3 B, float x)
    {
        return x * Vector3.Normalize(B - A) + A;
    }
}
