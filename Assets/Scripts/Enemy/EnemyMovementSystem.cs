using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateAfter(typeof(MisileMovementSystem))]
public partial struct EnemyMovementSystem : ISystem
{
    private JobHandle jobHandler;
 
    private void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemyMovementComponent>();
    }

    [BurstCompile]
    private void OnUpdate(ref SystemState state)
    {
        EnemyMovementJob enemyJob = new EnemyMovementJob { deltaTime = SystemAPI.Time.DeltaTime };
        enemyJob.ScheduleParallel();  
    }
}

[UpdateAfter(typeof(MisileMovementSystem))]
public partial struct EnemySpawnSystem : ISystem
{  
    private float timeBetweenSpawn, timer;
    private void OnCreate(ref SystemState state)
    {
        timer = 0.0f;
        timeBetweenSpawn = 4.0f;
        state.RequireForUpdate<EnemyMovementComponent>();
    }

    [BurstCompile]   
    private void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(state.WorldUpdateAllocator);
        SpawnMissileComponent spawnMissileComponent = SystemAPI.GetSingleton<SpawnMissileComponent>();

        foreach ((RefRO<LocalTransform> localTransform, RefRO<EnemyInfoComponent> enemyInfo, RefRO<EnemyMovementComponent> enemyMove) 
            in SystemAPI.Query<RefRO<LocalTransform>, RefRO<EnemyInfoComponent>, RefRO<EnemyMovementComponent>>().WithAll<EnemyMovementComponent>())
        {
            timer += SystemAPI.Time.DeltaTime;
            if (timer < timeBetweenSpawn) return;
            timer = 0.0f;

            Entity missile1 = entityCommandBuffer.Instantiate(spawnMissileComponent.enemyMissileEntity);
            entityCommandBuffer.SetComponent(missile1, new LocalTransform
            {
                Position = localTransform.ValueRO.Position + new float3(-3.8f, 0.25f, 2f),
                Rotation = quaternion.identity,
                Scale = 1f
            });

            Entity missile2 = entityCommandBuffer.Instantiate(spawnMissileComponent.enemyMissileEntity);
            entityCommandBuffer.SetComponent(missile2, new LocalTransform
            {
                Position = localTransform.ValueRO.Position + new float3(3.8f, 0.25f, 2f),
                Rotation = quaternion.identity,
                Scale = 1f
            });
        }

        entityCommandBuffer.Playback(state.EntityManager);
    }
}

[WithAll(typeof(EnemyMovementComponent))]
[BurstCompile]
public partial struct EnemyMovementJob : IJobEntity
{
    public float deltaTime;
    private void Execute(ref LocalTransform localTransform, in EnemyMovementComponent movement)
    {
        localTransform = localTransform.Translate(movement.moveSpeed * new float3(0, 0, -1) * deltaTime);
    }
}
