using UnityEngine;

public class PickupItem : MonoBehaviour
{
    PlaceSalt manager;
    public int index;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = PlaceSalt.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// picks up the item only if the player collides with it
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            manager.heldSalt++;
            manager.spawnedSalt--;
            manager.spawnLocations.Add(gameObject.transform.position);
            manager.inactives.Add(index);
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// resets the item
    /// </summary>
    /// <param name="pos">spawn position</param>
    public void Respawn(int _index, Vector2 pos)
    {
        transform.position = pos;
        index = _index;
        gameObject.SetActive(true);
    }
}
