
using Unity.Physics.Systems;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
public partial struct EnemyMissileCollideSystem : ISystem
{
    internal ComponentDataHandle componentDataHandle;
    internal struct ComponentDataHandle
    {
        public ComponentLookup<MissileComponent> lookUpEnemyMissileComponent;

        public ComponentDataHandle(ref SystemState state)
        {
            lookUpEnemyMissileComponent = state.GetComponentLookup<MissileComponent>();
        }
        public void Update(ref SystemState state)
        {
            lookUpEnemyMissileComponent.Update(ref state);
        }
    }
    private void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerMoveComponent>();
        componentDataHandle = new ComponentDataHandle(ref state);
    }

    [BurstCompile]
    private void OnUpdate(ref SystemState state)
    {      
        componentDataHandle.Update(ref state);

        foreach(RefRO<LocalTransform> localTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerMoveComponent>())
        {
            state.Dependency = new EnemyMissileCollide
            {
                enemyMissileComponent = componentDataHandle.lookUpEnemyMissileComponent,
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        }
    }
}

[BurstCompile]
[WithAll(typeof(PlayerMoveComponent))]
public struct EnemyMissileCollide : ITriggerEventsJob
{
    public ComponentLookup<MissileComponent> enemyMissileComponent;
    private Entity entityA, entityB;
    public void Execute(TriggerEvent triggerEvent)
    {
        entityA = triggerEvent.EntityA;
        entityB = triggerEvent.EntityB;

        if (enemyMissileComponent.HasComponent(entityA))
        {
            enemyMissileComponent.GetRefRW(entityA).ValueRW.collideEntity = entityB;
            enemyMissileComponent.GetRefRW(entityA).ValueRW.isDead = true;       
        }
        else if (enemyMissileComponent.HasComponent(entityB))
        {
            enemyMissileComponent.GetRefRW(entityB).ValueRW.collideEntity = entityA;
            enemyMissileComponent.GetRefRW(entityB).ValueRW.isDead = true;
        }
    }
}
