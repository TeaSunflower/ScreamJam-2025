using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlaceSalt : MonoBehaviour
{
    [SerializeField] List<GameObject> saltSpots;
    [SerializeField] GameObject player;
    private List<GameObject> usedSpots = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    /// <param name="lantern"></param>
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
        if (GetDistance(saltSpots[nearest]) > 10)
        {
            return;
        }
        usedSpots.Add(saltSpots[nearest]);
        saltSpots[nearest].SetActive(true);
        saltSpots.RemoveAt(nearest);
    }

    /// <summary>
    /// places salt
    /// </summary>
    /// <param name="context"></param>
    public void UseSalt(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SetSaltBarrier();
        }
    }
}
