//a
DetectorDeMetales (int N) {
    sem mutex = 1;

    process Persona [i=0 to N-1] {
        P(mutex);
        delay(1); //pasa la persona
        V(mutex);
    }
}

//b
DetectorDeMetales (int N) {
    sem mutex = 3; //Se aumenta el semáforo

    process Persona [i=0 to N-1] {
        P(mutex);
        delay(1); //pasa la persona
        V(mutex);
    }
}