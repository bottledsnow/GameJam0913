using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private KTVEnemy ktvEnemy;
    [SerializeField] private Transform[] targets;
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

        int randomIndex = Random.Range(0, targets.Length);
        Debug.Log("Enemy move to¡G" + targets[randomIndex].gameObject.name);
        return targets[randomIndex];
    }    
}
