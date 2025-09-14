using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Timer GameTimer;

    [Header("Setting")]
    [SerializeField] private float gameTime;

    [Header("CountDown")]
    [SerializeField] private GameObject ConutDown_3;
    [SerializeField] private GameObject ConutDown_2;
    [SerializeField] private GameObject ConutDown_1;
    [SerializeField] private GameObject ConutDown_All;
    [SerializeField] private GameObject GameStartUI;

    private Pause pause;
    [Header("Ending")]
    [SerializeField]  private GameObject Ending;
    private void Awake()
    {
        pause = GetComponent<Pause>();
        GameTimer.TimerEnded += toEnd;
        if (pause == null)
        {
            Debug.LogError("Pause component not found on GameManager.");
        }
    }

    private void Start()
    {
        StartCoroutine(starGame());
    }
    private void toEnd()
    {
        Ending.SetActive(true);
    }
    IEnumerator starGame()
    {
        pause.enabled = false;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1);
        GameStartUI.SetActive(false);  
        ConutDown_3.SetActive(false);
        ConutDown_2.SetActive(false);
        ConutDown_1.SetActive(false);
        ConutDown_All.SetActive(true);
        ConutDown_3.SetActive(true);
        ConutDown_2.SetActive(false);
        ConutDown_1.SetActive(false);
        yield return new WaitForSecondsRealtime(1);
        ConutDown_3.SetActive(false);
        ConutDown_2.SetActive(true);
        ConutDown_1.SetActive(false);
        yield return new WaitForSecondsRealtime(1);
        ConutDown_3.SetActive(false);
        ConutDown_2.SetActive(false);
        ConutDown_1.SetActive(true);
        yield return new WaitForSecondsRealtime(1);
        ConutDown_3.SetActive(false);
        ConutDown_2.SetActive(false);
        ConutDown_1.SetActive(false);
        GameTimer.StartTimer(gameTime);
        ConutDown_All.SetActive(false);
        Time.timeScale = 1;
        pause.enabled = true;
        GameStartUI.SetActive(true);
        yield return new WaitForSecondsRealtime(1);
        GameStartUI.SetActive(false);
    }
}
