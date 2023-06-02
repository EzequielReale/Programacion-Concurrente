/*
Una empresa de limpieza se encarga de recolectar residuos en una ciudad por medio de 3
camiones. Hay P personas que hacen continuos reclamos hasta que uno de los camiones
pase por su casa. Cada persona hace un reclamo, espera a lo sumo 15 minutos a que llegue
un camión y si no vuelve a hacer el reclamo y a esperar a lo sumo 15 minutos a que llegue
un camión y así sucesivamente hasta que el camión llegue y recolecte los residuos; en ese
momento deja de hacer reclamos y se va. Cuando un camión está libre la empresa lo envía a
la casa de la persona que más reclamos ha hecho sin ser atendido. Nota: maximizar la
concurrencia.
*/

procedure Ej8 is

    task type Camion;

    task type Persona is
        entry RecibirNumero(myId: in Integer);
        entry RecibirCamion();
    end Persona;

    task Empresa is
        entry RecibirReclamo(idP: in Integer);
        entry PedidoTrabajo(idP: out Integer);
    end Empresa;

    camiones: array(1..3) of Camion;
    personas: array(1..P) of Persona;


    task body Empresa is
        reclamos: array(1..P) of Integer;
        despachados: array(1..P) of Boolean := ((P) false);
        idCam, idPer: Integer;
    begin
        for i := 1 to P
            reclamos(i) := 0;
        end for;

        loop
            select
                accept RecibirReclamo(idP: in Integer) do
                    if (not despachados(idP)) then // Por si el camión ya fue despachado y la persona reclama de nuevo porque justo fue el timeout
                        reclamos(idP) += 1;
                    end if;
                end RecibirReclamo;
            or
                when (RecibirReclamo´Count > 0)
                    accept PedidoTrabajo(idP: out Integer) do
                        idP := maxIndex(reclamos);
                        reclamos(idP) := 0;
                        despachados(idP) := true;
                    end PedidoTrabajo;
            end select;
        end loop;
    end Empresa;


    task body Camion is
        idP: Integer;
    begin
        loop
            Empresa.PedidoTrabajo(idP);

            personas(idP).RecibirCamion();
            delay(random()); // Tiempo que tarda en recolectar los residuos
        end loop;
    end Camion;


    task body Persona is
        id: Integer;
        atendido: Boolean := false;
    begin
        accept RecibirNumero(myId: in Integer) do
            id := myId;
        end RecibirNumero;

        while (not atendido) loop
            Empresa.RecibirReclamo(id);
            
            select
                accept RecibirCamion() do
                    atendido := true;
                end RecibirCamion;
            or delay(900)
                null;
            end select;
        end loop;
    end Persona;


begin
    for i := 1 to P
        personas(i).RecibirNumero(i);
    end for;
end Ej8;