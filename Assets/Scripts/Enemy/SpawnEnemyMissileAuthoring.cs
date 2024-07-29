
using Unity.Entities;
using UnityEngine;

public class SpawnEnemyMissileAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject enemyMissile;
    private class Baker : Baker<SpawnEnemyMissileAuthoring>
    {
        public override void Bake(SpawnEnemyMissileAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            Entity spawnEntity = GetEntity(authoring.enemyMissile, TransformUsageFlags.Dynamic);
            DynamicBuffer<SpawnEnemyBuffer> dynamicBuffer = AddBuffer<SpawnEnemyBuffer>(entity);
            dynamicBuffer.Add(new SpawnEnemyBuffer
            {
                enemyMissileEntity = spawnEntity,
            });
        }
    }
}
[ChunkSerializable]
public struct SpawnEnemyBuffer : IBufferElementData
{
    public Entity enemyMissileEntity;
}
