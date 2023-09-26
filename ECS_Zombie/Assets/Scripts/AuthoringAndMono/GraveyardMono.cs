using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class GraveyardMono : MonoBehaviour
{
    public float2 FieldDimensions;
    public int NumerTombsonesToSpawn;
    public GameObject TombstonePrefab;
    public uint RandomSeed;
}

public class GraveyardBaker : Baker<GraveyardMono>
{
    /** GraveyardMono 컴포넌트를 ECS에서 사용할 수 있는 
     *  GraveyardProperties 컴포넌트로 변환
     *  1.0 이상부터 Addcomponent에 Entity를 매개변수로 추가 (graveyardEntity) 
     */
    public override void Bake(GraveyardMono authoring)
    {
        var graveyardEntity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(graveyardEntity, new GraveyardProperties
        {
            FieldDimensions = authoring.FieldDimensions,
            NumberTombstonesToSpawn = authoring.NumerTombsonesToSpawn,
            TombsonePrefab = GetEntity(authoring.TombstonePrefab, TransformUsageFlags.Dynamic)
        });

        AddComponent(graveyardEntity, new GraveyardRandom
        {
            Value = Unity.Mathematics.Random.CreateFromIndex(authoring.RandomSeed)
        });

        AddComponent<ZombieSpawnPoints>();
    }
}