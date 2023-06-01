/*
Se quiere modelar la cola de un banco que atiende un solo empleado, los clientes llegan y si
esperan más de 10 minutos se retiran
*/

procedure Ej2 is

    task type Cliente;

    task Empleado is
        entry atender();
    end Empleado;

    clientes: array(1..N) of Cliente;


    task body Cliente is
        dinero: double := random(1000, 100000);
    begin
        select
            Empleado.atender(dinero);
        or delay(600)
            null;
        end select;
    end Cliente;


    task body Empleado is
    begin
        loop
            accept atender(dinero: in-out double) do
                dinero := resolver(dinero);
            end atender;
        end loop;
    end Empleado;

end Ej2;
