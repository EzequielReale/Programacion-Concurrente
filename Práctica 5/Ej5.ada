/*
En un sistema para acreditar carreras universitarias, hay UN Servidor que atiende pedidos
de U Usuarios de a uno a la vez y de acuerdo con el orden en que se hacen los pedidos.
Cada usuario trabaja en el documento a presentar, y luego lo envía al servidor; espera la
respuesta de este que le indica si está todo bien o hay algún error. Mientras haya algún error,
vuelve a trabajar con el documento y a enviarlo al servidor. Cuando el servidor le responde
que está todo bien, el usuario se retira. Cuando un usuario envía un pedido espera a lo sumo
2 minutos a que sea recibido por el servidor, pasado ese tiempo espera un minuto y vuelve a
intentarlo (usando el mismo documento).
*/

procedure Ej5 is

    task type Usuario;

    task Servidor is
        entry enviarDocumento(d: in Documento, ok: out boolean);
    end Servidor;

    usuarios: array(1..U) of Usuario;

    task body Usuario is
        documento: Documento := crearDocumento();
        ok : boolean := false; 
    begin
        while (not ok) loop
            select
                Servidor.enviarDocumento(documento, ok);
                if (not ok) then
                    documento := corregirDocumento(documento);
                end if;
            or delay(120)
                delay(60);
            end select;
        end loop;
    end Usuario;

    task body Servidor is
    begin
        loop
            accept enviarDocumento(d: in Documento, ok: out boolean) do
                ok := validarDocumento(documento);
            end enviarDocumento;
        end loop;
    end Servidor;


begin
    null;
end Ej5;