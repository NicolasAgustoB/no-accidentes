$(document).ready(function () {
    tableSolicitud = $('#tablaSolicitudes').DataTable(
        {
            "dom": 'ftp',
            columns: [
                { data: "Id" },
                { data: "TipoSolicitud" },
                { data: "Empleado" },
                { data: "Cliente" },
                { data: "FechaInicio" },
                { data: "FechaTermino" },
                { data: "Situacion" },
            ],
            columnDefs: [
                { targets: [0], visible: false },
            ]
        }
    );
    $("#tablaSolicitudes").removeAttr("style");
});
$('#tablaSolicitudes tbody').on('click', 'tr', function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
        $('#btnFinishSolicitud').addClass('invisible');
        $('#btnReadSolicitud').addClass('invisible');
    } else {

        tableSolicitud.$('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        var fila = tableSolicitud.row('.selected').data();
        if (fila["Situacion"] == "Pendiente") {
            $('#btnFinishSolicitud').removeClass('invisible');
        }
        else {
            $('#btnFinishSolicitud').addClass('invisible');
        }
        $('#btnReadSolicitud').removeClass('invisible');
        $('#inputFinishSolicitud').val(fila["Id"]);
        $('#inputReadSolicitud').val(fila["Id"]);
    }

});
