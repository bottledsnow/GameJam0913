using SceneObjects;
using UnityEngine;
using System;

public class Room : MonoBehaviour
{
    [SerializeField] private Animator room;
    public LightRoom lightroom;

    private void Awake()
    {
        lightroom = GetComponent<LightRoom>();
        lightroom.LightTurnedOn += lightOn; 
        lightroom.LightTurnedOff += lightOut;
    }
    public void lightOn()
    {
        room.Play("LightOn");
        lightroom.LightOn();
    }
    public void lightOut()
    {
        room.Play("LightOut");
    }
}
