using UnityEngine;

public class Customer : MonoBehaviour
{
    private Animator[] customers;
    private void Start()
    {
        customers = GetComponentsInChildren<Animator>();
    }
    public void Shock()
    {
        foreach (var customer in customers)
        {
            customer.SetBool("Shock",true);
        }
    }
    public void resetShock()
    {
        foreach (var customer in customers)
        {
            customer.SetBool("Shock", false);
        }
    }
}
