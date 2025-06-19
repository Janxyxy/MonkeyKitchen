using System;
using UnityEngine;

public interface IHasProgress
{
    event Action<float> OnProgressChanged;
}
