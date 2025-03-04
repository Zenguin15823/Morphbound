using UnityEngine;

public class Shapeshift : MonoBehaviour
{
    private int currentForm;
    private float timeSince;

    public GameObject cat;
    public GameObject rhino;
    public int defaultForm; // 0 for cat, 1 for rhino
    public float cooldown = 5; // seconds cooldown between transformations

    private void Start()
    {
        currentForm = defaultForm;
        timeSince = cooldown;

        if (defaultForm == 0)
        {
            cat.SetActive(true);
            rhino.SetActive(false);
        }
        else
        {
            rhino.SetActive(true);
            cat.SetActive(false);
        }
    }

    void Update()
    {
        timeSince += Time.deltaTime;

        GameObject[] forms = { cat, rhino }; 
        for (int i = 0; i < forms.Length; i++)
        {
            if (i != currentForm) forms[i].transform.position = forms[currentForm].transform.position;
        }

        if (Input.GetKeyDown(KeyCode.E) && timeSince >= cooldown) changeForm();
    }

    void changeForm()
    {
        GameObject[] forms = { cat, rhino };
        int nextForm = (currentForm + 1) % forms.Length;
        Collider2D hit = Physics2D.OverlapBox(forms[nextForm].transform.position, forms[nextForm].transform.localScale, 0f, 3);
        if (hit != null)
        {
            forms[currentForm].SetActive(false);
            forms[nextForm].SetActive(true);
            currentForm = nextForm;
            timeSince = 0;
        }
        else
        {
            print("Not enough room to shapeshift!");
        }
    }
}
