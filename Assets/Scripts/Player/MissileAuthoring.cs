using UnityEngine;
using Unity.Entities;

public class MissileAuthoring : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private class Baker : Baker<MissileAuthoring>
    {
        public override void Bake(MissileAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MissileComponent { 
                moveSpeed = authoring.moveSpeed,
                isDead = false,
                thisEntity = entity
            });
        }
    }
}

public struct MissileComponent : IComponentData
{
    public bool isDead;
    public float moveSpeed;
    public Entity thisEntity, collideEntity;
}
