using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WaveBase : MonoBehaviour
{
    /// <summary>
    /// Retorna l'al�ada de l'aigua a una posici� donada.
    /// </summary>
    /// <param name="position">Posici� mundial (x, z) on es vol con�ixer l'al�ada.</param>
    /// <returns>Al�ada y de l'aigua a la posici� donada.</returns>
    public abstract float GetHeightAt(Vector3 position);
}
