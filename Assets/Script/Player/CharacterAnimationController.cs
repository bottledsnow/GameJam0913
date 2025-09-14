using System.Collections.Generic;
using UnityEngine;
using Movement;
using System.Linq;
using static CharacterAnimationStateData;

public class CharacterAnimationController : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private Animator animator;

    [Header("State Configuration")]
    [SerializeField]
    public List<CharacterAnimationStateData> stateSettings = new List<CharacterAnimationStateData>
    {
        new CharacterAnimationStateData(AnimationStateEnum.Idle, "Idle", true),
        new CharacterAnimationStateData(AnimationStateEnum.Walk, "Walk", true),
        new CharacterAnimationStateData(AnimationStateEnum.Run, "Run", true),
        new CharacterAnimationStateData(AnimationStateEnum.Scare, "Scare", false),
        new CharacterAnimationStateData(AnimationStateEnum.Scared, "Scared", true),
        new CharacterAnimationStateData(AnimationStateEnum.Clapping, "Clapping", true)
    };

    public bool testPlayAnimation = false; // 測試用，啟動時自動播放動畫

    [Header("Direction Configuration")]
    [SerializeField]
    public List<DirectionRotationData> directionSettings = new List<DirectionRotationData>
    {
        new DirectionRotationData(Move.Up, 0f),
        new DirectionRotationData(Move.Down, 180f),
        new DirectionRotationData(Move.Left, 270),
        new DirectionRotationData(Move.Right, 90)
    };

    [Header("Current State")]
    [SerializeField] public Move currentMove = Move.Down;
    [SerializeField] public AnimationStateEnum currentState = AnimationStateEnum.Idle;

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animation component not found! Please add an Animation component to this GameObject.");
        }
        ChangeStateAndFacing(currentState, currentMove);
    }

    void Start()
    {

    }

    void Update()
    {
        if (testPlayAnimation)
        {
            testPlayAnimation = false;
            ChangeState(currentState);
            Debug.Log($"[Test] Playing animation: {currentState} facing {currentMove}");
        }
    }

    // 切換動畫狀態
    public void ChangeState(AnimationStateEnum state)
    {
        ChangeStateAndFacing(state, currentMove);
    }

    // 切換面向方向
    public void ChangeFacing(Move direction)
    {
        if (currentMove != direction)
        {
            currentMove = direction;
            ApplyRotation(direction);
        }
    }

    // 切換狀態和面向
    public void ChangeStateAndFacing(AnimationStateEnum state, Move direction)
    {
        Debug.Log($"ChangeStateAndFacing called with state: {state}, direction: {direction}");
        if (animator == null)
        {
            Debug.LogWarning("Animator is not assigned!");
            return;
        }

        currentState = state;
        currentMove = direction;

        string animationName = GetAnimationName(state);

        Debug.Log($"animationName: {animationName}, state: {state}, direction: {direction}");
        if (!string.IsNullOrEmpty(animationName))
        {
            Debug.Log($"Playing animation: {animationName}");
            animator.Play(animationName, 0, 0f);
            ApplyRotation(direction);
        }
        else
        {
            Debug.LogWarning($"Animation for state {state} is not defined!");
        }
    }

    // 根據方向旋轉角色
    private void ApplyRotation(Move direction)
    {
        var rotationData = directionSettings.FirstOrDefault(x => x.direction == direction);
        if (rotationData != null)
        {
            transform.rotation = Quaternion.Euler(0, rotationData.rotationY, 0);
        }
    }

    // 獲取動畫名稱
    public string GetAnimationName(AnimationStateEnum state)
    {
        var animationData = stateSettings.Find(x => x.state == state);
        return animationData?.animationName;
    }

    // 獲取動畫數據
    public CharacterAnimationStateData GetAnimationData(AnimationStateEnum state)
    {
        return stateSettings.FirstOrDefault(x => x.state == state);
    }
}
