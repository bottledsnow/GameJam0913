using Movement;
using UnityEngine;

[System.Serializable]
public class CharacterAnimationStateData
{
    [SerializeField] public AnimationStateEnum state;
    [SerializeField] public string animationName;
    [SerializeField] public bool isLoop = true; // 是否重複播放

    public CharacterAnimationStateData(AnimationStateEnum state, string animationName, bool isLoop = true)
    {
        this.state = state;
        this.animationName = animationName;
    }

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
}
