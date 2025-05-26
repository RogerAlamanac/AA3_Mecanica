using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WaveBase : MonoBehaviour
{
    /// <summary>
    /// Retorna l'alçada de l'aigua a una posició donada.
    /// </summary>
    /// <param name="position">Posició mundial (x, z) on es vol conèixer l'alçada.</param>
    /// <returns>Alçada y de l'aigua a la posició donada.</returns>
    public abstract float GetHeightAt(Vector3 position);
}
