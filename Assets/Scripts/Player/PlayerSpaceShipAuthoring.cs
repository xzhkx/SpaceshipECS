using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
public class PlayerSpaceShipAuthoring : MonoBehaviour
{
    [SerializeField] private int health;
    private class Baker : Baker<PlayerSpaceShipAuthoring> { 
        public override void Bake(PlayerSpaceShipAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerShootComponent());
            AddComponent(entity, new PlayerInfoComponent
            {
                health = authoring.health
            });
            AddComponent(entity, new PlayerMoveComponent());
        }
    }
}

public struct PlayerInfoComponent : IComponentData
{
    public int health;
}

public struct PlayerMoveComponent : IComponentData
{
    public float2 moveInput;
}

public struct PlayerShootComponent : IComponentData, IEnableableComponent
{

}
