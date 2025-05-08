using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            float animationLength = anim.GetCurrentAnimatorStateInfo(0).length;
            Destroy(gameObject, animationLength);
        }
        else
        {
            Debug.LogWarning("No Animator found. Destroying.");
            Destroy(gameObject);
        }
    }

}
