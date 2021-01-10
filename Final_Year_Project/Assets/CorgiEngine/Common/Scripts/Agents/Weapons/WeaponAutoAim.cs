﻿using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// This component will let you setup your weapon so that it auto aims at targets on the specified layer mask, within the specified radius, and with a line of sight.
    /// When a target is found, the weapon will swap its default aim mode to Script, until no more target is found. 
    /// It's important to have a base aim mode to revert to, such as PrimaryMovement 
    /// </summary>
    [AddComponentMenu("Corgi Engine/Weapons/Weapon Auto Aim")]
    [RequireComponent(typeof(Weapon))]
    public class WeaponAutoAim : MonoBehaviour
    {
        [MMInformation("This component will let you setup your weapon so that it auto aims at targets on the specified layer mask, within the specified radius, and with a line of sight." +
                       "When a target is found, the weapon will swap its default aim mode to Script, until no more target is found. " +
                       "It's important to have a base aim mode to revert to, such as PrimaryMovement",MoreMountains.Tools.MMInformationAttribute.InformationType.Info,false)]

        [Header("Layer Masks")]
        /// the layermask on which to look for aim targets
        [Tooltip("the layermask on which to look for aim targets")]
        public LayerMask TargetsMask;
        /// the layermask on which to look for obstacles
        [Tooltip("the layermask on which to look for obstacles")]
        public LayerMask ObstacleMask;

        [Header("Scan for Targets")]
        /// the radius (in units) around the character within which to search for targets
        [Tooltip("the radius (in units) around the character within which to search for targets")]
        public float ScanRadius = 15f;
        /// the size of the boxcast that will be performed to verify line of fire
        [Tooltip("the size of the boxcast that will be performed to verify line of fire")]
        public Vector2 LineOfFireBoxcastSize = new Vector2(0.1f, 0.1f);
        /// the duration (in seconds) between 2 scans for targets
        [Tooltip("the duration (in seconds) between 2 scans for targets")]
        public float DurationBetweenScans = 1f;
        /// an offset to apply to the weapon's position for scan 
        [Tooltip("an offset to apply to the weapon's position for scan ")]
        public Vector3 DetectionOriginOffset = Vector3.zero;

        [Header("Weapon Rotation")]
        /// the rotation mode to apply when a target is found
        [Tooltip("the rotation mode to apply when a target is found")]
        public WeaponAim.RotationModes RotationMode;

        [Header("Camera Target")]
        /// whether or not this component should take control of the camera target when a camera is found
        [Tooltip("whether or not this component should take control of the camera target when a camera is found")]
        public bool MoveCameraTarget = true;
        /// the normalized distance (between 0 and 1) at which the camera target should be, on a line going from the weapon owner (0) to the auto aim target (1) 
        [Tooltip("the normalized distance (between 0 and 1) at which the camera target should be, on a line going from the weapon owner (0) to the auto aim target (1)")]
        [Range(0f, 1f)]
        public float CameraTargetDistance = 0.5f;
        /// the maximum distance from the weapon owner at which the camera target can be
        [Tooltip("the maximum distance from the weapon owner at which the camera target can be")]
        [MMCondition("MoveCameraTarget", true)]
        public float CameraTargetMaxDistance = 10f;
        /// the speed at which to move the camera target
        [Tooltip("the speed at which to move the camera target")]
        [MMCondition("MoveCameraTarget", true)]
        public float CameraTargetSpeed = 5f;
        [Header("Debug")]
        /// the current target of the auto aim module
        [Tooltip("the current target of the auto aim module")]
        [MMReadOnly]
        public Transform Target;
        /// whether or not to draw a debug sphere around the weapon to show its aim radius
        [Tooltip("whether or not to draw a debug sphere around the weapon to show its aim radius")]
        public bool DrawDebugRadius = true;

        protected float _lastScanTimestamp = 0f;
        protected WeaponAim _weaponAim;
        protected WeaponAim.AimControls _originalAimControl;
        protected WeaponAim.RotationModes _originalRotationMode;
        protected Vector3 _raycastOrigin;
        protected Weapon _weapon;
        protected bool _originalMoveCameraTarget;

        protected Character _character;
        protected List<Collider2D> _detectionColliders;
        protected Vector2 _facingDirection;
        protected Vector3 _boxcastDirection;
        protected Vector3 _aimDirection;
        protected ContactFilter2D _contactFilter;
        protected Collider2D _potentialHit;
        protected bool _initialized = false;

        /// <summary>
        /// On Awake we initialize our component
        /// </summary>
        protected virtual void Start()
        {
            Initialization();
        }

        /// <summary>
        /// On init we grab our WeaponAim
        /// </summary>
        protected virtual void Initialization()
        {
            _weaponAim = this.gameObject.GetComponent<WeaponAim>();
            _weapon = this.gameObject.GetComponent<Weapon>();
            if (_weaponAim == null)
            {
                Debug.LogWarning(this.name + " : the WeaponAutoAim on this object requires that you add either a WeaponAim2D or WeaponAim3D component to your weapon.");
                return;
            }
            _originalAimControl = _weaponAim.AimControl;
            _originalRotationMode = _weaponAim.RotationMode;

            _character = _weapon.Owner;
            _contactFilter = new ContactFilter2D();
            _contactFilter.SetLayerMask(TargetsMask);
            _contactFilter.useLayerMask = true;
            _contactFilter.useDepth = false;
            _contactFilter.useNormalAngle= false;
            _contactFilter.useOutsideDepth= false;
            _contactFilter.useOutsideNormalAngle= false;
            _contactFilter.useTriggers= false;
            _detectionColliders = new List<Collider2D>();
            _initialized = true;
        }

        /// <summary>
        /// On Update, we setup our ray origin, scan periodically and set aim if needed
        /// </summary>
        protected virtual void Update()
        {
            if (_weaponAim == null)
            {
                return;
            }

            DetermineRaycastOrigin();
            ScanIfNeeded();
            HandleTarget();
            HandleMoveCameraTarget();
        }

        /// <summary>
        /// A method used to compute the origin of the detection casts
        /// </summary>
        protected virtual void DetermineRaycastOrigin()
        {
            if (_character != null)
            {
                _facingDirection = _character.IsFacingRight ? Vector2.right : Vector2.left;
                _raycastOrigin.x = transform.position.x + _facingDirection.x * DetectionOriginOffset.x / 2;
                _raycastOrigin.y = transform.position.y + DetectionOriginOffset.y;
            }
            else
            {
                _raycastOrigin = transform.position + DetectionOriginOffset;
            }
        }

        /// <summary>
        /// This method should define how the scan for targets is performed
        /// </summary>
        /// <returns></returns>
        protected virtual bool ScanForTargets()
        {
            if (!_initialized)
            {
                Initialization();
            }

            Target = null;

            Collider2D test = Physics2D.OverlapCircle(_weapon.Owner.transform.position, ScanRadius, TargetsMask);

            int count = Physics2D.OverlapCircle(_weapon.Owner.transform.position, ScanRadius, _contactFilter, _detectionColliders);

            if (count == 0)
            {
                return false;
            }
            else
            {
                float nearestDistance = float.MaxValue;
                float distance;
                foreach (Collider2D collider in _detectionColliders)
                {
                    distance = (_weapon.Owner.transform.position - collider.transform.position).sqrMagnitude;
                    if (distance < nearestDistance)
                    {
                        _potentialHit = collider;
                        nearestDistance = distance;
                    }
                }

                _boxcastDirection = (Vector2)(_potentialHit.bounds.center - _raycastOrigin);
                RaycastHit2D hit = Physics2D.BoxCast(_raycastOrigin, LineOfFireBoxcastSize, 0f, _boxcastDirection.normalized, _boxcastDirection.magnitude, ObstacleMask);
                if (!hit)
                {
                    Target = _potentialHit.transform;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Sends aim coordinates to the weapon aim component
        /// </summary>
        protected virtual void SetAim()
        {
            _aimDirection = (Target.transform.position - _raycastOrigin).normalized;
            _weaponAim.SetCurrentAim(_aimDirection);
        }

        /// <summary>
        /// Moves the camera target towards the auto aim target if needed
        /// </summary>
        protected Vector3 _newCamTargetPosition;
        protected Vector3 _newCamTargetDirection;

        protected virtual void HandleMoveCameraTarget()
        {
            if (!MoveCameraTarget || (Target == null))
            {
                return;
            }

            _newCamTargetPosition = Vector3.Lerp(_weapon.Owner.transform.position, Target.transform.position, CameraTargetDistance);
            _newCamTargetDirection = _newCamTargetPosition - this.transform.position;

            if (_newCamTargetDirection.magnitude > CameraTargetMaxDistance)
            {
                _newCamTargetDirection = _newCamTargetDirection.normalized * CameraTargetMaxDistance;
            }

            _newCamTargetPosition = this.transform.position + _newCamTargetDirection;

            _newCamTargetPosition = Vector3.Lerp(_weapon.Owner.CameraTarget.transform.position,
                _newCamTargetPosition,
                Time.deltaTime * CameraTargetSpeed);

            _weapon.Owner.CameraTarget.transform.position = _newCamTargetPosition;
        }

        /// <summary>
        /// Performs a periodic scan
        /// </summary>
        protected virtual void ScanIfNeeded()
        {
            if (Time.time - _lastScanTimestamp > DurationBetweenScans)
            {
                ScanForTargets();
                _lastScanTimestamp = Time.time;
            }
        }

        /// <summary>
        /// Sets aim if needed, otherwise reverts to the previous aim control mode
        /// </summary>
        protected virtual void HandleTarget()
        {
            if ((Target == null) || (!Target.gameObject.activeInHierarchy))
            {
                _weaponAim.AimControl = _originalAimControl;
                _weaponAim.RotationMode = _originalRotationMode;
            }
            else
            {
                _weaponAim.AimControl = WeaponAim.AimControls.Script;
                _weaponAim.RotationMode = RotationMode;
                SetAim();
            }
        }

        /// <summary>
        /// Draws a sphere around the weapon to show its auto aim radius
        /// </summary>
        protected virtual void OnDrawGizmos()
        {
            if (DrawDebugRadius)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_raycastOrigin, ScanRadius);
            }
        }

    }
}