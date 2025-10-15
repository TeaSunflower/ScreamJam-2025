using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class LightManager : MonoBehaviour
{
    [SerializeField] List<GameObject> unlit;
    [SerializeField] GameObject player;
    private List<GameObject> lit = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// finds lantern closest to the player
    /// </summary>
    /// <returns>index of closest lantern</returns>
    public int GetNearestLantern()
    {
        float smallest = GetDistance(unlit[0]);
        int closest = 0;
        float dist = smallest;
        for (int i = 0; i < unlit.Count; i++)
        {
            dist = GetDistance(unlit[i]);
            if (dist < smallest)
            {
                smallest = dist;
                closest = i;
            }
        }
        return closest;
    }

    /// <summary>
    /// gets the distance between the player and lantern
    /// </summary>
    /// <param name="lantern"></param>
    /// <returns></returns>
    public float GetDistance(GameObject lantern)
    {
        float xdif = player.transform.position.x - lantern.transform.position.x;
        float ydif = player.transform.position.y - lantern.transform.position.y;
        return (Mathf.Sqrt((xdif * xdif) + (ydif * ydif)));
    }

    /// <summary>
    /// calls LightLantern
    /// </summary>
    /// <param name="context"></param>
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (unlit.Count == 0) { return; }
            LightLantern();
        }
    }

    /// <summary>
    /// lights the lantern
    /// </summary>
    public void LightLantern()
    {
        int nearest = GetNearestLantern();
        if (GetDistance(unlit[nearest]) > 10)
        {
            return;
        }
        lit.Add(unlit[nearest]);
        SpriteRenderer sr = unlit[nearest].GetComponent<SpriteRenderer>();
        sr.color = Color.red;
        unlit.RemoveAt(nearest);
    }
}
