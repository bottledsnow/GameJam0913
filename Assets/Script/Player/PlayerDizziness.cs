using Movement;
using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerDizziness : MonoBehaviour
{
    [SerializeField] private ParticleSystem Parti_Dizz;
    private PlayerMovement playerMovement;
    private float playermovespeed;
    private bool isDizziness = false;
    [SerializeField] private float dizzinessTime = 3.00f;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playermovespeed = playerMovement.speed;
    }

    private void FixedUpdate()
    {
        if(!isDizziness && playerMovement.speed != playermovespeed)
        {
            playerMovement.speed = playermovespeed;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "EnemyDamage")
        {
            if (!isDizziness)
            {
                StartCoroutine(dizziness());
            }
        }
    }
    IEnumerator dizziness()
    {
        Parti_Dizz.gameObject.SetActive(true);
        playerMovement.speed = 0;
        isDizziness = true;
        yield return new WaitForSeconds(dizzinessTime); // ¨ü timeScale ¼vÅT
        playerMovement.speed = playermovespeed;
        isDizziness = false;
        Parti_Dizz.gameObject.SetActive(false);
    }
}
