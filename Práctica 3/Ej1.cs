monitor BD {

    cond espera;
    int cantConsultas = 0;

    procedure entrar() {
        while (cantConsultas >= 5) wait(espera);
        cantConsultas++;
    }

    procedure salir() {
        cantConsultas--;
        signal(espera);
    }
}


process Persona [i: 0 to N-1] {
    BD.entrar();
    //usarBD();
    delay(1);
    BD.salir();
}
