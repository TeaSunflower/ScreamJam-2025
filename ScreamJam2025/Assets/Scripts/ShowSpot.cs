using NavMeshPlus.Components;
using UnityEngine;

public class ShowSpot : MonoBehaviour
{
    public GameObject player;
    private bool highlighted = false;
    public bool placed = false;
    private SpriteRenderer sr;
    PlaceSalt manager;
    NavMeshModifier nav;
    Collider2D myCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = PlaceSalt.Instance;
        player = manager.player;
        myCollider = GetComponent<Collider2D>();
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(myCollider, playerCollider, true);
        sr = GetComponent<SpriteRenderer>();
        nav = GetComponent<NavMeshModifier>();
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
            if (manager.heldSalt > 0 && !highlighted && dist <= 2)
            {
                highlighted = true;
                sr.enabled = true;
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

    /// <summary>
    /// places the salt spot
    /// </summary>
    public void SetPlaced()
    {
        myCollider.enabled = true;
        sr.enabled = true;
        sr.color = Color.white;
        placed = true;
        nav.area = 1;
    }

    public void StartDisabled()
    {
        sr.enabled = false;
    }
}
