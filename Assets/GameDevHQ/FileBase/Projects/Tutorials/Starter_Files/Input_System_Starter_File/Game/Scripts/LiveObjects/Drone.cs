using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Game.Scripts.UI;

namespace Game.Scripts.LiveObjects
{
    public class Drone : MonoBehaviour
    {
        [SerializeField]
        private PlayerManager _playerManager;

        private enum Tilt
        {
            NoTilt, Forward, Back, Left, Right
        }

        [SerializeField]
        private Rigidbody _rigidbody;
        [SerializeField]
        private float _speed = 5f;
        private bool _inFlightMode = false;
        [SerializeField]
        private Animator _propAnim;
        [SerializeField]
        private CinemachineVirtualCamera _droneCam;
        [SerializeField]
        private CinemachineVirtualCamera _playerFollowCam;
        [SerializeField]
        private InteractableZone _interactableZone;
        

        public static event Action OnEnterFlightMode;
        public static event Action onExitFlightmode;

        private void OnEnable()
        {
            InteractableZone.onZoneInteractionComplete += EnterFlightMode;
        }

        
        private void EnterFlightMode(InteractableZone zone)
        {
            if (_inFlightMode != true && zone.GetZoneID() == 4) // drone Scene
            {
                _propAnim.SetTrigger("StartProps");
                _droneCam.Priority = 11;
                _inFlightMode = true;
                _playerManager.EnableDrone(true);
                OnEnterFlightMode?.Invoke();
                UIManager.Instance.DroneView(true);
                _interactableZone.CompleteTask(4);
            }
        }

        public void ExitFlightMode()
        {            
            _droneCam.Priority = 9;
            _playerFollowCam.Priority = 11;
            _inFlightMode = false;
            _playerManager.EnableDrone(false);
            UIManager.Instance.DroneView(false);            
        }
     
        /*private void Update()
        {
            if (_inFlightMode)
            {
                CalculateTilt();
                CalculateMovementUpdate();

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _inFlightMode = false;
                    onExitFlightmode?.Invoke();
                    ExitFlightMode();
                }
            }
        }*/


        private void FixedUpdate()
        {
            _rigidbody.AddForce(transform.up * (9.81f), ForceMode.Acceleration);
           // if (_inFlightMode)
             //   CalculateMovementFixedUpdate();
        }

        public void CalculateMovementUpdate(float rotateDirection)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 30f * rotateDirection);
        }

        /*private void CalculateMovementUpdate()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                var tempRot = transform.localRotation.eulerAngles;
                tempRot.y -= _speed / 3;
                transform.localRotation = Quaternion.Euler(tempRot);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                var tempRot = transform.localRotation.eulerAngles;
                tempRot.y += _speed / 3;
                transform.localRotation = Quaternion.Euler(tempRot);
            }
        }*/

        public void CalculateMovementFixedUpdate(float heightDirection)
        {
            _rigidbody.AddForce((heightDirection * transform.up) * _speed, ForceMode.Acceleration);
        }

        /*private void CalculateMovementFixedUpdate()
        {
            
            if (Input.GetKey(KeyCode.Space))
            {
                _rigidbody.AddForce(transform.up * _speed, ForceMode.Acceleration);
            }
            if (Input.GetKey(KeyCode.V))
            {
                _rigidbody.AddForce(-transform.up * _speed, ForceMode.Acceleration);
            }
        }*/

        public void CalculateTilt(Vector2 direction)
        {
            transform.rotation = Quaternion.Euler((direction.y * 30f), transform.localRotation.eulerAngles.y, (direction.x * 30f));
        }

        /*private void CalculateTilt()
        {
            if (Input.GetKey(KeyCode.A)) 
                transform.rotation = Quaternion.Euler(00, transform.localRotation.eulerAngles.y, 30);
            else if (Input.GetKey(KeyCode.D))
                transform.rotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, -30);
            else if (Input.GetKey(KeyCode.W))
                transform.rotation = Quaternion.Euler(30, transform.localRotation.eulerAngles.y, 0);
            else if (Input.GetKey(KeyCode.S))
                transform.rotation = Quaternion.Euler(-30, transform.localRotation.eulerAngles.y, 0);
            else 
                transform.rotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);
        }*/

        private void OnDisable()
        {
            InteractableZone.onZoneInteractionComplete -= EnterFlightMode;
        }
    }
}
