using UnityEngine;

public class ResetOnHit : MonoBehaviour
{
    private ResetLevel levelResetter;

    private void Start()
    {
        levelResetter = GetComponentInParent<ResetLevel>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            levelResetter.ReloadLevel();
        }
    }
}
