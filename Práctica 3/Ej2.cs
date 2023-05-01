// a) Implemente una solución suponiendo que existe una única fotocopiadora compartida
// por todas las personas, y las mismas la deben usar de a una persona a la vez, sin
// importar el orden. Existe una función Fotocopiar() que simula el uso de la fotocopiadora.

monitor Fotocopiadora {

    procedure entrarAFotocopiar() {
        Fotocopiar();
    }
}

process Persona [id: 0 to N-1] {
    Fotocopiadora.entrarAFotocopiar();
}

// b) Modifique la solución de (a) para el caso en que se deba respetar el orden de llegada.

monitor Fotocopiadora {

    cond cola;
    bool libre = true;
    int esperando = 0;

    procedure entrarAFotocopiar() {
        if (!libre) {
            esperando++;
            wait(cola);
        }
        else libre = false;
    }

    procedure salir() {
        esperando--;
        if (esperando > 0) {
            signal(cola);
        }
        else libre = true;
    }
}

process Persona [id: 0 to N-1] {
    Fotocopiadora.entrarAFotocopiar();
    Fotocopiar();
    Fotocopiadora.salir();
}

// c) Modifique la solución de (b) para el caso en que se deba dar prioridad de acuerdo a la
// edad de cada persona (cuando la fotocopiadora está libre la debe usar la persona de
// mayor edad entre las que estén esperando para usarla).

monitor Fotocopiadora {

    cond cola[N];
    listaOrdenada fila;
    bool libre = true;
    int esperando = 0;

    procedure entrarAFotocopiar(id, edad: in int) {
        if (!libre) {
            esperando++;
            push(fila, id, edad);
            wait(cola[id]);
        }
        else libre = false;
    }

    procedure salir() {
        int id;
        
        esperando--;
        if (esperando > 0) {
            pop(fila, id);
            signal(cola[id]);
        }
        else libre = true;
    }
}

process Persona [id: 0 to N-1] {
    int edad = Random(100);

    Fotocopiadora.entrarAFotocopiar(id, edad);
    Fotocopiar();
    Fotocopiadora.salir();
}

// d) Modifique la solución de (a) para el caso en que se deba respetar estrictamente el orden
// dado por el identificador del proceso (la persona X no puede usar la fotocopiadora
// hasta que no haya terminado de usarla la persona X-1).

monitor Fotocopiadora {

    int siguiente = 0;
    cond cola[N];

    procedure entrarAFotocopiar(id: in int) {
        if (siguiente != id) wait cola[id]; 
    }

    procedure salir() {        
        if (siguiente < N-1) siguiente++;
        else siguiente = 0;
        
        signal cola[siguiente]; 
    }
}

process Persona [id: 0 to N-1] {
    Fotocopiadora.entrarAFotocopiar(id);
    Fotocopiar();
    Fotocopiadora.salir();
}

// e) Modifique la solución de (b) para el caso en que además haya un Empleado que le indica
// a cada persona cuando debe usar la fotocopiadora.

monitor Fotocopiadora {

    cond cola, llegoCliente, terminado;
    int esperando = 0;
    bool empleadoListo = false;

    procedure pedirEntrar() {
        esperando++;
        signal(llegoCliente);
        wait(cola);
    }

    procedure permitirAcceso() {
        if (esperando == 0) {
            wait(llegoCliente);
        }
        esperando--;
        signal(cola);
        wait(terminado);
    }

    procedure salir() {
        signal(terminado);
    }
}

process Empleado {
    while (true) Fotocopiadora.permitirAcceso();
}

process Persona [id: 0 to N-1] {
    Fotocopiadora.pedirEntrar();
    Fotocopiar();
    Fotocopiadora.salir();
}

// f) Modificar la solución (e) para el caso en que sean 10 fotocopiadoras. El empleado le
// indica a la persona cuando puede usar una fotocopiadora, y cual debe usar.

monitor Fotocopiadora {

    cond terminado, cola, llegoCliente, terminado;
    bool impresorasLibres[10] = ([10] , true);
    int esperando = 0, idImpresora;


    procedure pedirEntrar(idImp: out int) {
        esperando++;
        signal(llegoCliente);
        wait(cola);
        idImp = idImpresora;
    }

    procedure permitirAcceso() {
        int i = 0;
        
        if (esperando == 0) wait(llegoCliente);
        esperando--;
        
        while (impresorasLibres[i] == false) {
            i++;
            if (i == 10) {
                i = 0;
                wait(terminado);
            }
        }
        idImpresora = i;
        impresorasLibres[i] = false;

        signal(cola);
    }

    procedure salir(idImp: in int) {
        impresorasLibres[idImp] = true;
        signal(terminado);
    }
}


process Empleado {
    while (true) Fotocopiadora.permitirAcceso();
}

process Persona [id: 0 to N-1] {
    int idImp;

    Fotocopiadora.pedirEntrar(idImp);
    Fotocopiar();
    Fotocopiadora.salir(idImp);
}
