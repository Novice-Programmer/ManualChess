using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public static Shake shake { set; get; }

    private void Start()
    {
        shake = this;
    }

    public void ShakeTarget(Transform _tr,float _range,float _force)
    {
        Collider[] colls = Physics.OverlapSphere(_tr.position, _range, 1 << 21);

        foreach(var coll in colls)
        {
            var _rb = coll.GetComponent<Rigidbody>();
            _rb.AddExplosionForce(_force, _tr.position, _range, 30.0f);
        }
    }

    public void ShakeKing(Transform _tr,int playerLayerNum)
    {
        Collider[] colls = Physics.OverlapSphere(_tr.position, 1000.0f, 1 << 21);
        Collider[] colls2 = Physics.OverlapSphere(_tr.position, 1000.0f, 1 << playerLayerNum);

        foreach (var coll in colls)
        {
            var _rb = coll.GetComponent<Rigidbody>();
            _rb.mass = 1.0f;
            if (_rb.isKinematic)
            {
                _rb.isKinematic = false;
            }
            _rb.AddExplosionForce(1000.0f, _tr.position, 1000.0f, 300.0f);
        }
        foreach (var coll in colls2)
        {
            var _rb = coll.GetComponent<Rigidbody>();
            _rb.mass = 0.1f;
            _rb.isKinematic = false;
            _rb.AddExplosionForce(1000.0f, _tr.position, 1000.0f, 30.0f);
        }
    }

    private void ShakeObject(Vector3 pos)
    {

    }
}
