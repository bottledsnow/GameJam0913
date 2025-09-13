using System.Collections.Generic;
using UnityEngine;
using Movement;


public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    public Dictionary<AnimationStateEnum, string> characterAnimationDic = new Dictionary<AnimationStateEnum, string>(){ 
            { AnimationStateEnum.Idle, "Idle" },
            { AnimationStateEnum.Scare, "Scare" },
            { AnimationStateEnum.Walk, "Walk" },
            { AnimationStateEnum.Run, "Run" },
            { AnimationStateEnum.Scared, "Scared" },
            };
    private Dictionary<Move, float> directionRotations = new Dictionary<Move, float>()
        {
            { Move.Up, 0f },      // 向上
            { Move.Down, 180f },  // 向下
            { Move.Left, 90f },   // 向左
            { Move.Right, 270f }  // 向右
        };
    private AnimationStateEnum currentState = AnimationStateEnum.Idle; // 当前动画状态
    
    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
 
    }
    
    private void InitializeAnimationDictionary()
    {
        // 初始化動畫字典 - 每個狀態只有一個動畫
        characterAnimationDic = new Dictionary<AnimationStateEnum, string>
        {
            { AnimationStateEnum.Idle, "Idle" },
            { AnimationStateEnum.Scare, "Scare" },
            { AnimationStateEnum.Walk, "Walk" },
            { AnimationStateEnum.Run, "Run" },
            { AnimationStateEnum.Scared, "Scared" },
        };
        
        // 初始化方向旋轉角度字典
        directionRotations
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
        if (characterAnimationDic.ContainsKey(state))
        {
            string animationName = characterAnimationDic[state];
            animator.Play(animationName);
            
            // 根據移動方向旋轉角色
            ApplyRotation(move);
        }
        else
        {
            Debug.LogWarning($"Animation for state {state} is not defined!");
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
        if (directionRotations.ContainsKey(direction))
        {
            float rotationY = directionRotations[direction];
            transform.rotation = Quaternion.Euler(0, rotationY, 0);
        }
    }
}