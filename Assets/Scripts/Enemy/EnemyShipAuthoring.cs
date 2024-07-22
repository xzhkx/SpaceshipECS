using Unity.Entities;
using UnityEngine;

public class EnemyShipAuthoring : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private int health;
    private class Baker : Baker<EnemyShipAuthoring>
    {     
        public override void Bake(EnemyShipAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemyMovementComponent
            {
                moveSpeed = authoring.moveSpeed,
                thisEntity = entity,
                isDead = false
            });
            AddComponent(entity, new EnemyInfoComponent
            {
                health = authoring.health,
            });
        }
    }
}

public struct EnemyInfoComponent : IComponentData
{
    public int health;
}
public struct EnemyMovementComponent : IComponentData
{
    public bool isDead;
    public float moveSpeed;
    public Entity thisEntity;
}
