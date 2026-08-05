using UnityEngine;

namespace Game.ReadOnly
{
    [CreateAssetMenu(menuName = "Game/Config/PlayerMovement")]
    public class PlayerMovementData : ScriptableObject
    {
        [field: Header("Speed Settings")]
        [field:SerializeField] public float WalkSpeed { get; private set; } = 4.5f;
        [field: SerializeField] public float RunSpeed  { get; private set; } = 7f;
        [field: SerializeField] public float CrouchSpeed  { get; private set; } = 2.5f;
        [field: SerializeField] public float MovementSmoothing { get; private set; } = 15f;

        [field: Header("Physics Settings")]
        [field: SerializeField] public float Gravity  { get; private set; } = -15f;

        [field: Header("Jump Settings")]
        [field: SerializeField] public bool UseJump  { get; private set; } = true;
        [field: SerializeField] public float JumpForce  { get; private set; } = 5f;
        [field: SerializeField] public float CoyoteTime { get; private set; } = 0.2f;
        [field: SerializeField] public float JumpBufferTime { get; private set; } = 0.2f;

        [field: Header("Crouch Settings")]
        [field: SerializeField] public bool UseCrouch  { get; private set; } = true;
        [field: SerializeField] public float CrouchTransitionSpeed { get; private set; } = 10f;
        [field: SerializeField] public float CameraOffset { get; private set; } = 0.15f;

        [field: Header("Dodge Settings")]
        [field: SerializeField] public bool UseDodge { get; private set; } = true;
        [field: SerializeField] public float DodgeSpeed { get; private set; } = 15f;
        [field: SerializeField] public float DodgeDuration { get; private set; } = 0.3f;
        [field: SerializeField] public float DodgeCooldown { get; private set; } = 3f;
    }
}