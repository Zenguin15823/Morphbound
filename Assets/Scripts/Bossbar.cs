using UnityEngine;

public class Bossbar : MonoBehaviour
{
    public float fullWidth;

    private GameObject boss;
    private Enemy enemy;

    void Start()
    {
        enemy = boss.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(fullWidth * (enemy.health / enemy.maxHealth), transform.localScale.y);
    }
}
