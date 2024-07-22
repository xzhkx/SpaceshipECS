
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;

public partial struct PlayerSpaceShipSystem : ISystem
{
    private float timeBetweenSpawn, timer;
    private void OnCreate(ref SystemState state)
    {
        timeBetweenSpawn = 0.15f;
        timer = 0.0f;

        state.RequireForUpdate<PlayerMoveComponent>();
    }

    [BurstCompile] 
    private void OnUpdate(ref SystemState state) {

        foreach((RefRW<LocalTransform> localTransform, RefRO<PlayerMoveComponent> playerMove) 
            in SystemAPI.Query<RefRW<LocalTransform>, RefRO<PlayerMoveComponent>>().WithAll<PlayerMoveComponent>())
        {
            localTransform.ValueRW = localTransform.ValueRO.Translate(20 * new float3(playerMove.ValueRO.moveInput.x, 0, playerMove.ValueRO.moveInput.y)
                * SystemAPI.Time.DeltaTime);
        }

        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(state.WorldUpdateAllocator);
        SpawnMissileComponent spawnMissileComponent = SystemAPI.GetSingleton<SpawnMissileComponent>();

        foreach (RefRO<LocalTransform> localTransform in
            SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerShootComponent>())
        {
            timer += SystemAPI.Time.DeltaTime;
            if (timer < timeBetweenSpawn) return;
            timer = 0.0f;

            Entity missile1 = entityCommandBuffer.Instantiate(spawnMissileComponent.missileEntity);
            entityCommandBuffer.SetComponent(missile1, new LocalTransform
            {
                Position = localTransform.ValueRO.Position + new float3(-3.8f, 0.25f, 2f),
                Rotation = quaternion.identity,
                Scale = 1.0f
            });

            Entity missile2 = entityCommandBuffer.Instantiate(spawnMissileComponent.missileEntity);
            entityCommandBuffer.SetComponent(missile2, new LocalTransform
            {
                Position = localTransform.ValueRO.Position + new float3(3.8f, 0.25f, 2f),
                Rotation = quaternion.identity,
                Scale = 1.0f
            });
        }

        entityCommandBuffer.Playback(state.EntityManager);
    }
}
