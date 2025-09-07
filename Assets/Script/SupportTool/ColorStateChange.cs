using UnityEngine;

public class ColorStateChange : MonoBehaviour
{
    private string Origin;
    [SerializeField] private ColorStateData ColorStateData;

    protected virtual void Awake()
    {
        Origin = gameObject.name;
        this.gameObject.name = ColorStateData.red + Origin;
    }
    public virtual void ColorToRed()
    {
        this.gameObject.name = ColorStateData.red + Origin;
    }
    public virtual void ColorToGreen()
    {
        this.gameObject.name = ColorStateData.green + Origin;
    }
}
