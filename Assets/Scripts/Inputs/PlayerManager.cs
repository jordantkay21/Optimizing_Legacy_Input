using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Scripts.Player;

public class PlayerManager : MonoBehaviour
{

    private PlayerInputActions _input;

    [SerializeField]
    private Player _player;

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
    }

    void InitializeInputs()
    {
        _input = new PlayerInputActions();
        _input.Player.Enable();
    }

}
