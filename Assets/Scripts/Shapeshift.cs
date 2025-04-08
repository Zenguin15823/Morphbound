using UnityEngine;

public class Shapeshift : MonoBehaviour
{
    private int currentForm;
    private float timeSince;
    private SceneSwitcher sw;

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

        sw = GetComponent<SceneSwitcher>();
    }

    void Update()
    {
        if (sw.isPaused) return;

        timeSince += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E) && timeSince >= cooldown) changeForm();
    }

    void changeForm()
    {
        GameObject[] forms = { cat, rhino };

        // check if dead - don't transform if so
        if (forms[currentForm].GetComponent<Death>().dead) return;

        // bring other forms to current position
        for (int i = 0; i < forms.Length; i++)
        {
            if (i != currentForm) forms[i].transform.position = forms[currentForm].transform.position;
        }

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

    public GameObject getCurrentForm()
    {
        if (currentForm == 0) return cat;
        if (currentForm == 1) return rhino;
        else return null;
    }
}
