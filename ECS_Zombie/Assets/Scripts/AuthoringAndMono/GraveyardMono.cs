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
    /** GraveyardMono ������Ʈ�� ECS���� ����� �� �ִ� 
     *  GraveyardProperties ������Ʈ�� ��ȯ
     *  1.0 �̻���� Addcomponent�� Entity�� �Ű������� �߰� (graveyardEntity) 
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