/*
Se dispone de un sistema compuesto por 1 central y 2 procesos. Los procesos envían
señales a la central. La central comienza su ejecución tomando una señal del proceso 1,
luego toma aleatoriamente señales de cualquiera de los dos indefinidamente. Al recibir una
señal de proceso 2, recibe señales del mismo proceso durante 3 minutos.
El proceso 1 envía una señal que es considerada vieja (se deshecha) si en 2 minutos no fue
recibida.
El proceso 2 envía una señal, si no es recibida en ese instante espera 1 minuto y vuelve a
mandarla (no se deshecha).
*/

procedure Ej3 is

    task Proceso1;

    task Proceso2;

    task Central is
        entry recibirSeñal1;
        entry recibirSeñal2;
        entry recibirTimeout;
    end Central;

    task Contador is
        entry contar;
    end Contador;


    task body Proceso1 is
    begin
        loop
            select 
                Central.recibirSeñal1();
            or delay(120)
                null;
            end select;
        end loop;
    end Proceso1;


    task body Proceso2 is
    recibido : boolean := false;
    begin
        loop
            while (not recibido) loop
                select
                    Central.recibirSeñal2();
                    recibido := true;
                or delay(60)
                    null;
                end select;
                recibido := false;
            end loop;
        end loop;
    end Proceso2;


    task body Central is
        timeout : boolean := false;
    begin
        accept recibirSeñal1();
        loop
            select
                accept recibirSeñal1();
            or
                accept recibirSeñal2() do
                    Contador.contar();
                    while (not timeout) loop
                        select
                            when (recibirTimeout´count = 0) => accept recibirSeñal2();
                        or
                            accept recibirTimeout() do
                                timeout := true;
                            end recibirTimeout;
                        end select;
                    end loop;
                    timeout := false;
                end recibirSeñal2;
            end select;
        end loop;
    end Central;


    task body Contador is
    begin
        accept contar();
        delay(180);
        Central.recibirTimeout();
    end Contador;


begin
    null;
end Ej3;
