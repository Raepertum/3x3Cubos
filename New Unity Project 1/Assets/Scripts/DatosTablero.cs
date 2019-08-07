using UnityEngine;

public class DatosTablero : MonoBehaviour
{

    //Matriz de coordenadas
    Vector2[] posprimeralinea = {new Vector3(-2.5f, 2.53f),new Vector3(-0.86f, 2.53f),
                                 new Vector3(0.86f, 2.53f),new Vector3(2.53f, 2.53f)};
    Vector2[] possegundalinea = {new Vector3(-2.5f, 0.86f),new Vector3(-0.86f, 0.86f),
                                 new Vector3(0.86f, 0.86f),new Vector3(2.53f, 0.86f)};
    Vector2[] posterceralinea = {new Vector3(-2.5f, -0.80f),new Vector3(-0.86f, -0.80f),
                                 new Vector3(0.86f, -0.80f),new Vector3(2.53f, -0.80f)};
    Vector2[] poscuartalinea = {new Vector3(-2.5f, -2.50f),new Vector3(-0.86f, -2.50f),
                                 new Vector3(0.86f, -2.50f),new Vector3(2.53f, -2.50f)};

    //Matriz de las posiciones 
    Vector2[][] matrizPosiciones = new Vector2[4][];

    //Matriz de los cubos: Cada una de las filas de la matriz
    GameObject[] primeralinea = new GameObject[4];
    GameObject[] segundalinea = new GameObject[4];
    GameObject[] terceralinea = new GameObject[4];
    GameObject[] cuartalinea = new GameObject[4];

    //Matriz de todas las filas
    GameObject[][] matrizFilas = new GameObject[4][];


    void Start()
    {
        //Cada fila de la matriz de posiciones es una fila de posiciones
        matrizPosiciones[0] = posprimeralinea;
        matrizPosiciones[1] = possegundalinea;
        matrizPosiciones[2] = posterceralinea;
        matrizPosiciones[3] = poscuartalinea;

        //Cada fila de la matriz de filas es una fila
        matrizFilas[0] = primeralinea;
        matrizFilas[1] = segundalinea;
        matrizFilas[2] = terceralinea;
        matrizFilas[3] = cuartalinea;
        
    }

    public GameObject[] getFila(int numfila)
    {
        return matrizFilas[numfila];
    }

    public GameObject[] getColumna(int numcolumna)
    {
        GameObject[] columna = new GameObject[4];
        columna[0] = primeralinea[0];
        columna[1] = segundalinea[0];
        columna[2] = terceralinea[0];
        columna[3] = cuartalinea[0];
        return columna;
    }

    public Vector2 getCoordenadas(int numfila, int numcolumna)
    {
        return matrizPosiciones[numfila][numcolumna];
    }

    public Vector2[] getFilaCoordenadas(int numfila)
    {
        return matrizPosiciones[numfila];
    }
    public bool esPosicionVacia(int fila, int columna) {
        return (matrizFilas[fila][columna] == null);
    }
    public void llenarPosicion(GameObject cubo, int numfila, int numcolumna)
    {
        matrizFilas[numfila][numcolumna] = cubo;
    }
    public void destruirCubo(int fila, int columna)
    {
        Destroy(matrizFilas[fila][columna]);
        matrizFilas[fila][columna] = null;
    }
}