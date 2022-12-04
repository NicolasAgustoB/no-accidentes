$(document).ready(function () {
    tableAccidente = $('#tablaAccidentes').DataTable(
        {
            "dom": 'ftp',
            columns: [
                { data: "Id" },
                { data: "Tipo" },
                { data: "Empresa" },
                { data: "Sucursal" },
                { data: "Cliente" },
                { data: "Accidentados" },
                { data: "Fecha" },
                { data: "Telefono" },
                { data: "Profesional" },
                { data: "Estado" }
            ],
            columnDefs: [
                { targets: [0], visible: false },
            ]
        }
    );
    $("#tablaAccidentes").removeAttr("style");
});
$('#tablaAccidentes tbody').on('click', 'tr', function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
        $('#btnFinishAccidente').addClass('invisible');
        $('#btnReadAccidente').addClass('invisible');
    } else {

        tableAccidente.$('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        var fila = tableAccidente.row('.selected').data();
        if (fila["Estado"] == "Pendiente") {
            $('#btnFinishAccidente').removeClass('invisible');
        }
        else {
            $('#btnFinishAccidente').addClass('invisible');
        }
        $('#btnReadAccidente').removeClass('invisible');
        $('#inputFinishAccidente').val(fila["Id"]);
        $('#inputReadAccidente').val(fila["Id"]);
    }

});
