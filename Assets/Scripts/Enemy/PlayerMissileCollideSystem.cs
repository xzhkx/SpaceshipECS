using Unity.Physics.Systems;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
[BurstCompile]
public partial struct PlayerMissileCollideSystem : ISystem
{
    internal ComponentDataHandle componentDataHandle;
    internal struct ComponentDataHandle
    {
        public ComponentLookup<MissileComponent> playerMissileComponent;

        public ComponentDataHandle(ref SystemState state)
        {
            playerMissileComponent = state.GetComponentLookup<MissileComponent>();
        }
        public void Update(ref SystemState state)
        {
            playerMissileComponent.Update(ref state);
        }
    }

    private void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemyMovementComponent>();
        componentDataHandle = new ComponentDataHandle(ref state);
    }

    [BurstCompile]
    private void OnUpdate(ref SystemState state)
    {
        componentDataHandle.Update(ref state);
        
        foreach(RefRW<EnemyMovementComponent> enemyComponent in SystemAPI.Query<RefRW<EnemyMovementComponent>>().WithAll<EnemyMovementComponent>())
        {
            state.Dependency = new PlayerMissileTriggerJob
            {
                playerMissileComponent = componentDataHandle.playerMissileComponent,
                
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        }     
    }
}

[WithAll(typeof(EnemyMovementComponent))]
public struct PlayerMissileTriggerJob : ITriggerEventsJob
{
    public ComponentLookup<MissileComponent> playerMissileComponent;
    private Entity entityA, entityB;
    public void Execute(TriggerEvent triggerEvent)
    {
        entityA = triggerEvent.EntityA;
        entityB = triggerEvent.EntityB;

        if (playerMissileComponent.HasComponent(triggerEvent.EntityA))
        {
            playerMissileComponent.GetRefRW(entityA).ValueRW.collideEntity = entityB; //get the opposite entityt - the player
            playerMissileComponent.GetRefRW(triggerEvent.EntityA).ValueRW.isDead = true;
        }

        else if (playerMissileComponent.HasComponent(triggerEvent.EntityB))
        {
            playerMissileComponent.GetRefRW(entityB).ValueRW.collideEntity = entityA;
            playerMissileComponent.GetRefRW(triggerEvent.EntityB).ValueRW.isDead = true;
        }        
    }
}
