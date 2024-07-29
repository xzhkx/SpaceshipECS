
using Unity.Entities;
using UnityEngine;

public class SpawnMissileAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject missilePrefab, enemyMissilePrefab;
    private class Baker : Baker<SpawnMissileAuthoring>
    {
        public override void Bake(SpawnMissileAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
           
            AddComponent(entity, new SpawnMissileComponent
            {
                missileEntity = GetEntity(authoring.missilePrefab, TransformUsageFlags.Dynamic),   
                enemyMissileEntity = GetEntity(authoring.enemyMissilePrefab, TransformUsageFlags.Dynamic),
            });
        }
    }
}

public struct SpawnMissileComponent : IComponentData
{
    public Entity missileEntity, enemyMissileEntity;
    
}
