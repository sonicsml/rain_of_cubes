using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cube : MonoBehaviour
{
    [SerializeField] private int _minLifeTime = 2;
    [SerializeField] private int _maxLifeTime = 5;

    Color defaultColor = Color.white;
    Color collidedColor = Color.red;

    private Renderer _cubeRenderer;

    private bool _hasCollided = false;

    private int _lifeTime;

    private Action<Cube> _destroyAction;

    private void Start()
    {
        _cubeRenderer = GetComponent<Renderer>();
        _lifeTime = Random.Range(_minLifeTime, _maxLifeTime);
    }

    public void Init(Action<Cube> destroyAction)
    {
        _destroyAction = destroyAction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out MeshRenderer Platform))
        {
            _hasCollided = true;
         
            if (_hasCollided)
            {
                StartCoroutine(DestroyAfterDelay());
                _cubeRenderer.material.color = collidedColor;
            }
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(_lifeTime);

        _destroyAction?.Invoke(this);
    }
    public void ResetCube()
    {
        _hasCollided = false;
        _cubeRenderer.material.color = defaultColor;
    }
}
