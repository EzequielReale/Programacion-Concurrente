/*
En una clínica existe un médico de guardia que recibe continuamente peticiones de
atención de las E enfermeras que trabajan en su piso y de las P personas que llegan a la
clínica ser atendidos.
Cuando una persona necesita que la atiendan espera a lo sumo 5 minutos a que el médico lo
haga, si pasado ese tiempo no lo hace, espera 10 minutos y vuelve a requerir la atención del
médico. Si no es atendida tres veces, se enoja y se retira de la clínica.
Cuando una enfermera requiere la atención del médico, si este no lo atiende inmediatamente
le hace una nota y se la deja en el consultorio para que esta resuelva su pedido en el
momento que pueda (el pedido puede ser que el médico le firme algún papel). Cuando la
petición ha sido recibida por el médico o la nota ha sido dejada en el escritorio, continúa
trabajando y haciendo más peticiones.
El médico atiende los pedidos dándole prioridad a los enfermos que llegan para ser atendidos.
Cuando atiende un pedido, recibe la solicitud y la procesa durante un cierto tiempo. Cuando
está libre aprovecha a procesar las notas dejadas por las enfermeras.
*/

procedure Ej4 is

    task type Enfermera;

    task type Paciente;

    task Medico is
        entry atenderEnfermera(p: out text);
        entry atenderPaciente();
    end Medico;

    task Mesa is
        entry dejarNota(p: in text);
        entry recibirNota(p: out text);
    end Mesa;

    enfermeras: array(1..E) of Enfermera;
    pacientes: array(1..P) of Paciente;


    task body Enfermera is
        papel: text := "Para el medico";
    begin
        loop
            select
                Medico.atenderEnfermera(papel);
            else
                Mesa.dejarNota(papel);
            end select;

            delay(random()); // Suponiendo que sigue haciendo otras cosas
        end loop;
    end Enfermera;


    task body Paciente is
        intentos: integer := 0;
    begin
        while (intentos < 3) loop
            select
                Medico.atenderPaciente();
            or delay(300)
                delay(600);
                intentos += 1;
            end select;
        end loop;
    end Paciente;


    task body Medico is
    begin
        loop
            select
                accept atenderPaciente() do
                    delay(random()); // Lo atiende
                end atenderPaciente;
            or
                when (atenderPaciente´count = 0) =>
                    select
                        accept atenderEnfermera(papel: out text) do
                            papel := firmarPapel(papel);
                        end atenderEnfermera;
                    else
                        Mesa.recibirNota(papel) do
                            papel := firmarPapel(papel);
                        end recibirNota;
                    end select;
            end select;
        end loop;
    end Medico;


    task body Mesa is
        notas : queue of text;
    begin
        loop
            select
                accept dejarNota(papel: in text) do
                    notas.push(papel);
                end dejarNota;
            or
                accept recibirNota(papel: out text) do
                    papel := notas.pop();
                end recibirNota;
            end select;
        end loop;
    end Mesa;


begin
    null;
end Ej4;
