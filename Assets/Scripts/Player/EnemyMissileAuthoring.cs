
using Unity.Entities;
using UnityEngine;

public class EnemyMissileAuthoring : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private class Baker : Baker<EnemyMissileAuthoring>
    {
        public override void Bake(EnemyMissileAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemyMissileComponent { moveSpeed = authoring.moveSpeed });
        }
    }
}

public struct EnemyMissileComponent : IComponentData
{
    public float moveSpeed;
}
