using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransformBehaviour : MonoBehaviour
{
    [Header("Dock")]
    public GameObject dockPoint;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = dockPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = dockPoint.transform.position;
    }
}
