monitor Equipo [id: 0 to 4] {

    int jugadoresEquipo = 0, cancha;
    cond equipoCompleto;

    procedure esperarEquipo(idCancha: out int) {
        jugadoresEquipo++;
        if (jugadoresEquipo < 5) wait(equipoCompleto);
        else { 
            Predio.elegirCancha(cancha);
            signal_all(equipoCompleto);
        }
        idCancha = cancha;
    }
}

monitor Predio {

    int orden = 0;

    procedure elegirCancha(idCancha: out int) {
        if (orden < 2) idCancha = 0;
        else idCancha = 1;
        orden++;
    }
}

monitor Cancha [id: 0 to 2] {

    int jugadores = 0;
    cond finPartido;

    procedure jugarPartido() {
        jugadores++;
        if (jugadores < 10) wait(finPartido);
        else {
            delay(50 minutos);
            signal_all(finPartido);
        }
    }
}

process Jugador [id: 0 to 19] {
    int idCancha, idEquipo = DarEquipo();
    Equipo[idEquipo].esperarEquipo(idCancha);
    Cancha[idCancha].jugarPartido();
}