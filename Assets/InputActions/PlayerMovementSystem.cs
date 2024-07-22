
using Unity.Entities;
using UnityEngine.InputSystem;
using Unity.Mathematics;

public partial class PlayerMovementSystem : SystemBase
{
    private PlayerMovementInputAction playerInputAction;
    private Entity playerSpaceShipEntity;

    protected override void OnCreate()
    {
        playerInputAction = new PlayerMovementInputAction();
        playerInputAction.Player.Enable();

        playerInputAction.Player.Shoot.performed += OnShoot;
        playerInputAction.Player.Shoot.canceled += EndShoot;
    }

    protected override void OnStartRunning()
    {
        playerSpaceShipEntity = SystemAPI.GetSingletonEntity<PlayerTagComponent>();

        if (!SystemAPI.Exists(playerSpaceShipEntity)) return;
        SystemAPI.SetComponentEnabled<PlayerShootComponent>(playerSpaceShipEntity, false);
    }

    protected override void OnUpdate() {
        UnityEngine.Vector2 input = playerInputAction.Player.Movement.ReadValue<UnityEngine.Vector2>();
        float2 inputMove = new float2(input.x, input.y);
        SystemAPI.SetSingleton(new PlayerMoveComponent { moveInput = inputMove });
    }

    protected override void OnStopRunning() { 
        playerInputAction.Disable();
        playerSpaceShipEntity = Entity.Null;
    }

    private void OnShoot(InputAction.CallbackContext contect)
    {
        if (!SystemAPI.Exists(playerSpaceShipEntity)) return;
        SystemAPI.SetComponentEnabled<PlayerShootComponent>(playerSpaceShipEntity, true);
    }

    private void EndShoot(InputAction.CallbackContext context)
    {
        if (!SystemAPI.Exists(playerSpaceShipEntity)) return;
        SystemAPI.SetComponentEnabled<PlayerShootComponent>(playerSpaceShipEntity, false);
    }
}
