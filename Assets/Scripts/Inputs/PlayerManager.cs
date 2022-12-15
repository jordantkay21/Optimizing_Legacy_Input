using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Scripts.Player;
using Game.Scripts.LiveObjects;

public class PlayerManager : MonoBehaviour
{

    private PlayerInputActions _input;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private Drone _drone;

    // Start is called before the first frame update
    void Start()
    {
        InitializeInputs();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = _input.Player.Movement.ReadValue<Vector2>();
        _player.Move(move);

        float rotateDirection = _input.Drone.Rotation.ReadValue<float>();
        _drone.CalculateMovementUpdate(rotateDirection);

        float heightDirection = _input.Drone.Height.ReadValue<float>();
        _drone.CalculateMovementFixedUpdate(heightDirection);

        Vector2 droneMovement = _input.Drone.Movement.ReadValue<Vector2>();
        _drone.CalculateTilt(droneMovement);
    }

    void InitializeInputs()
    {
        _input = new PlayerInputActions();
        _input.Player.Enable();

        _input.Drone.Escape.performed += Escape_performed;
    }

    private void Escape_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _drone.ExitFlightMode();
    }

    public void EnableDrone(bool flightmode)
    {
        if (flightmode == true)
        {
            _input.Player.Disable();
            _input.Drone.Enable();
        }
        else
        {
            _input.Player.Enable();
            _input.Drone.Disable();
        }
    }



}
