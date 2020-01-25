using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    
    public static GameManager instance;
    public Animator screenFader;
    public Player player;
    
    public float maxDistance = 0;
    public bool paused = true;

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
    }

    void Start()
    {
        TrackManager.instance.InitializeTrack();
        player.Initialize();
        paused = false;
    }

    void Update()
    {
        //fading
        // if (player.transform.position.y < -0.5f)
        // {
        //     screenFader.SetTrigger("Fade");
        // }
    }
}
