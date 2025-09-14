using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
	public string level;

	public void Level() 
	{    
        SceneManager.LoadScene(level);
	}
}