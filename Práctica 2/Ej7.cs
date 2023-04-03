FabricaDeVentanas () {

    Pieza marcos[30], vidrios[50];
    sem mutexM = 1, mutexV = 1, capacidadMarcos = 30, capacidadVidrios = 50, tengoMarco = 0, tengoVidrio = 0;

    process Carpintero [id: 0 to 3] { //30 marcos
        while (true) {
            Pieza marco = crearMarco();
            P(capacidadMarcos);
            P(mutexM);
            marcos.push(marco)
            V(mutexM);
            V(tengoMarco);
        }
    }

    process Vidriero { //50 vidrios
        while (true) {
            Pieza vidrio = crearVidrio();
            P(capacidadVidrios);
            P(mutexV);
            vidrios.push(vidrio);
            V(mutexV);
            V(tengoVidrio);
        }
    }

    process Armador [id: 0 to 1] {
        while (true) {
            P(tengoMarco);
            P(mutexM);
            Pieza marco = marcos.pop();
            V(mutexM);
            V(capacidadMarcos);

            P(tengovidrio);
            P(mutexV);
            Pieza vidrio = vidrios.pop();
            V(mutexV);
            V(capacidadVidrios);
            
            Ventana ventana = crearVentana(marco, vidrio);
        }
    }
}