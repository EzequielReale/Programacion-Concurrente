monitor Carrera {

    cond espera;
    int corredoresLlegaron = 0;

    procedure EsperarInicio(totalCorredores: in int) {
        if (corredoresLlegaron < totalCorredores) {
            corredoresLlegaron++;
            wait(espera);
        } 
        else signal_all(espera); 
    }
}

monitor Expendedora {

    cond cola, noHayBotellas, yaHayBotellas;
    int botellas = 20, esperando = 0;
    bool libre = true;

    procedure HacerCola() {
        if (!libre) {
            esperando++;
            wait(cola);
        }
        else libre = false;

        if (botellas == 0) {
            signal(noHayBotellas);
            wait(yaHayBotellas);
        }
    }

    procedure Proximo() {
        botellas--;
        esperando--;
        if (esperando > 0) signal(cola);
        else libre = true;
    }

    procedure ReponerStock() {
        wait(noHayBotellas);
        botellas = 20;
    }

    procedure AvisarDeStock() {
        signal(yaHayBotellas);
    }
}

process Corredor [int id: 0 to c-1] {
    int corredores = c;
    Carrera.EsperarInicio(corredores);
    delay(30 minutos); //Corre la maratón
    Expendedora.HacerCola();
    delay(1 minuto);
    //TomarBotella();
    Expendedora.Proximo();
}

process Repositor {
    while (true) {
        Expendedora.ReponerStock();
        //ReponerBotellas();
        Expendedora.AvisarDeStock();
    }
}
