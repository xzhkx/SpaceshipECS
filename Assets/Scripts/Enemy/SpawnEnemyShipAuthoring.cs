using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class SpawnEnemyShipAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyShipArray;
    private Entity[] enemyShipEntities;

    private class Baker : Baker<SpawnEnemyShipAuthoring>{
        public override void Bake(SpawnEnemyShipAuthoring authoring)
        {
            authoring.enemyShipEntities = new Entity[4];
            for (int i = 0; i < authoring.enemyShipArray.Length; i++)
            {
                Entity entity = GetEntity(authoring.enemyShipArray[i], TransformUsageFlags.Dynamic);
                authoring.enemyShipEntities[i] = entity;
            }
            Entity spawnEntity = GetEntity(TransformUsageFlags.None);
            //dynamic buffer
        }
    }
}

public struct SpawnEnemyShipComponent : IComponentData
{
    public Entity[] enemyShipArray;
}
