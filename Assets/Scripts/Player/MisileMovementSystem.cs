using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using UnityEngine;

public partial struct MisileMovementSystem : ISystem 
{
    private Entity entity;
    private void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<MissileComponent>();
    }

    [BurstCompile]
    private void OnUpdate(ref SystemState state)
    {
        MissileMovementJob moveJob = new MissileMovementJob { deltaTime = SystemAPI.Time.DeltaTime };
        moveJob.ScheduleParallel();  
        
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (RefRO<MissileComponent> missileComponent in SystemAPI.Query<RefRO<MissileComponent>>().WithAll<MissileComponent>())
        {
            if (missileComponent.ValueRO.isDead)
            {              
                Entity collideEntity = missileComponent.ValueRO.collideEntity;
                if (state.EntityManager.HasComponent<PlayerInfoComponent>(collideEntity))
                {
                    PlayerInfoComponent playerInfo = state.EntityManager.GetComponentData<PlayerInfoComponent>(collideEntity);
                    int newHealth = playerInfo.health - 1;
                    state.EntityManager.SetComponentData(collideEntity, new PlayerInfoComponent { health = newHealth });
                    Debug.Log("Player " + playerInfo.health);                   
                }
                if (state.EntityManager.HasComponent<EnemyInfoComponent>(collideEntity))
                {
                    EnemyInfoComponent enemyInfo = state.EntityManager.GetComponentData<EnemyInfoComponent>(collideEntity);
                    int newHealth = enemyInfo.health - 1;
                    state.EntityManager.SetComponentData(collideEntity, new EnemyInfoComponent { health = newHealth });
                    Debug.Log("Enemy" + enemyInfo.health);
                }
                entityCommandBuffer.DestroyEntity(missileComponent.ValueRO.thisEntity);
            }
        }
        entityCommandBuffer.Playback(state.EntityManager);
    }
}

[BurstCompile]
[WithAll(typeof(MissileComponent))]
public partial struct MissileMovementJob : IJobEntity
{
    public float deltaTime;
    public void Execute(ref LocalTransform localTransform, in MissileComponent missile)
    {
        localTransform = localTransform.Translate(missile.moveSpeed 
            * deltaTime * new float3(0, 0, 1));
    }
}
