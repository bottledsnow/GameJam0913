using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour
{
    [SerializeField] private KTVEnemy ktvEnemy;
    [SerializeField] private Transform[] targets;
    [SerializeField] private Room room;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Collider-Center")
        {
            ktvEnemy = other.GetComponentInParent<KTVEnemy>();
            ktvEnemy.GiveTarget(giveTarget());
        }
    }
    private Transform giveTarget()
    {
        if (targets == null || targets.Length == 0)
        {
            Debug.LogWarning("no Target¡I");
            return null;
        }
        turnLightOn();
        int randomIndex = Random.Range(0, targets.Length);
        Debug.Log("Enemy move to¡G" + targets[randomIndex].gameObject.name);
        return targets[randomIndex];
    }
    private void turnLightOn()
    {
        if (room == null) return;

        if(room.lightroom.LightOut == true)
        {
            StartCoroutine(LightOn());
            StartCoroutine(ktvEnemy.ToTurnLightOn());
        }
        
    }
    IEnumerator LightOn()
    {
        yield return new WaitForSeconds(ktvEnemy.stayTime); // ¨ü timeScale ¼vÅT
        room.lightOn();
    }
}
