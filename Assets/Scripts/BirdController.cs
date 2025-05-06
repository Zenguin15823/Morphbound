using UnityEngine;

public class BirdController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float moveDistance = 5f;

    [Header("Egg Dropping")]
    [SerializeField] private GameObject eggPrefab;
    [SerializeField] private float dropInterval = 3f;

    private Vector3 startPosition;
    private float direction = 1f;

    void Start()
    {
        startPosition = transform.position;

        if (eggPrefab != null)
        {
            InvokeRepeating(nameof(DropEgg), dropInterval, dropInterval);
        }
        else
        {
            Debug.LogError("Egg Prefab is not assigned in the BirdController script!", this.gameObject);
        }
    }

    void Update()
    {
        MoveSideToSide();
    }

    void MoveSideToSide()
    {
        float movement = direction * moveSpeed * Time.deltaTime;
        transform.Translate(movement, 0, 0);

        if (direction > 0 && transform.position.x >= startPosition.x + moveDistance)
        {
            direction = -1f;
            transform.position = new Vector3(startPosition.x + moveDistance, transform.position.y, transform.position.z);
        }
        else if (direction < 0 && transform.position.x <= startPosition.x - moveDistance)
        {
            direction = 1f;
            transform.position = new Vector3(startPosition.x - moveDistance, transform.position.y, transform.position.z);
        }
    }

    void DropEgg()
    {
        if (eggPrefab != null)
        {
            Instantiate(eggPrefab, transform.position, Quaternion.identity);
        }
    }
}