/*
Hay un sistema de reconocimiento de huellas dactilares de la policía que tiene 8 Servidores
para realizar el reconocimiento, cada uno de ellos trabajando con una Base de Datos propia;
a su vez hay un Especialista que utiliza indefinidamente. El sistema funciona de la siguiente
manera: el Especialista toma una imagen de una huella (TEST) y se la envía a los servidores
para que cada uno de ellos le devuelva el código y el valor de similitud de la huella que más
se asemeja a TEST en su BD; al final del procesamiento, el especialista debe conocer el
código de la huella con mayor valor de similitud entre las devueltas por los 8 servidores.
Cuando ha terminado de procesar una huella comienza nuevamente todo el ciclo. Nota:
suponga que existe una función Buscar(test, código, valor) que utiliza cada Servidor donde
recibe como parámetro de entrada la huella test, y devuelve como parámetros de salida el
código y el valor de similitud de la huella más parecida a test en la BD correspondiente.
Maximizar la concurrencia y no generar demora innecesaria.
*/

procedure Ej7 is

    task type Servidor is
        entry RecibirNumero(numero: in Integer);
        entry BuscarHuella(test: in Huella);
    end Servidor;

    task type BD is
        entry RecibirNumero(numero: in Integer);
        entry BuscarHuella(test: in Huella, codigo: in-out Integer, valor: in-out Double);
    end BD;

    task Especialista is
        entry DevolverHuella(codigo: in Integer, valor: in Integer);
    end Especialista;


    servidores: array(1..8) of Servidor;

    BDS: array(1..8) of BD;


    task body Servidor is
        id, codigo: Integer;
        test: Huella;
        valor: Double;
    begin
        accept RecibirNumero(numero: in Integer) do
            id := numero;
        end RecibirNumero;
        
        loop
            accept BuscarHuella(parTest: in Huella) do
                test := parTest;
            end BuscarHuella;

            BDS(id).BuscarHuella(test, codigo, valor);

            Especialista.DevolverHuella(codigo, valor);
        end loop;
    end Servidor;


    task body BD is
        id: Integer;
    begin
        accept RecibirNumero(numero: in Integer) do
            id := numero;
        end RecibirNumero;
        
        loop
            accept BuscarHuella(test: in Huella, codigo: in-out Integer, valor: in-out Double) do
                buscarEnLaBD(test, codigo, valor);
            end BuscarHuella;
        end loop;
    end BD;


    task body Especialista is
        test: Huella;
        codigo, maxCodigo: Integer;
        valor, maxValor: Double;
    begin
        loop
            test := tomarHuella();
            maxValor := -1;
            maxCodigo := -1;

            for i:= 1..8 loop
                servidores(i).Buscar(test);
            end loop;

            for i:= 1..8 loop
                accept DevolverHuella(codigo: in Integer, valor: in Integer);
                if (valor > maxValor) then
                        maxValor := valor;
                        maxCodigo := codigo;
                end if;
            end loop;

            hacerAlgoConElMaximo(maxCodigo, maxValor);
        end loop;
    end Especialista;


begin
    for i:= 1..8 loop
        BDS(i).RecibirNumero(i);
        servidores(i).RecibirNumero(i);
    end loop;
end Ej7;