using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;

public partial struct SpawnEnemyShipSystem : ISystem
{
    private SpawnEnemyShipComponent spawnEnemyShip;
    private float timeBetweenSpawn;
    private float currentTime;
    private void OnCreate(ref SystemState state)
    {
        currentTime = 0f;
        timeBetweenSpawn = 3f;
    }

    private void OnStartRunning(ref SystemState state)
    {
        //spawnEnemyShip = SystemAPI.GetSingleton<SpawnEnemyShipComponent>();
    }

    private void OnUpdate(ref SystemState state)
    { 
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(state.WorldUpdateAllocator);

        int randomIndex = Random.Range(0, spawnEnemyShip.enemyShipArray.Length);

        //entityCommandBuffer.Instantiate(spawnEnemyShip.enemyShipArray[randomIndex]

    }
}
