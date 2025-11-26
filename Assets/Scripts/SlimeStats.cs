using UnityEngine;

public enum TipoSlime
{
    Normal,
    Rapido,
    Pesado,
    Tanque
}

public class SlimeStats : MonoBehaviour
{
    public TipoSlime tipoSeleccionado;
    public SlimeType tipo; // esta se llena automáticamente

    void Start()
    {
        switch (tipoSeleccionado)
        {
            case TipoSlime.Normal:
                tipo = slime.normal;
                break;

            case TipoSlime.Rapido:
                tipo = slime.rapido;
                break;

            case TipoSlime.Pesado:
                tipo = slime.pesado;
                break;

            case TipoSlime.Tanque:
                tipo = slime.tanque;
                break;
        }

        Debug.Log($"{name} es un slime {tipo.nombre} con velocidad {tipo.velocidad}");
    }
}
