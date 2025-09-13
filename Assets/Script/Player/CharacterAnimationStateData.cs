using UnityEngine;

public class CharacterAnimationStateData
{
    [SerializeField] public AnimationStateEnum state;
    [SerializeField] public string animationName;
    [SerializeField] public bool isLoop = true; // 是否重複播放
    
    public CharacterAnimationStateData(AnimationStateEnum state, string animationName, bool isLoop = true)
    {
        this.state = state;
        this.animationName = animationName;
        this.isLoop = isLoop;
    }
}
