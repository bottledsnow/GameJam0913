using TMPro;
using UnityEngine;

public class Ending : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private Score score;
    [SerializeField] private TextMeshProUGUI text_score;
    [Header("Ending")]
    [SerializeField] private GameObject GoodEnding;
    [SerializeField] private GameObject BadEnding;

    private float starScore;
    private void Start()
    {
        end();
    }
    public void end()
    {
        starScore = score.GetScore();
        text_score.text = starScore.ToString();

        if(starScore <2.5f)
        {
            GoodEnding.SetActive(true);
        }
        if(starScore >=2.5f)
        {
            BadEnding.SetActive(true);
        }
    }
}
