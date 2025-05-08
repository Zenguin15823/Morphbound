using UnityEngine;
using UnityEngine.UI;

public class Bossbar : MonoBehaviour
{
    public GameObject boss;

    private Enemy enemy;
    private Slider slider;

    void Start()
    {
        enemy = boss.GetComponent<Enemy>();
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = (enemy.health / enemy.maxHealth);
    }
}
