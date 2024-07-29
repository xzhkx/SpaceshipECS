using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;

public partial struct SpawnEnemyShipSystem : ISystem
{
    private float timeBetweenSpawn;
    private float currentTime;
    private void OnCreate(ref SystemState state)
    {
        currentTime = 0f;
        timeBetweenSpawn = 6f;
    }

    private void OnStartRunning(ref SystemState state)
    {
        foreach (DynamicBuffer<EnemyShipBuffer> enemyShipBuffer in SystemAPI.Query<DynamicBuffer<EnemyShipBuffer>>())
        {
            EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
            Entity enemy = enemyShipBuffer[UnityEngine.Random.Range(0, 3)].enemyShipEntity;
            entityCommandBuffer.Instantiate(enemy);
            entityCommandBuffer.SetComponent(enemy, new LocalTransform
            {
                Position = new float3(UnityEngine.Random.Range(-40, 30), 3, 66),
                Rotation = quaternion.identity,
                Scale = 1
            });
        }
    }

    private void OnUpdate(ref SystemState state)
    {
        currentTime += SystemAPI.Time.DeltaTime;
        if (currentTime < timeBetweenSpawn) return;
        currentTime = 0f;

        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);

        foreach(DynamicBuffer<EnemyShipBuffer> enemyShipBuffer in SystemAPI.Query<DynamicBuffer<EnemyShipBuffer>>())
        {
            Entity enemy = enemyShipBuffer[UnityEngine.Random.Range(0, 3)].enemyShipEntity;
            entityCommandBuffer.Instantiate(enemy);
            entityCommandBuffer.SetComponent(enemy, new LocalTransform
            {
                Position = new float3(UnityEngine.Random.Range(-40, 30), 3, 66),
                Rotation = quaternion.identity,
                Scale = 1
            });
        }

        entityCommandBuffer.Playback(state.EntityManager);
    }
}
