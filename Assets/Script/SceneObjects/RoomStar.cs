using UnityEngine;

public class RoomStar : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle_star;
    [SerializeField] private float starWaitTime;
    [SerializeField] private Color lightOutColor;
    private float timertime;
    private bool isLightOn = true;
    private int starCount = 1;
    private int maxStarCount = 5;

    public void SeLightState(bool state)
    {
        isLightOn = state;

        if (isLightOn == true)
        {
            particle_star.startColor = Color.white;
        }

        if(state == false)
        {
            particle_star.Clear();
            particle_star.Play();
            particle_star.startColor = lightOutColor;
            ResetStarNumber();
        }
    }
    public void ResetStarNumber()
    {
        starCount = 1;
        ChangeStarCount(starCount);
    }
    private void Update()
    {
        timer();
    }
    private void Awake()
    {
        ResetStarNumber();
        SeLightState(true);
    }
    private void timer()
    {
        if (isLightOn) timertime += Time.deltaTime;

        if (timertime > starWaitTime)
        {
            if (starCount < maxStarCount)
            {
                if(isLightOn)
                {
                    starCount++;
                }
                ChangeStarCount(starCount);
                timertime = 0;
            }
        }
    }
    private void ChangeStarCount(int count)
    {
        particle_star.emission.SetBurst(0, new ParticleSystem.Burst(0.0f, (short)count));
    }
}
