using UnityEngine;

public class HPBarController : MonoBehaviour
{
    public GameObject owner;

    private void Update()
    {
        if (owner == null) return;
        transform.position = owner.transform.position;
    }
}
