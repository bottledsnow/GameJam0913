using SceneObjects;
using UnityEngine;
using System;

public class Room : MonoBehaviour
{
    [SerializeField] private Animator room;
    public LightRoom lightroom;

    private RoomStar roomStar;
    private void Awake()
    {
        lightroom = GetComponent<LightRoom>();
        roomStar = GetComponent<RoomStar>();
        lightroom.LightTurnedOn += lightOn; 
        lightroom.LightTurnedOff += lightOut;
    }
    public void lightOn()
    {
        room.Play("LightOn");
        lightroom.LightOn();
        roomStar.SeLightState(true);
    }
    public void lightOut()
    {
        room.Play("LightOut");
        roomStar.SeLightState(false);
    }
}
