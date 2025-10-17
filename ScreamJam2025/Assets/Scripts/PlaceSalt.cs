using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceSalt : MonoBehaviour
{
    [SerializeField] List<GameObject> saltSpots;
    [SerializeField] public GameObject player;
    [SerializeField] GameObject pickup;
    [SerializeField] int totalSalt;
    private List<ShowSpot> spotScripts = new List<ShowSpot>();
    private List<ShowSpot> placedScripts = new List<ShowSpot>();
    private List<GameObject> placedSpots = new List<GameObject>();
    public int heldSalt = 0;
    public int spawnedSalt = 0;
    [SerializeField] float spawnCooldown;
    private float timer;
    public List<Vector2> spawnLocations = new List<Vector2>();
    private List<GameObject> pickups = new List<GameObject>();
    private List<PickupItem> itemScripts = new List<PickupItem>();
    public List<int> inactives = new List<int>();

    private static PlaceSalt instance;
    public static PlaceSalt Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //sets the script for each spot
        foreach (GameObject spot in saltSpots)
        {
            ShowSpot script = spot.GetComponent<ShowSpot>();
            script.StartDisabled();
            spotScripts.Add(script);
        }

        //instantiates the three salt pickups that will be moved around
        for (int i = 0; i < totalSalt; i++)
        {
            GameObject saltPickup = Instantiate(pickup);
            pickups.Add(saltPickup);
            itemScripts.Add(saltPickup.GetComponent<PickupItem>());
            inactives.Add(i);
        }

        spawnLocations.Add(new Vector2(27.5f, 17.5f));
        spawnLocations.Add(new Vector2(-28.5f, 16.5f));
        spawnLocations.Add(new Vector2(-27.5f, -18.5f));
        spawnLocations.Add(new Vector2(28.5f, -19.5f));

        timer = spawnCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (heldSalt + spawnedSalt < totalSalt)
        {
            if (timer >= spawnCooldown)
            {
                int index = inactives[0];
                inactives.RemoveAt(0);
                int spawnIndex = Random.Range(0, spawnLocations.Count);
                itemScripts[index].Respawn(index, spawnLocations[spawnIndex]);
                spawnLocations.RemoveAt(spawnIndex);
                timer = 0f;
                spawnedSalt++;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// finds the nearest salt spot to the player
    /// </summary>
    /// <returns>index of closest salt spot</returns>
    public int GetNearestSpot()
    {
        float smallest = GetDistance(saltSpots[0]);
        int closest = 0;
        float dist = smallest;
        for (int i = 0; i < saltSpots.Count; i++)
        {
            dist = GetDistance(saltSpots[i]);
            if (dist < smallest)
            {
                smallest = dist;
                closest = i;
            }
        }
        return closest;
    }

    /// <summary>
    /// gets the distance between the player and the salt spot
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public float GetDistance(GameObject line)
    {
        float xdif = player.transform.position.x - line.transform.position.x;
        float ydif = player.transform.position.y - line.transform.position.y;
        return (Mathf.Sqrt((xdif * xdif) + (ydif * ydif)));
    }

    /// <summary>
    /// places salt at the nearest spot
    /// </summary>
    public void SetSaltBarrier()
    {
        int nearest = GetNearestSpot();
        if (GetDistance(saltSpots[nearest]) > 2)
        {
            return;
        }
        heldSalt--;
        spotScripts[nearest].SetPlaced();
        placedSpots.Add(saltSpots[nearest]);
        placedScripts.Add(spotScripts[nearest]);
        spotScripts.RemoveAt(nearest);
        saltSpots.RemoveAt(nearest);
    }

    /// <summary>
    /// places salt
    /// </summary>
    /// <param name="context"></param>
    public void UseSalt(InputAction.CallbackContext context)
    {
        if (context.performed && heldSalt > 0)
        {
            SetSaltBarrier();
        }
    }
}
