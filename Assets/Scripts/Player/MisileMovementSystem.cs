using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using UnityEngine;
using System;

public partial class MisileMovementSystem : SystemBase 
{
    public Action<int> UpdatePlayerHealth;
    private Entity entity;
    protected override void OnCreate()
    {
        RequireForUpdate<MissileComponent>();
    }

    protected override void OnUpdate()
    {
        MissileMovementJob moveJob = new MissileMovementJob { deltaTime = SystemAPI.Time.DeltaTime };
        moveJob.ScheduleParallel();  
        
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(WorldUpdateAllocator);

        foreach (RefRO<MissileComponent> missileComponent in SystemAPI.Query<RefRO<MissileComponent>>().WithAll<MissileComponent>())
        {
            if (missileComponent.ValueRO.isDead)
            {              
                Entity collideEntity = missileComponent.ValueRO.collideEntity;
                if (EntityManager.HasComponent<PlayerInfoComponent>(collideEntity))
                {
                    PlayerInfoComponent playerInfo = EntityManager.GetComponentData<PlayerInfoComponent>(collideEntity);

                    if(playerInfo.health > 1)
                    {
                        int newHealth = playerInfo.health - 1;
                        EntityManager.SetComponentData(collideEntity, new PlayerInfoComponent { health = newHealth });
                        UpdatePlayerHealth?.Invoke(newHealth);
                    }
                    else
                    {
                        entityCommandBuffer.DestroyEntity(collideEntity);
                        UpdatePlayerHealth?.Invoke(0);
                    }
                  
                    Debug.Log("Player " + playerInfo.health);                   
                }
                if (EntityManager.HasComponent<EnemyInfoComponent>(collideEntity))
                {
                    EnemyInfoComponent enemyInfo = EntityManager.GetComponentData<EnemyInfoComponent>(collideEntity);
                    if(enemyInfo.health > 1)
                    {
                        int newHealth = enemyInfo.health - 1;
                        EntityManager.SetComponentData(collideEntity, new EnemyInfoComponent { health = newHealth });
                    }
                    else
                    {
                        entityCommandBuffer.DestroyEntity(collideEntity);
                    }
                    Debug.Log("Enemy" + enemyInfo.health);
                }
                entityCommandBuffer.DestroyEntity(missileComponent.ValueRO.thisEntity);
            }
        }
        entityCommandBuffer.Playback(EntityManager);
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
