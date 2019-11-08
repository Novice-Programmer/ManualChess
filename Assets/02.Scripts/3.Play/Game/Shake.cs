using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public static Shake Instance { set; get; }

    private void Start()
    {
        Instance = this;
    }

    public void ShakeTarget(Vector3 _pos, float _range, float _force)
    {
        Collider[] colls = Physics.OverlapSphere(_pos, _range, 1 << 21);

        foreach (var coll in colls)
        {
            var _rb = coll.GetComponent<Rigidbody>();
            _rb.AddExplosionForce(_force, _pos, _range, 30.0f);
        }
    }

    public void ShakeKing(Vector3 _pos, int playerLayerNum)
    {
        Collider[] colls = Physics.OverlapSphere(_pos, 1000.0f, 1 << 21);
        Collider[] colls2 = Physics.OverlapSphere(_pos, 1000.0f, 1 << playerLayerNum);

        foreach (var coll in colls)
        {
            var _rb = coll.GetComponent<Rigidbody>();
            _rb.mass = 1.0f;
            if (_rb.isKinematic)
            {
                _rb.isKinematic = false;
            }
            _rb.AddExplosionForce(1000.0f, _pos, 1000.0f, 300.0f);
        }

        if (GameManager.Instance.playerLayerNum == playerLayerNum)
        {
            foreach (var coll in colls2)
            {
                var _rb = coll.GetComponent<Rigidbody>();
                var _bx = coll.GetComponent<BoxCollider>();
                _rb.mass = 0.1f;
                _rb.isKinematic = false;
                _bx.isTrigger = false;
                _rb.AddExplosionForce(1000.0f, _pos, 1000.0f, 30.0f);
            }
        }
    }

    private void ShakeObject(Vector3 pos)
    {

    }
}
