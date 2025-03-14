using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private int _spawnAmout = 3;
    [SerializeField] private bool _usePool;
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private int _maxCapacity = 50;

    private ObjectPool<Cube> _pool;

    private void Start()
    {
        _pool = new ObjectPool<Cube>(() =>
        {
            return Instantiate(_prefab);
        }, cube =>
        {
            cube.gameObject.SetActive(true);
        }, cube =>
        {
            cube.gameObject.SetActive(false);
        }, cube =>
        {
            Destroy(cube.gameObject);
        }, false, _defaultCapacity, _maxCapacity);

        InvokeRepeating(nameof(Create), 0.2f, 0.2f);
    }

    private void Create()
    {
        for (var i = 0; i < _spawnAmout; i++)
        {
            Cube cube;

            if (_usePool)
            {
                cube = _pool.Get();
            }
            else
            {
                cube = Instantiate(_prefab);
            }

            cube.transform.position = transform.position + Random.insideUnitSphere * 7;
            cube.Init(DestroyCube);
        }
    }

    private void DestroyCube(Cube cube)
    {
        if (_usePool)
        {
            cube.ResetCube();
            _pool.Release(cube);
        }
        else
        {
            Destroy(cube.gameObject);
        }
    }
}
