/*
Un banco decide entregar promociones a sus clientes por medio
de su agente de prensa, el cual lo hace de la siguiente manera:
el agente debe entregar 50 premios entre los 1000 clientes,
para esto, obtiene al azar un número de cliente y le entrega el
premio, una vez que este lo toma continúa con la entrega.
NOTAS:
    - Cuando los 50 premios han sido entregados el agente y los
      clientes terminan su ejecución.
    - No se puede utilizar una estructura de tipo arreglo para
      almacenar los premios de los clientes.
*/

sem mutex = 1, esperarResultado = 0, tomarPremio = 0;
int ganador;
Premio premioPublico;

process Cliente [id: 0 to 999] {
    for i = 0 to 49 {
        P(esperarResultado);
        if (ganador == id) {
            Premio miPremio = premioPublico;
            V(tomarPremio);
        }
    }
}

process AgenteDePrensa {
    Premio premio;

    for i = 0 to 49 {
        ganador = random(0, 999);
        premioPublico = premio;
        for i = 0 to 999  V(esperarResultado);
        P(tomarPremio);
    }
}