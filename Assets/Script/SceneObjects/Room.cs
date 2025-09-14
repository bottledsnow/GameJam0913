using SceneObjects;
using UnityEngine;
using System;

public class Room : MonoBehaviour
{
    [SerializeField] private Animator room;
    [SerializeField] private Customer customer;
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
        customer.resetShock();
    }
    public void lightOut()
    {
        room.Play("LightOut");
        roomStar.SeLightState(false);
        customer.Shock();
    }
}
