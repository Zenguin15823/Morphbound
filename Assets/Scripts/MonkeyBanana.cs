using UnityEngine;

public class MonkeyBanana : MonoBehaviour
{
    public GameObject banana;
    public Transform bananaPos;

    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 4)
        {
            timer = 0;
            throw_banana();
        }
    }

    void throw_banana()
    {
        Instantiate(banana, bananaPos.position, Quaternion.identity);
    }
}
