[System.Serializable]
public class SlimeType
{
    public string nombre;
    public float velocidad;
    public float peso;
    public int vida;

    public SlimeType(string nombre, float velocidad, float peso, int vida)
    {
        this.nombre = nombre;
        this.velocidad = velocidad;
        this.peso = peso;
        this.vida = vida;
    }
}

public static class slime
{
    public static SlimeType normal = new SlimeType("Normal", 5f, 1f, 10);
    public static SlimeType rapido = new SlimeType("Rápido", 8f, 0.5f, 8);
    public static SlimeType pesado = new SlimeType("Pesado", 3f, 3f, 20);
    public static SlimeType tanque = new SlimeType("Tanque", 2f, 5f, 40);
}
