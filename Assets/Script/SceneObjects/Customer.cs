using UnityEngine;
using System.Collections;
public class Customer : MonoBehaviour
{
    private Animator[] customers;
    [SerializeField] private GameObject Parti_sing;
    [SerializeField] private GameObject Parti_shock;
    private void Start()
    {
        customers = GetComponentsInChildren<Animator>();
        Parti_sing.SetActive(true);
        Parti_shock.SetActive(false);
    }
    public void Shock()
    {
        foreach (var customer in customers)
        {
            customer.SetBool("Shock",true);
        }
        StartCoroutine(InShock());
    }
    public void resetShock()
    {
        
        foreach (var customer in customers)
        {
            customer.gameObject.SetActive(true);
            customer.SetBool("Shock", false);
        }
        Parti_sing.SetActive(true);
        Parti_shock.SetActive(false);
    }
    IEnumerator InShock()
    {
        Parti_sing.SetActive(false);
        yield return new WaitForSeconds(0.35f);
        foreach (var customer in customers)
        {
            customer.gameObject.SetActive(false);
        }
        Parti_shock.SetActive(true);
    }
}
