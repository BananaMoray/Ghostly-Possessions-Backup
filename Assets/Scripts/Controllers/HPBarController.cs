using UnityEngine;

public class HPBarController : MonoBehaviour
{
    public GameObject owner;

    private void Update()
    {
        transform.position = owner.transform.position;
    }
}
