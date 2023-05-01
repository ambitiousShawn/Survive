using Shawn.ProjectFramework;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // »÷ÍË
    public void KnockBack(Vector3 direction, float time)
    {
        rb.AddForce(direction, ForceMode.Impulse);
    }
}
