using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Tablero : MonoBehaviour {
       
    /*
    Lista de cosas pendientes de implementar:
    
    - Crear la transición gráfica cuando se genera un cubo aleatorio (Incremento de escala)
    - Crear las transiciones gráficas cuando fusionamos cubos
    - Crear las transiciones gráficas cuando movemos cubos
    - Usar movimiento con el sonido y con la animación
    - Crear un menú desde el que seleccionar la opción de jugar
    - Actualizar las funciones de movimiento para que utilicen sólo las matrices
    - Crear el generador de cubos  

    */
    //Prototipo del cubo
    public GameObject CuboPrefab;
    //Prototipo del mensaje
    public GameObject mensajeDerrota;

    
    //La instancia del mensaje
    GameObject mensaje;

    //La posición inicial del mensaje
    Vector3 posinicialmensaje = new Vector3(0.5f,0.5f,0);
        

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

    //¿Debe crearse un nuevo cubo en una posicion aleatoria?
    bool debecrearsecubo = true;
    
    
    //Sprites
    Sprite grafcubo1;
    Sprite grafcubo2;
    Sprite grafcubo3;
    Sprite grafcubo6;
    Sprite grafcubo12;
    Sprite grafcubo24;
    Sprite grafcubo48;
    Sprite grafcubo96;
    Sprite grafcubo192;
    Sprite grafcubo384;
    Sprite grafcubo768;
    Sprite grafcubo1536;
    Sprite grafcubo3072;
    Sprite grafcubo6144;
    Sprite grafcubo12288;
    Sprite grafcubo24576;
    Sprite grafcubo49152;
    Sprite grafcubo98304;
    Sprite grafcubo196608;
    Sprite grafcubo393216;
    Sprite grafcubo786432;
    Sprite grafcubo1572864;
    Sprite grafcubo3145728;
    Sprite grafcubo6291456;
    Sprite grafcubo12582912;

    //El cubo que toca crear en la posición aleatoria
    int cuboacrear = 1;

    //Puntuación que vamos a mostrar
    int puntuacion = 0;

    //El campo de texto
    public Text texto;

    //La bandera que señala si se puede mover
    bool puedeMover = true;

    //Animación de que se ha perdido
    bool animacionPerdidoTerminada = false;

    //El botón de reinicio
    public Button botonreinicio;

    //Vector de escala ideal
    Vector3 escalaideal = new Vector3(3,3,0);

    //Escala normal cubo
    Vector3 escalanormalcubo = new UnityEngine.Vector3(1,1,0);

    //Incremento de escala
    Vector3 incrementoescala = new Vector3(0.1f, 0.1f, 0.1f);

    //La lista de cubos cuya animación consiste en ser creados
    List<GameObject> cubosAnimarCreacion = new List<GameObject>();

    //Las booleanas que se utilizan para controlar la animacion
    bool animacioncreacionpendiente;


    // Use this for initialization
    void Start () {

        //Instanciamos el mensaje de derrota en la posición inicial
        mensaje = Instantiate(mensajeDerrota, posinicialmensaje, Quaternion.identity);
        //Lo escalamos para que no se vea
        mensaje.transform.localScale = new Vector3(0, 0, 0);

        //Localizamos el campo de texto
        texto = GameObject.Find("CampoTexto").GetComponent<Text>();
        

        //Localizamos el botón
        botonreinicio = GameObject.Find("BotonReinicio").GetComponent<Button>();
        //Lo invisibilizamos
        botonreinicio.gameObject.SetActive(false);
        //Añadimos el evento
        botonreinicio.onClick.AddListener(reiniciar);


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

        cargaSprites();

        

        //Las booleanas que se utilizan para marcar los principios y los finales de las animaciones
        animacioncreacionpendiente = false;

        //Las listas de animación
        cubosAnimarCreacion = new List<GameObject>();

        //Creamos dos cubos en posiciones aleatorias

        /*
        generarCuboPosicionAleatoria();
        generarCuboPosicionAleatoria();*/


        //Creamos 4 cubos en posiciones no aleatorias

        crearCubo(matrizPosiciones[0][0], primeralinea, 0, 1, "Creacion");
        crearCubo(matrizPosiciones[1][0], segundalinea, 0, 2, "Creacion");
        crearCubo(matrizPosiciones[2][0], terceralinea, 0, 1, "Creacion");
        crearCubo(matrizPosiciones[3][0], cuartalinea, 0, 2, "Creacion");


        //Actualizamos la posición inicial
        actualizapuntuacion(0);


    }

    private void cargaSprites()
    {
        //Inicialización de los sprites
        grafcubo1 = Resources.Load("Cubo 1", typeof(Sprite)) as Sprite;
        grafcubo2 = Resources.Load("Cubo 2", typeof(Sprite)) as Sprite;
        grafcubo3 = Resources.Load("Cubo 3", typeof(Sprite)) as Sprite;
        grafcubo6 = Resources.Load("Cubo 6", typeof(Sprite)) as Sprite;
        grafcubo12 = Resources.Load("Cubo 12", typeof(Sprite)) as Sprite;
        grafcubo24 = Resources.Load("Cubo 24", typeof(Sprite)) as Sprite;
        grafcubo48 = Resources.Load("Cubo 48", typeof(Sprite)) as Sprite;
        grafcubo96 = Resources.Load("Cubo 96", typeof(Sprite)) as Sprite;
        grafcubo192 = Resources.Load("Cubo 192", typeof(Sprite)) as Sprite;
        grafcubo384 = Resources.Load("Cubo 384", typeof(Sprite)) as Sprite;
        grafcubo768 = Resources.Load("Cubo 768", typeof(Sprite)) as Sprite;
        grafcubo1536 = Resources.Load("Cubo 1536", typeof(Sprite)) as Sprite;
        grafcubo3072 = Resources.Load("Cubo 3072", typeof(Sprite)) as Sprite;
        grafcubo6144 = Resources.Load("Cubo 6144", typeof(Sprite)) as Sprite;
        grafcubo12288 = Resources.Load("Cubo 12288", typeof(Sprite)) as Sprite;
        grafcubo24576 = Resources.Load("Cubo 24576", typeof(Sprite)) as Sprite;
        grafcubo49152 = Resources.Load("Cubo 49152", typeof(Sprite)) as Sprite;
        grafcubo98304 = Resources.Load("Cubo 98304", typeof(Sprite)) as Sprite;
        grafcubo196608 = Resources.Load("Cubo 196608", typeof(Sprite)) as Sprite;
        grafcubo393216 = Resources.Load("Cubo 393216", typeof(Sprite)) as Sprite;
        grafcubo786432 = Resources.Load("Cubo 786432", typeof(Sprite)) as Sprite;
        grafcubo1572864 = Resources.Load("Cubo 1572864", typeof(Sprite)) as Sprite;
        grafcubo3145728 = Resources.Load("Cubo 3145728", typeof(Sprite)) as Sprite;
        grafcubo6291456 = Resources.Load("Cubo 6291456", typeof(Sprite)) as Sprite;
        grafcubo12582912 = Resources.Load("Cubo 12582912", typeof(Sprite)) as Sprite;

    }


    //Crea un cubo de unas determinadas características en una posición determinada
    private void crearCubo(Vector2 coordenadas, GameObject[] linea, int posicion, int tipocubo, string tipoanimacion)
    {
        
        //Creamos el cubo
        GameObject cubo = Instantiate(CuboPrefab, coordenadas, Quaternion.identity);
        //Lo asignamos a una posición dentro de una de las líneas
        linea[posicion] = cubo;
        //Establecemos el tipo de cubo que es
        cubo.GetComponent<Cubo>().tipocubo = tipocubo;


        print("Creamos el cubo tipo" + tipocubo);


        //Establecemos la imagen del cubo:
        switch (tipocubo) {
            case 1:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo1;
                break;
            case 2:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo2;
                break;
            case 3:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo3;
                break;
            case 6:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo6;
                break;
            case 12:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo12;
                break;
            case 24:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo24;
                break;
            case 48:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo48;
                break;
            case 96:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo96;
                break;
            case 192:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo192;
                break;
            case 384:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo384;
                break;
            case 768:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo768;
                break;
            case 1536:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo1536;
                break;
            case 3072:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo3072;
                break;
            case 6144:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo6144;
                break;
            case 12288:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo12288;
                break;
            case 24576:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo24576;
                break;
            case 49152:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo49152;
                break;
            case 98304:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo98304;
                break;
            case 196608:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo196608;
                break;
            case 393216:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo393216;
                break;
            case 786432:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo786432;
                break;
            case 1572864:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo1572864;
                break;
            case 3145728:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo3145728;
                break;
            case 6291456:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo6291456;
                break;
            case 12582912:
                cubo.GetComponent<SpriteRenderer>().sprite = grafcubo12582912;
                break;
        }

        if (tipoanimacion.Equals("generacion"))
        {            
            //El cubo empieza a escala cero
            cubo.transform.localScale=new Vector3(0,0,0);
            //Añadimos el cubo a la lista de cubos cuya creación debemos animar
            cubosAnimarCreacion.Add(cubo);
        }
    }

   

    public void generarCuboPosicionAleatoria()
    {
        //Número línea aleatorio
        int numfilaaleatorio = UnityEngine.Random.Range(0,4);
        //Número columna aleatorio
        int numcolumnaaleatorio = UnityEngine.Random.Range(0,4);
        //Si la posición está ocupada, se vuelve a llamar a la función
        if (matrizFilas[numfilaaleatorio][numcolumnaaleatorio]!=null){
            generarCuboPosicionAleatoria();
        }
        //Si la posición no está ocupada, se crea el cubo y se actualiza el valor de cuboacrear
        else
        {
            //Calculamos las coordenadas
            Vector2 coordenadas = matrizPosiciones[numfilaaleatorio][numcolumnaaleatorio];
                        
            //Ponemos la bandera para que no se avance
            animacioncreacionpendiente = true;
            //Creamos el cubo y animamos su generación
            crearCubo(coordenadas, matrizFilas[numfilaaleatorio], numcolumnaaleatorio, cuboacrear, "generacion");

            //Mientras haya una animación pendiente
            
            /*
            while (animacioncreacionpendiente)
            {

            };*/
            
            //Actualizamos el valor de cuboacrear
            if (cuboacrear == 1)
            {
                cuboacrear = 2;
            }
            else
            {
                cuboacrear = 1;
            }
        }
}


    //Código del movimientoDerecha usando getValorNuevoCubo

    public void movimientoDerecha(GameObject[] fila, Vector2[] filaposiciones)
    {

        for (int i = 3; i >= 0; i--)
        {

            if (existeCuboDentroLimiteYPuedeFusionarseConCuboALaIzquierda(fila, i))
            {
                print("Existe cubo dentro del límite y puede fusionarse con el cubo que tiene a la izquierda");
                if (hayEspacioALaDerecha(fila, i)){
                    crearCuboALaDerechaTrasFusionConCuboIzquierdaYActualizarPuntuacion(fila, filaposiciones, i);
                    print("Hay espacio a la derecha");
                }
                else
                {
                    crearCuboEnPosicionTrasFusionConCuboIzquierdaYActualizarPuntuacion(fila, filaposiciones, i);
                    print("No hay espacio a la derecha");
                }
                debecrearsecubo = true;
            }
            else if (existeCuboDentroLimiteQueTieneUnCuboALaIzquierdaPeroNoSonFusionables(fila, i))
            {
                print("Existe cubo dentro del límite que tiene un cubo a la izquierda pero no son fusionables");
                if (hayEspacioALaDerecha(fila, i)){
                    print("Hay espacio a la derecha");
                    mueveCuboAlaDerecha(fila, filaposiciones, i);
                    debecrearsecubo = true;                    
                }
            }
            else if (elCuboExisteYEstaEnLaPrimeraColumna(fila, i)){
                print("El cubo está en la primera columna");
                if(hayEspacioALaDerecha(fila, i)){
                    print("Hay espacio a la derecha");                    
                    mueveCuboAlaDerecha(fila, filaposiciones, i);
                    debecrearsecubo = true;
                }
            }

            else if (existeCuboDentroLimiteQueNoTieneCuboALaIzquierda(fila, i))
            {
                print("Existe un cubo dentro del límite que no tiene un cubo a la izquierda");
                if (hayEspacioALaDerecha(fila, i)){
                    print("Hay espacio a la derecha");
                    mueveCuboAlaDerecha(fila, filaposiciones, i);
                    debecrearsecubo = true;
                }
            }
        }
    }
    
    //Movimiento de derecha a izquierda usando getValorNuevoCubo
    public void movimientoIzquierda(GameObject[] fila, Vector2[] filaposiciones)
    {
        //El recorrido lo hacemos del 0 al 3
        for (int i = 0; i <= 3; i++)
        {
            //Si se encuentra el cubo que vamos a mover
            if (fila[i] != null)
            {
                
                //Si (i+1) no es mayor que tres
                //Si en primeralinea[i+1] también hay un cubo
                if ((i + 1 <= 3) && fila[i + 1] != null)
                {
                    //Si ese cubo puede fusionarse con el cubo de primeralinea[i] 
                    //Caso 1. Caso 2: Uno de los cubos es un 1 y otro de los cubos es un 2

                    if (cuboSumaConCuboTres(fila[i].GetComponent<Cubo>(), fila[i + 1].GetComponent<Cubo>())
                        || cubosSumanMasDeCuatro(fila[i].GetComponent<Cubo>(), fila[i + 1].GetComponent<Cubo>())
                        && cubosSonIguales(fila[i].GetComponent<Cubo>(), fila[i + 1].GetComponent<Cubo>()))

                    {
                        //Obtenemos el valor del cubo que vamos a crear
                        int valorcubo = getValorNuevoCubo(fila[i], fila[i + 1]);
                        //Hacemos desaparecer el cubo que hay en [i-1]

                        destruirCubo(fila[i + 1]);

                        /*
                        Destroy(fila[i + 1]);
                        fila[i + 1] = null;
                        */

                        //Convertimos el cubo que hay en i en un cubo de nivel superior (Nivel 3)
                        //Primero destruimos el cubo

                        destruirCubo(fila[i]);

                        /*
                        Destroy(fila[i]);
                        fila[i] = null;*/


                        //Luego creamos un cubo en la posición i-1 si es posible
                        if (i - 1 >=0)
                        {

                            //Crearíamos un cubo
                            debecrearsecubo = true;

                            //Si no existe otro cubo que impida que se ocupe la posición i+1
                            if (fila[i - 1] == null)
                            {
                                //Creamos el cubo 3
                                crearCubo(filaposiciones[i - 1], fila, i - 1, valorcubo, "movimientoizquierda");

                                //Crearíamos un cubo
                                debecrearsecubo = true;
                            }

                            else
                            {
                                //Creamos el cubo 3
                                crearCubo(filaposiciones[i], fila, i, valorcubo, "nada");

                            }
                          
                        }
                        else if (i - 1 < 0)
                        {
                            //Creamos el cubo 3
                            crearCubo(filaposiciones[i], fila, i, valorcubo, "nada");

                        }

                        //Actualizamos la puntuación
                        actualizapuntuacion(valorcubo);
                    }
                    
                    //Caso 3: Hay un cubo a la derecha pero no son compatibles

                    else
                    {

                        if (i > 0)
                        {
                            //Si el espacio a la izquierda está desocupado
                            if (fila[i - 1] == null)
                            {
                                //Desplazamos el cubo de i a i-1
                                fila[i].transform.position = (filaposiciones[i - 1]);
                                //Cambiamos su posición en la matriz
                                fila[i - 1] = fila[i];
                                fila[i] = null;
                            }
                        }
                    }
                }

                //Si en la fila[i+1] no hay un cubo (Si no se cumple la condición de fusión)
                else
                {                   
                    //Si no está en el extremo del tablero
                    if (i > 0)
                    {
                        //Si el espacio a la izquierda está desocupado
                        if (fila[i - 1] == null)
                        {
                            //Desplazamos el cubo de i a i-1
                            fila[i].transform.position = (filaposiciones[i - 1]);
                            //Cambiamos su posición en la matriz
                            fila[i - 1] = fila[i];
                            fila[i] = null;

                            //Crearíamos un cubo
                            debecrearsecubo = true;
                        }
                    }
                }
            }

            else
            {
               
            }
        }
    }
        
    //Movimiento de abajo arriba usando getValorCubo
    public void movimientoArriba(int numcolumna)
    {
        //El recorrido lo hacemos desde la fila número 0 a la fila número 3
        for (int i = 0; i <= 3; i++)
        {


            //Si se encuentra el cubo que vamos a mover en cada fila dentro de la columna
            if ((matrizFilas[i])[numcolumna] != null)
            {
                //Si (i+1) no es mayor que tres (i indica número de fila)
                //Si en la línea de arriba[i+1] también hay un cubo
                if ((i + 1 <= 3) && matrizFilas[i + 1][numcolumna] != null)
                {
                    //Si ese cubo de la misma columna pero de una línea superior puede fusionarse:
                    //Caso 1\Caso 2: Uno de los cubos es un 1 y otro de los cubos es un 2


                    if (cuboSumaConCuboTres(matrizFilas[i][numcolumna].GetComponent<Cubo>(), matrizFilas[i + 1][numcolumna].GetComponent<Cubo>())
                        || cubosSumanMasDeCuatro(matrizFilas[i][numcolumna].GetComponent<Cubo>(), matrizFilas[i + 1][numcolumna].GetComponent<Cubo>())
                        && cubosSonIguales(matrizFilas[i][numcolumna].GetComponent<Cubo>(), matrizFilas[i + 1][numcolumna].GetComponent<Cubo>()))
                        
                    {
                        //Obtenemos el valor del cubo que vamos a crear
                        int valorcubo = getValorNuevoCubo(matrizFilas[i][numcolumna], matrizFilas[i + 1][numcolumna]);
                        //Hacemos desaparecer el cubo que hay en [i-1]
                        Destroy(matrizFilas[i + 1][numcolumna]);
                        matrizFilas[i + 1][numcolumna] = null;
                        //Convertimos el cubo que hay en i en un cubo de nivel superior (Nivel 3)
                        //Primero destruimos el cubo
                        Destroy(matrizFilas[i][numcolumna]);
                        matrizFilas[i][numcolumna] = null;
                        //Luego creamos un cubo en la posición i-1 si es posible
                        if (i - 1 >= 0)
                        {
                            //Crearíamos un cubo
                            debecrearsecubo = true;

                            //Si no existe otro cubo que impida que se ocupe la posición i-1
                            if (matrizFilas[i - 1][numcolumna] == null)
                            {
                                //Creamos el cubo 3
                                crearCubo(matrizPosiciones[i - 1][numcolumna], matrizFilas[i - 1], numcolumna, valorcubo, "movimientoarriba");

                            }

                            //Si dicho cubo sí existe
                            else
                            {
                                //Creamos el cubo 3
                                crearCubo(matrizPosiciones[i][numcolumna], matrizFilas[i], numcolumna, valorcubo, "nada");

                            }

                        }
                        else if (i - 1 < 0)
                        {
                            //Creamos el cubo 3
                            crearCubo(matrizPosiciones[i][numcolumna], matrizFilas[i], numcolumna, valorcubo, "nada");

                        }
                        //Actualizamos la puntuación
                        actualizapuntuacion(valorcubo);
                    }
                    
                    //Caso 3: Hay un cubo a la arriba pero no son compatibles

                    else
                    {

                        if (i > 0)
                        {
                            //Si el espacio de arriba está desocupado

                            if (matrizFilas[i - 1][numcolumna] == null)
                            {
                                //Desplazamos el cubo de i a i-1
                                matrizFilas[i][numcolumna].transform.position = (matrizPosiciones[i - 1][numcolumna]);
                                //Cambiamos su posición en la matriz
                                matrizFilas[i - 1][numcolumna] = matrizFilas[i][numcolumna];
                                matrizFilas[i][numcolumna] = null;
                                //Crearíamos un cubo
                                debecrearsecubo = true;
                            }
                        }

                    }
                }
                //Si en la fila[i+1] no hay un cubo (Si no se cumple la condición de fusión)
                else
                {
                    

                    //Si no está en el extremo del tablero
                    if (i > 0)
                    {
                        //Si el espacio de arriba está desocupado
                        if (matrizFilas[i - 1][numcolumna] == null)
                        {
                            
                            //Desplazamos el cubo de i a i-1
                            matrizFilas[i][numcolumna].transform.position = (matrizPosiciones[i - 1][numcolumna]);
                            //Cambiamos su posición en la matriz
                            matrizFilas[i - 1][numcolumna] = matrizFilas[i][numcolumna];
                            matrizFilas[i][numcolumna] = null;
                            //Crearíamos un cubo
                            debecrearsecubo = true;

                        }
                    }
                }
            }

            else
            {
               
            }
        }
    }
       
    
    //Movimiento de arriba abajo usando getValorCubo
    public void movimientoAbajo(int numcolumna)
    {
        //El recorrido lo hacemos desde la fila número 3 a la fila número 0
        for (int i = 3; i >= 0; i--)
        {
            //Si se encuentra el cubo que vamos a mover en cada fila dentro de la columna
            if ((matrizFilas[i])[numcolumna] != null)
            {
                //Si (i-1) no es menor que cero (i indica número de fila)
                //Si en la línea de abajo[i-1] también hay un cubo
                if (!estaEnElLimiteIzquierdo(i) && matrizFilas[i - 1][numcolumna] != null)
                {
                    //Si ese cubo de la misma columna pero de una línea inferior puede fusionarse:
                    //Caso 1: Uno de los cubos es un 1 y otro de los cubos es un 2


                    if (cuboSumaConCuboTres(matrizFilas[i][numcolumna].GetComponent<Cubo>(), matrizFilas[i - 1][numcolumna].GetComponent<Cubo>())
                        || cubosSumanMasDeCuatro(matrizFilas[i][numcolumna].GetComponent<Cubo>(), matrizFilas[i - 1][numcolumna].GetComponent<Cubo>())
                        && cubosSonIguales(matrizFilas[i][numcolumna].GetComponent<Cubo>(), matrizFilas[i - 1][numcolumna].GetComponent<Cubo>()))

                    {
                       
                        //Obtenemos el valor del cubo que vamos a crear
                        int valorcubo = getValorNuevoCubo(matrizFilas[i][numcolumna], matrizFilas[i - 1][numcolumna]);
                        //Hacemos desaparecer el cubo que hay en [i-1]

                        destruirCubo(matrizFilas[i - 1][numcolumna]);

                        /*Destroy(matrizFilas[i - 1][numcolumna]);
                        matrizFilas[i - 1][numcolumna] = null;*/

                        //Convertimos el cubo que hay en i en un cubo de nivel superior (Nivel 3)
                        //Primero destruimos el cubo

                        destruirCubo(matrizFilas[i][numcolumna]);

                        /*
                        Destroy(matrizFilas[i][numcolumna]);
                        matrizFilas[i][numcolumna] = null;
                        */

                        //Luego creamos un cubo en la posición i+1 si es posible
                        if (i + 1 <= 3)
                        {
                           
                            //Crearíamos un cubo
                            debecrearsecubo = true;

                            //Si no existe otro cubo que impida que se ocupe la posición i+1
                            if (matrizFilas[i + 1][numcolumna] == null)
                            {
                                //Creamos el cubo 3

                               
                                crearCubo(matrizPosiciones[i + 1][numcolumna], matrizFilas[i + 1], numcolumna, valorcubo, "movimientoabajo");

                            }

                            //Si dicho cubo sí existe
                            else
                            {
                                //Creamos el cubo 3
                                crearCubo(matrizPosiciones[i][numcolumna], matrizFilas[i], numcolumna, valorcubo, "nada");

                            }

                        }
                        else if (i + 1 > 3)
                        {
                            //Creamos el cubo 3
                             crearCubo(matrizPosiciones[i][numcolumna], matrizFilas[i], numcolumna, valorcubo, "nada");

                        }
                        //Actualizamos la puntuación
                        actualizapuntuacion(valorcubo);
                    }
                    
                    //Caso 3: Hay un cubo abajo pero no son compatibles

                    else
                    {
                        if (i + 1 <= 3)
                        {
                            //Si el espacio de abajo está desocupado

                            if (matrizFilas[i + 1][numcolumna] == null)
                            {
                                //Desplazamos el cubo de i a i-1
                                matrizFilas[i][numcolumna].transform.position = (matrizPosiciones[i + 1][numcolumna]);
                                //Cambiamos su posición en la matriz
                                matrizFilas[i + 1][numcolumna] = matrizFilas[i][numcolumna];
                                matrizFilas[i][numcolumna] = null;
                                //Crearíamos un cubo
                                debecrearsecubo = true;
                            }
                        }

                    }


                }
                //Si en la fila[i-1] no hay un cubo (Si no se cumple la condición de fusión)
                else
                {
                   
                    //Si no está en el extremo del tablero
                    if (i + 1 <= 3)
                    {
                        //Si el espacio de abajo está desocupado
                        if (matrizFilas[i + 1][numcolumna] == null)
                        {

                            //Desplazamos el cubo de i a i+1
                            matrizFilas[i][numcolumna].transform.position = (matrizPosiciones[i + 1][numcolumna]);
                            //Cambiamos su posición en la matriz
                            matrizFilas[i + 1][numcolumna] = matrizFilas[i][numcolumna];
                            matrizFilas[i][numcolumna] = null;
                            //Crearíamos un cubo
                            debecrearsecubo = true;
                        }
                    }
                }
            }

            else
            {
                
            }
        }
    }

    
    public bool hayEspacioALaDerecha(GameObject[] fila, int numcolumna)
    {
        return numcolumna + 1 <= 3;
    }

    public void crearCuboALaDerechaTrasFusionConCuboIzquierdaYActualizarPuntuacion(GameObject[] fila, Vector2[] filaposiciones, int columna)
    {
        int valorcubo = obtenerValorFusionCubosYDestruirlos(fila, columna, columna-1);        
        crearCubo(filaposiciones[columna + 1], fila, columna + 1, valorcubo, "movimientoderecha");
        actualizapuntuacion(valorcubo);
    }

    public int obtenerValorFusionCubosYDestruirlos(GameObject[] fila, int columna, int columnaCuboFusion)
    {
        int valorcubo = getValorNuevoCubo(fila[columna], fila[columnaCuboFusion]);
        destruirCubo(fila[columnaCuboFusion]);
        destruirCubo(fila[columna]);
        return valorcubo;
    }

    public void crearCuboEnPosicionTrasFusionConCuboIzquierdaYActualizarPuntuacion(GameObject[] fila, Vector2[] filaposiciones, int columna)
    {
        int valorcubo = obtenerValorFusionCubosYDestruirlos(fila, columna, columna - 1);
        crearCubo(filaposiciones[columna], fila, columna, valorcubo, "nada");
        actualizapuntuacion(valorcubo);
    }

   

    public bool existeCuboDentroLimiteYPuedeFusionarseConCuboALaIzquierda(GameObject[] fila, int numcolumna)
    {
        if (numcolumna - 1 < 0) {
            return false; 
             }
        else {
            GameObject cubo = fila[numcolumna];
            GameObject cuboALaIzquierda = fila[numcolumna - 1];
            return (existeCubo(cubo) && !estaEnElLimiteIzquierdo(numcolumna) && existeCubo(cuboALaIzquierda) && sonFusionablesPorLaIzquierda(cuboALaIzquierda, cubo));
        }
    }

    public bool existeCuboDentroLimiteQueTieneUnCuboALaIzquierdaPeroNoSonFusionables(GameObject[] fila, int numcolumna)
    {
        if (numcolumna - 1 < 0)
        {
            return false;

        }
        else
        {
            GameObject cubo = fila[numcolumna];
            GameObject cuboALaIzquierda = fila[numcolumna - 1];
            return (existeCubo(cubo) && !estaEnElLimiteIzquierdo(numcolumna) && existeCubo(cuboALaIzquierda) && !sonFusionablesPorLaIzquierda(cuboALaIzquierda, cubo));
        }
    }

    public bool elCuboExisteYEstaEnLaPrimeraColumna(GameObject[] fila, int numcolumna)
    {
        GameObject cubo = fila[numcolumna];
        return (existeCubo(cubo) && elCuboEstaEnLaPrimeraColumna(numcolumna));
    }

    public bool elCuboEstaEnLaPrimeraColumna(int numcolumna)
    {
        return numcolumna == 0;
    }

    public bool existeCuboDentroLimiteQueNoTieneCuboALaIzquierda(GameObject[] fila, int numcolumna)
    {
        GameObject cubo = fila[numcolumna];
        return (existeCubo(cubo) && !estaEnElLimiteIzquierdo(numcolumna));
       
    }


    public void mueveCuboAlaDerecha(GameObject[] fila, Vector2[] filaposiciones, int columna)
    {

        Hay que seguir trabajando en este método

        float difX = (fila[columna].transform.position.x - (filaposiciones[columna + 1]).x);
        float fracciondifX = difX / 100;

        for(int i = 0; i< 100; i++)
        {

            float posXActual = fila[columna].transform.position.x;
            float posXDeseada = fila[columna].transform.position.x;
            float diferenciaEntrePosicionXActualyDeseada = Math.Abs(posXActual - posXDeseada);
            
            if (diferenciaEntrePosicionXActualyDeseada<fracciondifX){
                fila[columna].transform.position = (filaposiciones[columna + 1]);
            }
            else
            {
                fila[columna].transform.Translate(new Vector3(-fracciondifX, 0, 0));
            }
        }

        //print("difX ="+ difX);
        //fila[columna].transform.position = (filaposiciones[columna + 1]);

        fila[columna + 1] = fila[columna];
        fila[columna] = null;
        debecrearsecubo = true;
    }

    public bool cuboSumaConCuboTres(Cubo cubo, Cubo cubo2)
    {
        return cubo.GetComponent<Cubo>().tipocubo + cubo2.GetComponent<Cubo>().tipocubo == 3;
    }

    public bool cubosSumanMasDeCuatro(Cubo cubo, Cubo cubo2)
    {
        return cubo.GetComponent<Cubo>().tipocubo + cubo2.GetComponent<Cubo>().tipocubo > 4;
    }

    public bool cubosSonIguales(Cubo cubo, Cubo cubo2)
    {
        return cubo.GetComponent<Cubo>().tipocubo == cubo2.GetComponent<Cubo>().tipocubo;
    }

    public void destruirCubo(GameObject cubo)
    {
        Destroy(cubo);
        cubo = null;
    }

    public bool estaEnElLimiteIzquierdo(int numcolumna)
    {
        return numcolumna - 1 < 0;
    }

    public bool existeCubo(GameObject cubo)
    {
        return cubo != null;
    }

    public bool sonFusionablesPorLaIzquierda(GameObject Cubo1, GameObject Cubo2)
    {
        return cuboSumaConCuboTres(Cubo1.GetComponent<Cubo>(), Cubo2.GetComponent<Cubo>())
                        || cubosSumanMasDeCuatro(Cubo1.GetComponent<Cubo>(), Cubo2.GetComponent<Cubo>())
                        && cubosSonIguales(Cubo1.GetComponent<Cubo>(), Cubo2.GetComponent<Cubo>());
    }

    //Prototipo de método
    //Devuelve el cubo que correspondería crear
    //El propósito es fusionar el código de Caso 1 con el código de Caso 2
    //Este método parte de una comprobación previa por parte del método que lo llama
    //Que tiene que verificar que los cubos son fusionables

    public int getValorNuevoCubo(GameObject cubo1, GameObject cubo2)
    {
        if (cubo1.GetComponent<Cubo>().tipocubo + cubo2.GetComponent<Cubo>().tipocubo == 3)
        {
            return 3;
        }
        else
        {
            return cubo1.GetComponent<Cubo>().tipocubo + cubo2.GetComponent<Cubo>().tipocubo;
        }
        
}


    // Update is called once per frame
    void Update()
    {

        //Si hay animaciones pendientes
        if (animacioncreacionpendiente)
        {
            animarCreacion();

        }


        //Código del movimiento

        //Bandera que activa el movimiento

        if (puedeMover)
        {

            //Detecta el movimiento a la derecha
            if (Input.GetKeyDown(KeyCode.D))
            {
                //Movemos
                movimientoDerecha(matrizFilas[0], posprimeralinea);
                movimientoDerecha(matrizFilas[1], possegundalinea);
                movimientoDerecha(matrizFilas[2], posterceralinea);
                movimientoDerecha(matrizFilas[3], poscuartalinea);

                //Creamos cubo si corresponde
                if (debecrearsecubo)
                {
                    generarCuboPosicionAleatoria();
                    debecrearsecubo = false;
                }

                //Vemos si existen más movimientos
                notienesmasmovimientos();


            }
            //Detecta el movimiento a la izquierda
            else if (Input.GetKeyDown(KeyCode.A))
            {
                //Movemos
                movimientoIzquierda(matrizFilas[0], posprimeralinea);
                movimientoIzquierda(matrizFilas[1], possegundalinea);
                movimientoIzquierda(matrizFilas[2], posterceralinea);
                movimientoIzquierda(matrizFilas[3], poscuartalinea);

                //Creamos cubo si corresponde
                if (debecrearsecubo)
                {
                    generarCuboPosicionAleatoria();
                    debecrearsecubo = false;
                }

                //Vemos si existen más movimientos
                notienesmasmovimientos();
            }

            //Detecta el movimeinto hacia arriba
            else if (Input.GetKeyDown(KeyCode.W))
            {
                //Movemos
                movimientoArriba(0);
                movimientoArriba(1);
                movimientoArriba(2);
                movimientoArriba(3);

                //Creamos el cubo si corresponde
                if (debecrearsecubo)
                {
                    generarCuboPosicionAleatoria();
                    debecrearsecubo = false;
                }

                //Vemos si existen más movimientos
                notienesmasmovimientos();
            }


            //Detecta el movimeinto hacia abajo
            else if (Input.GetKeyDown(KeyCode.S))
            {
                //Movemos
                movimientoAbajo(0);
                movimientoAbajo(1);
                movimientoAbajo(2);
                movimientoAbajo(3);

                //Creamos el cubo si corresponde
                if (debecrearsecubo)
                {
                    generarCuboPosicionAleatoria();
                    debecrearsecubo = false;
                }

                //Vemos si existen más movimientos
                notienesmasmovimientos();

            }

            //Imprime en la consola la matriz
            if (Input.GetKeyDown(KeyCode.I))
            {
                for (int i = 0; i < 4; i++)
                {


                    print(imprimeposicion(matrizFilas[i][0]) +
                        imprimeposicion(matrizFilas[i][1]) +
                        imprimeposicion(matrizFilas[i][2]) +
                        imprimeposicion(matrizFilas[i][3]));


                }

            }
        }

        //Si no se puede mover
        else
        {
           
            //Si la animación no ha terminado y el mensaje no ha llegado a su escala máxima
            if (
                (!animacionPerdidoTerminada)&&                
                (mensaje.transform.localScale.y< escalaideal.y)&&
                (mensaje.transform.localScale.x< escalaideal.x)
                )
            {
                mensaje.transform.localScale = mensaje.transform.localScale + incrementoescala;
            }
            else{

                animacionPerdidoTerminada = true;

            }

            //Si ha completado el movimiento se muestra la puntuación final y se crea el botón de reinicio

            if (animacionPerdidoTerminada)
            {
                botonreinicio.gameObject.SetActive(true);
            }


        }
                
    }


    //Función que se encarga de comprobar si hay animaciones de generación de cubos aleatorias pendientes
    public void animarCreacion()
    {
        //Los cubos que eliminaremos
        List<GameObject> cubosaeliminar = new List<GameObject>();


        //Si la lista de cubos cuya creación estamos animando es mayor que cero
        if (cubosAnimarCreacion.Count > 0)
        {
            foreach (GameObject cubo in cubosAnimarCreacion)
            {

                //Si el cubo se ha escalado más de lo que debía
                if ((cubo.transform.localScale.x > escalanormalcubo.x)
                    || (cubo.transform.localScale.y > escalanormalcubo.y))
                {
                    //Equilibramos la escala y sacamos el cubo de la propia lista
                    cubo.transform.localScale = escalanormalcubo;
                    //Lo incluimos en la lista de cubos que vamos a eliminar
                    cubosaeliminar.Add(cubo);
                }
                //Si el cubo tiene exactamente la misma escala que debería
                else if ((cubo.transform.localScale.x == escalanormalcubo.x)
                   && (cubo.transform.localScale.y == escalanormalcubo.y))
                {
                    //Lo sacamos de la lista
                    cubosaeliminar.Add(cubo);
                }
                //Si el cubo es de menor tamaño del que debiera
                else if ((cubo.transform.localScale.x < escalanormalcubo.x)
                   || (cubo.transform.localScale.y < escalanormalcubo.y))
                {
                    //incrementamos su tamaño
                    cubo.transform.localScale += incrementoescala;
                }

            }
         }
        //Si el tamaño de la lista de cubos que tenemos que reescalar es cero (No hay más cubos)
        //desbloqueamos la bandera
        else
        {
            animacioncreacionpendiente = false;
        }

        //Por cada cubo que esté en cubosaeliminar
        foreach (GameObject cubo in cubosaeliminar)
        {
            if (cubosAnimarCreacion.Contains(cubo))
            {
                cubosAnimarCreacion.Remove(cubo);
            }


        }


    }


    /*
     Prototipo de generador gráfico de cubos

      
     */
     /*
    Sprite generadorCuboGrafico(int numcubo)
    {
    }*/

    public void actualizapuntuacion(int puntos)
    {

        //Instanciamos el texto
        
        this.puntuacion += puntos;
        texto.text  = "Puntuación: "+puntuacion;

    }

    
    public string imprimeposicion(GameObject cubo)
    {
        
        if (cubo==null)
        {
            return "   0   ";
        }
        else
        {

            return "   "+cubo.GetComponent<Cubo>().tipocubo+"   ";

        }
        
    }

    //Esta función nos dice si nos quedan movimientos pendientes
    private bool notienesmasmovimientos()
    {
        
        //Primero comprobamos si existe alguna casilla vacía
        //Si existe, hay movimientos libres, y retornamos que sí existen más movimientos
        for(int i=0; i<=3; i++)
        {
            for(int j=0; j<=3; j++)
            {
                if (matrizFilas[i][j] == null)
                {
                    return false;
                }
            }
        }

        //Si alguno es fusionable, retornamos false
        
        
        for(int i = 0; i<3; i++)
        {
            //Primera fila
            //Primer cubo: Chequeamos abajo y derecha
            //Segundo cubo: Chequeamos abajo y derecha
            //Tercer cubo: Chequeamos abajo y derecha
            //Cuarto cubo: Chequeamos abajo

            //Segunda fila (No hacemos chequeos con la fila de arriba)
            //Primer cubo: Chequeamos abajo y derecha
            //Segundo cubo: Chequeamos abajo y derecha
            //Tercer cubo: Chequeamos abajo y derecha
            //Cuarto cubo: Chequeamos abajo

            //Tercera fila (No hacemos chequeos con la fila de arriba)
            //Primer cubo: Chequeamos abajo y derecha
            //Segundo cubo Chequeamos abajo y derecha
            //Tercer cubo: Chequeamos abajo y derecha
            //Cuarto cubo: Chequeamos abajo

            
        if(   sonfusionables(matrizFilas[i][0], matrizFilas[i+1][0])
           || sonfusionables(matrizFilas[i][0], matrizFilas[i][1])
           || sonfusionables(matrizFilas[i][1], matrizFilas[i + 1][1])
           || sonfusionables(matrizFilas[i][1], matrizFilas[i][2])
           || sonfusionables(matrizFilas[i][2], matrizFilas[i + 1][2])
           || sonfusionables(matrizFilas[i][2], matrizFilas[i][3])
           || sonfusionables(matrizFilas[i][3], matrizFilas[i + 1][3])){

                return false;

            }
        }

        //Cuarta fila
        //Chequeamos el primer cubo con el segundo cubo
        //Chequeamos el segundo cubo con el tercer cubo
        //Chequeamos el tercer cubo con el cuarto cubo

        if ((sonfusionables(matrizFilas[3][0], matrizFilas[3][1]))
          ||(sonfusionables(matrizFilas[3][1], matrizFilas[3][2]))
          ||(sonfusionables(matrizFilas[3][2], matrizFilas[3][3])))
        {
            return false;
        }

        puedeMover = false;
        
        return true;


    }

    //Saber si dos cubos son fusionables
    private bool sonfusionables(GameObject cubo1, GameObject cubo2)
    {
        if ((cubo1.GetComponent<Cubo>().tipocubo + cubo2.GetComponent<Cubo>().tipocubo == 3) ||
           ((cubo1.GetComponent<Cubo>().tipocubo + cubo2.GetComponent<Cubo>().tipocubo > 4)
           && (cubo1.GetComponent<Cubo>().tipocubo == cubo2.GetComponent<Cubo>().tipocubo)))

        {
            return true;
        }

        return false;                 
    }


    public void reiniciar()
    {
        //Puede mover
        puedeMover = true;
        //Termina la animación
        animacionPerdidoTerminada = false;
        //Invisibilizamos el botón de reanudar
        botonreinicio.gameObject.SetActive(false);
        //Devolvemos el mensaje a la escala
        mensaje.transform.localScale = new Vector3(0, 0, 0);
        //Limpiamos el tablero
        for(int i=0; i<=3; i++)
        {
            for(int j=0; j<=3; j++)
            {
                Destroy(matrizFilas[i][j]);
                matrizFilas[i][j] = null;
            }
        }
        //Añadimos los dos primeros cubos
        generarCuboPosicionAleatoria();
        generarCuboPosicionAleatoria();
        //Actualizamos la puntuación
        actualizapuntuacion(-puntuacion);
    }

}

