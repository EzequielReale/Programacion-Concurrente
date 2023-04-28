/*
Si valorás tu salud mental, no hagas este ejercicio. Yo te avisé.

Resolver con SEMÁFOROS el siguiente problema. En una empresa hay UN
Coordinador y 30 Empleados que formarán 3 grupos de 10 empleados cada uno.
Cada grupo trabaja en una sección diferente y debe realizar 345 unidades de un
producto. Cada empleado al llegar se dirige al coordinador para que le indique el
número de grupo al que pertenece y una vez que conoce este dato comienza a trabajar
hasta que se han terminado de hacer las 345 unidades correspondientes al grupo (cada
unidad es hecha por un único empleado). Al terminar de hacer las 345 unidades los
10 empleados del grupo se deben juntar para retirarse todos juntos. El coordinador
debe atender a los empleados de acuerdo al orden de llegada para darle el número de
grupo (a los 10 primeros que lleguen se le asigna el grupo 1, a los 10 del medio el 2,
y a los 10 últimos el 3). Cuando todos los grupos terminaron de trabajar el
coordinador debe informar (imprimir en pantalla) el empleado que más unidades ha
realizado (si hubiese más de uno con la misma cantidad máxima debe informarlos a
todos ellos).
NOTA: maximizar la concurrencia; suponga que existe una función Generar() que simula
la elaboración de una unidad de un producto.
*/

sem mutexCola = 1, mutexUnidades[3] = ([3], 1], numeroLeido = 0, esperaLlegada[30] = ([30] , 0), hayEmpleado = 0, terminaronTodos = 0, esperarIrse = 0;
int idGrupo = 0, unidadesRealizadasEmpleado[30] = ([30] , 0]), unidadesRealizadasGrupo[3] = ([3] , 0), finJornada = 0;
queue empleados;

process Empleado [id: 0 to 29] {
    int idG;
    Unidad unidad;
    
    P(mutexCola);
    push(empleados, id);
    V(mutexCola);
    V(hayEmpleado);
    P(esperaLlegada[id]);
    idG = idGrupo;
    V(numeroLeido);
    
    P(mutexUnidades[idG]);
    while (unidadesRealizadasGrupo[idG] < 345) {
        unidadesRealizadasGrupo[idG]++;
        V(mutexUnidades[idG]);
        unidad = Generar();
        unidadesRealizadasEmpleado[id]++;
        P(mutexUnidades[idG]);
    }
    V(mutexUnidades[idG]);
    
    finJornada++;
    if (finJornada == 30) V(terminaronTodos);
    P(esperarIrse);
    //se retira
}

process Coordinador {
    int id, asignadosAGrupo, max = -1;
    queue ganadores;
    
    for i = 0 to 29 {
        P(hayEmpleado);
        P(mutexCola);
        pop(empleados, id);
        V(mutexCola);
        asignadosAGrupo++;
        if (asignadosAGrupo == 10) {
            idGrupo++;
            asignadosAGrupo = 0;
        }
        V(esperaLlegada[id]);
        P(numeroLeido);
    }
    
    P(terminaronTodos);
    for i = 0 to 29 {
        if (unidadesRealizadasEmpleado[i] == max) {
            push(ganadores, i);
        }
        else if (unidadesRealizadasEmpleado[i] > max) {
            max = unidadesRealizadasEmpleado[i];
            ganadores = new Queue();
            push(ganadores, i);
        }
    }
    while (!empty(ganadores)) {
        pop(ganadores, id);
        print("El empleado con id %d tendrá un premio por productividad", id);
    }
    
    for i = 0 to 29 V(esperarIrse);
}
