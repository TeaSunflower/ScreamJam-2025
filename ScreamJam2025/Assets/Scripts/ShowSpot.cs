using UnityEngine;

public class ShowSpot : MonoBehaviour
{
    public GameObject player;
    private bool highlighted = false;
    public bool placed = false;
    private SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!placed)
        {
            float dist = GetDistance();
            Debug.Log(dist);
            if (!highlighted && dist <= 2)
            {
                highlighted = true;
                sr.enabled = true;
                Debug.Log("highlighted");
            }
            else if (highlighted && dist > 2)
            {
                highlighted = false;
                sr.enabled = false;
            }
        }
    }

    /// <summary>
    /// gets the distance between the player and the salt spot
    /// </summary>
    /// <returns></returns>
    public float GetDistance()
    {
        float xdif = player.transform.position.x - transform.position.x;
        float ydif = player.transform.position.y - transform.position.y;
        return (Mathf.Sqrt((xdif * xdif) + (ydif * ydif)));
    }

}
