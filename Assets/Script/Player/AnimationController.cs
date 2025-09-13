using System.Collections.Generic;
using UnityEngine;
using Movement;
using System.Linq;

[System.Serializable]
public class CharacterAnimationData
{
    [SerializeField] public AnimationStateEnum state;
    [SerializeField] public string animationName;
    [SerializeField] public bool isLoop = true; // 是否重複播放
    
    public CharacterAnimationData(AnimationStateEnum state, string animationName, bool isLoop = true)
    {
        this.state = state;
        this.animationName = animationName;
        this.isLoop = isLoop;
    }
}

[System.Serializable]
public class DirectionRotationData
{
    [SerializeField] public Move direction;
    [SerializeField] public float rotationY;
    
    public DirectionRotationData(Move direction, float rotationY)
    {
        this.direction = direction;
        this.rotationY = rotationY;
    }
}


public class AnimationController : MonoBehaviour
{
    public bool testPlayAnimation = false; // 測試用，啟動時自動播放動畫
    public AnimationStateEnum testAnimationState = AnimationStateEnum.Idle; // 測試
    public Move testMoveDirection = Move.Down; // 測試用移動方向

    [SerializeField] private Animator animator;
    
    [SerializeField] public List<CharacterAnimationData> characterAnimationList = new List<CharacterAnimationData>
    {
        new CharacterAnimationData(AnimationStateEnum.Idle, "Idle", true),      // 待機循環播放
        new CharacterAnimationData(AnimationStateEnum.Scare, "Scare", false),  // 驚嚇單次播放
        new CharacterAnimationData(AnimationStateEnum.Walk, "Walk", true),     // 行走循環播放
        new CharacterAnimationData(AnimationStateEnum.Run, "Run", true),       // 跑步循環播放
        new CharacterAnimationData(AnimationStateEnum.Scared, "Scared", true)  // 害怕循環播放
    };
    [SerializeField] public List<DirectionRotationData> directionRotationList = new List<DirectionRotationData>
    {
        new DirectionRotationData(Move.Up, 0f),
        new DirectionRotationData(Move.Down, 180f),
        new DirectionRotationData(Move.Left, 90f),
        new DirectionRotationData(Move.Right, 270f)
    };
    private Move currentMove = Move.Down; // 當前移動方向
    private AnimationStateEnum currentState = AnimationStateEnum.Idle; // 当前动画状态

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
            
