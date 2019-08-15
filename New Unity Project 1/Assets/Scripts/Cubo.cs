using UnityEngine;

public class Cubo : MonoBehaviour
{

    public GameObject gameobject;
    public int tipocubo;
    public int posicionxdeseada;

    public Cubo()
    {

    }

    public int getPosicionXDeseada()
    {
        return posicionxdeseada;
    }
    public void setPosicionXDeseada(int posx)
    {
        this.posicionxdeseada = posx;
    }

       
}