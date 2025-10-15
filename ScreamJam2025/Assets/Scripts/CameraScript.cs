using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    Transform player;

    Transform position;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        position = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition;
        newPosition = player.position;
        newPosition.z = -10;

        position.position = newPosition;
    }
}
