using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public partial struct SpawnZombieSystem : ISystem
{

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnDestory(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        var ecbSington = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

        new SpawnZombieJob
        {
            DeltaTime = deltaTime,
            ecb = ecbSington.CreateCommandBuffer(state.WorldUnmanaged)
        }.Run();
    }
}

[BurstCompile]
public partial struct SpawnZombieJob : IJobEntity
{
    public float DeltaTime;
    public EntityCommandBuffer ecb;

    private void Execute(GraveyardAspect graveyard)
    {
        graveyard.ZombieSpawnTimer -= DeltaTime;
        if (!graveyard.TimeToSpawnZombie) return;
        if (!graveyard.ZombieSpawnPointInitialized()) return;

        graveyard.ZombieSpawnTimer = graveyard.ZombieSpawnRate;
        var newZombie = ecb.Instantiate(graveyard.ZombiePrefab);

        var newZombieTransform = graveyard.GetZombieSpawnPoint();
        ecb.SetComponent(newZombie, newZombieTransform);

        var zombieHeading = MathHelpers.GetHeading(newZombieTransform.Position, graveyard.Position);
        ecb.SetComponent(newZombie, new ZombieHeading { Value = zombieHeading });
    }
}
