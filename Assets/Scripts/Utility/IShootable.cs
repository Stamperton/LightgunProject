using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootable
{
    void OnGetHit(Vector3 hitPoint);
}
