AlumnosProfesores(int N) {

    int presentes = 0;
    queue entregaron;
    bool corregidos[N]; //En false

    process Push(int id);
    process Pop();
    process EstaVacia();

    process Profesor() [i = 0 to 2] {
        int idActual;
        <await (!entregaron.EstaVacia()); idActual = entregaron.Pop()>
        //Corrige el examen
        corregidos[idActual] = true;
    }

    process Alumno() [i = 0 to N-1] {
        //Llega al examen y espera a arrancar
        <presentes++;>
        <await (presentes == N);>
        //Realiza el examen
        <entregaron.Push(i);>
        <await (corregidos[i]);>
        //Se va
    }
}