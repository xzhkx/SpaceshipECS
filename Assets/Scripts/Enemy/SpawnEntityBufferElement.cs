using UnityEngine;
using Unity.Entities;

public class SpawnEntityBufferElement : MonoBehaviour
{
    [SerializeField] private GameObject[] entityShip;
    private class Baker : Baker<SpawnEntityBufferElement>
    {
        public override void Bake(SpawnEntityBufferElement authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            DynamicBuffer<EnemyShipBuffer> dynamicBuffer = AddBuffer<EnemyShipBuffer>(entity);
            for(int i = 0; i < authoring.entityShip.Length; i++)
            {
                Entity enemy = GetEntity(authoring.entityShip[i], TransformUsageFlags.Dynamic);
                dynamicBuffer.Add(new EnemyShipBuffer { enemyShipEntity = enemy });
            }
        }
    }
    
}

[InternalBufferCapacity(4)]
public struct EnemyShipBuffer : IBufferElementData
{
    public Entity enemyShipEntity;
}


