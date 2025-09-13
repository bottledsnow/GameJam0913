using UnityEngine;

public class RandomAnimatorStart : MonoBehaviour
{
    void Start()
    {
        Animator anim = GetComponent<Animator>();

        // 把動畫跳到隨機時間點
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        anim.Play(state.fullPathHash, -1, Random.value);
    }
}