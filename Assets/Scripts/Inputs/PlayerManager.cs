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
    [SerializeField]
    private Crate _crate;
    [SerializeField]
    private Laptop _laptop;

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

        _input.Punch.StrongPunch.performed += StrongPunch_performed;
        _input.Punch.WeakPunch.performed += WeakPunch_performed;

        _input.Laptop.CameraSwitch.started += CameraSwitch_started;
        _input.Laptop.CameraSwitch.canceled += CameraSwitch_canceled;
        _input.Laptop.Escape.performed += Escape_performed2;
        _input.Laptop.Escape.canceled += Escape_canceled;
    }

    private void CameraSwitch_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _laptop.SwitchCam(true);
    }

    private void Escape_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _laptop.ExitCam(false);
    }

    private void Escape_performed2(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _laptop.ExitCam(true);
    }

    private void CameraSwitch_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _laptop.SwitchCam(false);
    }



    private void WeakPunch_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _crate.SetPunch(false);
    }

    private void StrongPunch_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _crate.SetPunch(true);
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

    public void EnableLaptopMode(bool laptopMode)
    {
        if (laptopMode == true)
        {
            _input.Laptop.Enable();
        }
        else
        {
            _input.Laptop.Disable();
        }
    }

    public void EnablePunchMode(bool punchMode)
    {
        if (punchMode == true)
        {
            _input.Punch.Enable();
        }
        else
        {
            _input.Punch.Disable();
        }
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
