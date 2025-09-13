using UnityEngine;

public class AnimatorControllerPlayer : MonoBehaviour
{
    public Animator animator;

    // 在 Inspector 上設定動畫狀態名稱，例如 "Idle"、"Walk"、"Attack"
    public string stateName = "Idle";
    public bool testAnimationState = false;

    void Start()
    {
        // 取得角色上的 Animator
        animator = GetComponent<Animator>(); 

        if (animator == null)
        {
            Debug.LogError("找不到 Animator 元件！");
            return;
        }



        // 啟動後直接播放指定動畫狀態
        animator.Play(stateName, 0, 0f);
    }

    void Update()
    {
        // 測試：按下 A 鍵播放 Attack 動畫
        if (testAnimationState)
        {   
            testAnimationState = false; // 重置測試標誌
            animator.Play(stateName);
        }
    }
}