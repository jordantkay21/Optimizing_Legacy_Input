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

    [SerializeField]
    private Forklift _forkLift;

    // Start is called before the first frame update
    void Start()
    {
        InitializeInputs();
    }

    // Update is called once per frame
    void Update()
    {
        //Player Controls
        Vector2 playerMovement = _input.Player.Movement.ReadValue<Vector2>();
        _player.Move(playerMovement);

        //Drone Controls
        float rotateDirection = _input.Drone.Rotation.ReadValue<float>();
        _drone.CalculateMovementUpdate(rotateDirection);

        float heightDirection = _input.Drone.Height.ReadValue<float>();
        _drone.CalculateMovementFixedUpdate(heightDirection);

        Vector2 droneMovement = _input.Drone.Movement.ReadValue<Vector2>();
        _drone.CalculateTilt(droneMovement);

        //Fork Lift Controls
        Vector2 forkLiftMove = _input.ForkLift.Movement.ReadValue<Vector2>();
        _forkLift.CalculateMovement(forkLiftMove);

    }

    void InitializeInputs()
    {
        _input = new PlayerInputActions();
        _input.Player.Enable();

        _input.Drone.Escape.performed += Escape_performed;

        _input.ForkLift.Escape.performed += Escape_performed1;

        _input.ForkLift.Lift.started += Lift_started;
        _input.ForkLift.Lift.canceled += Lift_canceled;
    }

    private void Lift_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        float liftDirection = 0;
        _forkLift.LiftDirection(liftDirection);
    }

    private void Lift_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        float liftDirection = _input.ForkLift.Lift.ReadValue<float>();
        _forkLift.LiftDirection(liftDirection);
    }

    private void Escape_performed1(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _forkLift.ExitDriveMode();
    }

    private void Escape_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _drone.ExitFlightMode();
    }

    public void EnableDriveMode(bool driveMode)
    {
        if (driveMode == true)
        {
            _input.Player.Disable();
            _input.ForkLift.Enable();
        }
        else
        {
            _input.Player.Enable();
            _input.ForkLift.Disable();
        }
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
