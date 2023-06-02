/*
En una playa hay 5 equipos de 4 personas cada uno (en total son 20 personas donde cada
una conoce previamente a que equipo pertenece). Cuando las personas van llegando
esperan con los de su equipo hasta que el mismo esté completo (hayan llegado los 4
integrantes), a partir de ese momento el equipo comienza a jugar. El juego consiste en que
cada integrante del grupo junta 15 monedas de a una en una playa (las monedas pueden ser
de 1, 2 o 5 pesos) y se suman los montos de las 60 monedas conseguidas en el grupo. Al
finalizar cada persona debe conocer el grupo que más dinero junto. Nota: maximizar la
concurrencia. Suponga que para simular la búsqueda de una moneda por parte de una
persona existe una función Moneda() que retorna el valor de la moneda encontrada.
*/

procedure Ej6 is

    task type Equipo is
        entry recibirNumero(id: in Integer);
        entry llegue(id: in Integer);
        entry sumarMonedas(monedas: in queue);
    end Equipo;

    task type Persona is
        entry recibirNumero(id: in Integer);
        entry empezar();
        entry ganador(id: in Integer);
    end Persona;

    task Referi is
        entry totalMonedas(monto: in Double, id: in Integer);
    end Referi;

    equipos: array(1..5) of Equipo;
    personas: array(1..20) of Persona;


    task body Equipo is
        id: Integer;
        monto: Double := 0;
        integrantes: queue of Integer;
    begin
        accept recibirNumero(myId: in Integer) do
            id := myId;
        end recibirNumero;

        while (integrantes.length() < 4) loop
            accept llegue(idP: in Integer) do
                integrantes.push(idP);
            end llegue;
        end loop;

        for i:= 1..4 loop
            personas(integrantes.pop()).empezar();
        end loop;

        for i:= 1..4 loop
            accept sumarMonedas(monedas: in queue) do
                while (not monedas.empty()) loop
                    monto += monedas.pop();
                end loop;
            end sumarMonedas;
        end loop;

        Referi.totalMonedas(monto, id);
    end Equipo;


    task body Persona is
        id, idE, idGanador: Integer;
        monedas: queue of Double;
    begin
        idE := getEquipo();
        accept recibirNumero(myId: in Integer) do
            id := myId;
        end recibirNumero;

        equipos(idE).llegue(id);

        accept empezar();

        for i:= 1..15 loop
            monedas.push(Moneda());
        end loop;

        equipos(idE).sumarMonedas(monedas);

        accept ganador(idG: in Integer) do
            idGanador := idG;
        end ganador;
    end Persona;


    task body Referi is
        maxMonto: Double := -1;
        montoActual: Double;
        maxId, idAct: Integer;
    begin
        for i := 1..4 loop
            accept totalMonedas(monto: in Double, id: in Integer) do
                montoActual := monto;
                idAct := id;
            end totalMonedas;
            
            if (montoActual > maxMonto) then
                maxMonto := montoActual;
                maxId := idAct;
            end if;
        end loop;

        for i := 1..20 loop
            personas(i).ganador(maxId);
        end loop;
    end Referi;


begin
    for i := 1..20 loop
        personas(i).recibirNumero(i);
    end loop;

    for i := 1..5 loop
        equipos(i).recibirNumero(i);
    end loop;
end Ej6;