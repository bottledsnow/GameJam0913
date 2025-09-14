using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    
    [SerializeField] private RoomStar[] stars;
    [SerializeField] private TextMeshProUGUI text_score;
    [Header("Score")]
    [SerializeField] private float score = 0;
    public float GetScore()
    {
        return score;
    }
    private void Awake()
    {
        foreach (RoomStar star in stars)
        {
            star.OnStarCountChanged += UpdateScore;
        }
    }
    private void UpdateScore()
    {
        float totalScore = 0;
        foreach (RoomStar star in stars)
        {
            totalScore += star.GetStarCount();
        }
        int starUnitMutiple = (int)(totalScore/9 / 0.5f);
        score = starUnitMutiple *0.5f;
        text_score.text = score.ToString();
    }
}
