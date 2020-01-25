using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverEffect : MonoBehaviour
{

    public float horizontalSpeed;
    public float verticalSpeed = 2;
    
    public float horizontalAmplitude = 0;
    public float verticalAmplitude = 0.1f;

    public float stabilityPower;
    private Vector3 tmpPosition;
    private float random;

    // Start is called before the first frame update
    private void Start()
    {
        tmpPosition = transform.localPosition;
        random = Random.Range(-1.0f, 1.0f);

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        var time = Time.realtimeSinceStartup + random;
//        Vector3 cameraRelative = vehicleTransform.InverseTransformPoint(transform.position);
        tmpPosition.x = transform.localPosition.x + Mathf.Sin(time * horizontalSpeed) * horizontalAmplitude;
        tmpPosition.y =  Mathf.Sin(time * verticalSpeed) * verticalAmplitude;
        transform.localPosition = tmpPosition;
    }
}
