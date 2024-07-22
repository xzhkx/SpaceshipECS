using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerTagAuthoring : MonoBehaviour
{
    private class Baker : Baker<PlayerTagAuthoring>
    {
        public override void Bake(PlayerTagAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PlayerTagComponent());
        }
    }
}

public struct PlayerTagComponent : IComponentData
{

}
