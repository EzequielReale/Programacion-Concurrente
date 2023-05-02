/*
Resolver con monitores la siguiente situación. En la guardia de traumatología
de un hospital trabajan 5 médicos y una enfermera. A la guardia acuden P
Pacientes que al llegar se dirigen a la enfermera para que le indique a que
médico se debe dirigir y cuál es su gravedad (entero entre 1 y 10). Cuando
tiene estos datos se dirige al médico correspondiente y espera hasta que lo
termine de atender para retirarse. Cada médico atiende a sus pacientes en
orden de acuerdo a la gravedad de cada uno.
Nota: maximizar la concurrencia.
*/

monitor Enfermera {
    
    procedure hablarConEnfermera(medico: out int, gravedad: out int) {
        medico = asignarMedico(0, 4);
        gravedad = asignarGravedad(1, 10);
    }
}

monitor Fila [id: 0 to 4] {

    cond hayPaciente, turno[P];
    sortedQueue cola;

    procedure llegar(idP: in int, gravedad: in int) {
        push(cola, idP, gravedad);
        signal(hayPaciente);
        wait(turno[idP]);
    }

    procedure siguiente() {
        int idP, gravedad;
        
        if (empty(cola)) wait(hayPaciente);
        pop(cola, idP, gravedad);
        signal(turno[idP]);
    }
}

monitor Traumatologia [id: 0 to 4] {

    bool llegue;
    cond esperaMedico, esperaPaciente;

    procedure atenderse() {
        llegue = true;
        signal(esperaMedico);
        wait(esperaPaciente);
    }

    procedure atenderAPaciente() {
        if (!llegue) wait(esperaMedico);
        atenderPaciente();
        signal(esperaPaciente);
        llegue = false;
    }
}


process Paciente [id: 0 to P-1] {
    int medico, gravedad;

    Enfermera.hablarConEnfermera(medico, gravedad);
    Fila[medico].llegar(id, gravedad);
    Traumatologia[medico].atenderse();
}

process Medico [id: 0 to 4] {
    while (true) {
        Fila[id].siguiente();
        Traumatologia[id].atenderAPaciente();
    }
}
