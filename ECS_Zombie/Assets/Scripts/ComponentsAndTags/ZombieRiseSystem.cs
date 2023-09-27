using Unity.Burst;
using Unity.Entities;

[BurstCompile]
[UpdateAfter(typeof(SpawnZombieSystem))]
public partial struct ZombieRiseSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

        new ZombieRiseJob
        {
            DeltaTime = deltaTime,
            ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct ZombieRiseJob : IJobEntity
{
    public float DeltaTime;
    public EntityCommandBuffer.ParallelWriter ecb;

    [BurstCompile]
    private void Execute(ZombieRiseAspect zombie, [ChunkIndexInQuery] int sortKey)
    {
        zombie.Rise(DeltaTime);
        if (!zombie.IsAboveGround) return;

        zombie.SetAtGroundLevel();
        ecb.RemoveComponent<ZombieRiseRate>(sortKey, zombie.Entity);
        ecb.SetComponentEnabled<ZombieWalkProperties>(sortKey, zombie.Entity, true);
    }
}