        if (animator == null)
        {
            Debug.LogError("Animator component not found! Please add an Animator component to this GameObject.");
        }
        else
        {
            Debug.Log($"Animator found: {animator.name}");
            if (animator.runtimeAnimatorController == null)
            {
                Debug.LogWarning("Animator Controller is not assigned!");
            }
            else
            {
                Debug.Log($"Animator Controller: {animator.runtimeAnimatorController.name}");
            }
        }
    }

    void Update()
    {
        if (testPlayAnimation)
        {
            testPlayAnimation = false; // 重置測試標誌
            ChangeCharacterState(testAnimationState, testMoveDirection);
        }
    }
    
    // 使用當前面向切換動作狀態
    public void ChangeCharacterState(AnimationStateEnum state)
    {
        ChangeCharacterState(state, currentMove);
    }
    
    // 指定狀態和面向切換動作
    public void ChangeCharacterState(AnimationStateEnum state, Move move)
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator is not assigned!");
            return;
        }
        
        currentState = state;
        currentMove = move;
        
        // 播放動畫
        string animationName = GetAnimationNameInternal(state);
        Debug.Log($"Attempting to play animation: {animationName} for state: {state}");
        
        if (!string.IsNullOrEmpty(animationName))
        {
            Debug.Log($"Playing animation: {animationName}");
            animator.Play(animationName);
            
            // 根據移動方向旋轉角色
            ApplyRotation(move);
            Debug.Log($"Applied rotation for move: {move}");
            
            // 如果是單次播放動畫，設定事件監聽
            var animationData = GetAnimationDataInternal(state);
            if (animationData != null && !animationData.isLoop)
            {
                Debug.Log($"Animation {animationName} is set to single play, starting coroutine");
                StartCoroutine(WaitForAnimationComplete(animationName));
            }
            else
            {
                Debug.Log($"Animation {animationName} is set to loop");
            }
        }
        else
        {
            Debug.LogWarning($"Animation for state {state} is not defined or animation name is empty!");
            Debug.LogWarning($"Available animations: {string.Join(", ", characterAnimationList.Select(x => $"{x.state}:{x.animationName}"))}");
        }
    }
    
    // 僅改變面向，保持當前動作狀態
    public void ChangeFacing(Move direction)
    {
        if (currentMove != direction)
        {
            currentMove = direction;
            ApplyRotation(direction);
        }
    }
    
    // 根據移動方向旋轉角色
    private void ApplyRotation(Move direction)
    {
        var rotationData = directionRotationList.FirstOrDefault(x => x.direction == direction);
        if (rotationData != null)
        {
            transform.rotation = Quaternion.Euler(0, rotationData.rotationY, 0);
        }
    }
    
    // 獲取動畫名稱（私有方法，供內部使用）
    private string GetAnimationNameInternal(AnimationStateEnum state)
    {
        var animationData = characterAnimationList.FirstOrDefault(x => x.state == state);
        return animationData?.animationName;
    }
    
    // 獲取動畫數據（私有方法，供內部使用）
    private CharacterAnimationData GetAnimationDataInternal(AnimationStateEnum state)
    {
        return characterAnimationList.FirstOrDefault(x => x.state == state);
    }
    
    // 檢查動畫是否應該循環播放
    public bool ShouldAnimationLoop(AnimationStateEnum state)
    {
        var animationData = GetAnimationDataInternal(state);
        return animationData?.isLoop ?? true; // 預設為循環播放
    }
    
    // 檢查特定狀態的動畫是否存在
    public bool HasAnimation(AnimationStateEnum state)
    {
        return characterAnimationList.Any(x => x.state == state);
    }
    
    // 檢查動畫控制器是否就緒
    public bool IsReady()
    {
        return animator != null;
    }

    // 公開的獲取動畫名稱方法
    public string GetAnimationName(AnimationStateEnum state)
    {
        var animationData = characterAnimationList.FirstOrDefault(x => x.state == state);
        return animationData?.animationName;
    }

    // 公開的獲取動畫數據方法
    public CharacterAnimationData GetAnimationData(AnimationStateEnum state)
    {
        return characterAnimationList.FirstOrDefault(x => x.state == state);
    }
    
    // 強制播放動畫（即使是相同狀態）
    public void ForcePlayAnimation(AnimationStateEnum state, Move move)
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator is not assigned!");
            return;
        }
        
        string animationName = GetAnimationNameInternal(state);
        if (!string.IsNullOrEmpty(animationName))
        {
            currentState = state;
            currentMove = move;
            animator.Play(animationName, 0, 0f); // 從頭開始播放
            ApplyRotation(move);
            
            // 如果是單次播放動畫，設定事件監聽
            var animationData = GetAnimationDataInternal(state);
            if (animationData != null && !animationData.isLoop)
            {
                StartCoroutine(WaitForAnimationComplete(animationName));
            }
        }
        else
        {
            Debug.LogWarning($"Animation for state {state} is not defined!");
        }
    }
    
    // 等待動畫播放完成的協程
    private System.Collections.IEnumerator WaitForAnimationComplete(string animationName)
    {
        // 等待動畫開始播放
        yield return null;
        
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (stateInfo.IsName(animationName) && stateInfo.normalizedTime < 1.0f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }
        
        // 動畫播放完成，可以在這裡添加回調事件
        OnAnimationComplete(currentState);
    }
    
    // 動畫完成事件（可以被重寫或擴展）
    protected virtual void OnAnimationComplete(AnimationStateEnum completedState)
    {
        Debug.Log($"Animation {completedState} completed!");
        
        // 對於某些單次播放動畫，可能需要切換回待機狀態
        if (completedState == AnimationStateEnum.Scare)
        {
            ChangeCharacterState(AnimationStateEnum.Idle);
        }
    }
}