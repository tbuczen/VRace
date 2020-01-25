using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TrackManager : MonoBehaviour {

    public static TrackManager instance;

    public Segment prefab;
    private Transform player;

    //Position of upcoming segment
    Vector3 nextArcPos = Vector3.zero;
    Vector3 nextArcHeightOffset = Vector3.zero;
    float nextArcAngle = 0;
    float nextArcUVOffset = 0;
    float textureScale = 12;
    
    public Segment current;
    private Segment furthest;
    public float furthestDistance = 0;

    List<Segment> track;
    Transform trackHolder;
    Vector3 trackYOffset = Vector3.zero;

    float minRadius = 6;
    float maxRadius = 40;
    
    [Header("Track Segment Parameters")]
    public float segmentWidth = 10f;
    public float minAngle = 5;
    public float maxAngle = 70;
    //Slope should be a range; minus values lowers segments - plus values raise it 
    public float slope = -0.08f;

    float meshQuality = 1f;
    private float totalAngle = 0;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void InitializeTrack()
    {
        trackHolder = new GameObject("Track").transform;
        Debug.Log("track initialised");
        //List of segments
        track = new List<Segment>();
        while((nextArcPos.getFlat() - player.position.getFlat()).magnitude < 50)
        {
            AddSegment();
        }

        current = track[0];
    }

    void Update()
    {
        for(int i = 0; i < track.Count; i++)
        {
            if(track[i].distanceOffset + track[i].totalDistance < GameManager.instance.maxDistance - 2)
            {
                track[i].transform.SetParent(null);
                track[i].ResetArc();
                track[i].gameObject.SetActive(false);
            }
        }

        if((nextArcPos.getFlat() - player.position).magnitude < 35)
        {
            AddSegment();
        }
    }

    void AddSegment()
    {
        Segment newSegment = GetPooledArc();

        newSegment.transform.position = nextArcPos;
        newSegment.startAngle = nextArcAngle;
        newSegment.width = segmentWidth;
        newSegment.slope = slope;

        float randomRadius = Random.Range(minRadius, maxRadius);
        float randomAngle = Random.Range(minAngle, maxAngle);
        newSegment.radius = randomRadius;
        
        //5 -70
        if(Random.value > 0.5){
            totalAngle += randomAngle;
            newSegment.flipped = true;
        }else{
            totalAngle -= randomAngle;
            newSegment.flipped = false;
        }

        
        //clamp between -120 and 120
        if (totalAngle < -120)
        {
            totalAngle += randomAngle;
            newSegment.flipped = true;
        }
        
        if (totalAngle > 120)
        {
            totalAngle -= randomAngle;
            newSegment.flipped = false;
        }
        

        newSegment.angle = randomAngle;

        newSegment.Initialize(); // calculates the total length of the arc and identifies it's end position and center of rotation

        newSegment.distanceOffset = furthestDistance;
        newSegment.uvOffset = nextArcUVOffset;
        newSegment.textureScale = textureScale;

        furthestDistance += newSegment.totalDistance;
        nextArcHeightOffset.Set(0, furthestDistance * slope, 0);
        nextArcAngle = newSegment.endAngle;
        nextArcPos = newSegment.endPos;
        nextArcUVOffset += newSegment.totalDistance / textureScale - Mathf.Floor(newSegment.totalDistance / textureScale);
        if(nextArcUVOffset >= 1)
        {
            nextArcUVOffset = nextArcUVOffset - 1;
        }

        if (furthest != null)
        {
            furthest.next = newSegment; // each arc contains a reference to the next arc in the track
        }
        furthest = newSegment;

        newSegment.arcDivisions = Mathf.RoundToInt(newSegment.totalDistance / meshQuality); // adjust mesh divisions based on the length of the arc
        newSegment.GenerateMesh();

        track.Add(newSegment);
        trackHolder.position = Vector3.zero;
        newSegment.transform.SetParent(trackHolder);
        trackHolder.position = trackYOffset;

        newSegment.gameObject.SetActive(true);
    }

    Segment GetPooledArc()
    {
        for (int i = 0; i < track.Count; i++)
        {
            if (!track[i].gameObject.activeInHierarchy)
            {
                return track[i];
            }
        }

        // if none is found create a new object only if the pool has not exceeded its hard cap

        Segment newSegment = Instantiate(prefab) as Segment;
        return newSegment;
    }

    public void OffsetHeight(float offset) // the entire track actually moves up in the Y axis, instead of the player descending.  Keeps player from straying ever further from origin.
    {
        trackYOffset.Set(0, -offset, 0);
        trackHolder.position = trackYOffset;
    }

    public void IncrementArc()
    {
        Segment next = current.next;
        current = next;
    }
}
