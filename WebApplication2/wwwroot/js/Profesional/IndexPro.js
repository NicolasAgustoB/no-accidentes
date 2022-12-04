$(document).ready(function () {
    tableServicio = $('#tablaServicios').DataTable(
        {
            "dom": 'ftp',
            columns: [
                { data: "Id" },
                { data: "Actividad" },
                { data: "Cliente" },
                { data: "EmpresaNombre" },
                { data: "SucursalNombre" },
                { data: "Fecha" },
                { data: "HoraInicio" },
                { data: "HoraTermino" },
                { data: "Estado" }
            ],
            columnDefs: [
                { targets: [0], visible: false },
            ]
        }
    );
    $("#tablaServicios").removeAttr("style");
});
$('#tablaServicios tbody').on('click', 'tr', function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
        $('#btnReadServicio').addClass('invisible');
        $('#btnFinishServicio').addClass('invisible');
    } else {
        tableServicio.$('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        var fila = tableServicio.row('.selected').data();
        $('#btnReadServicio').removeClass('invisible');
        $('#btnFinishServicio').removeClass('invisible');
    }
    $('#inputReadServicio').val(fila["Id"])
    $('#inputFinishServicio').val(fila["Id"])
});
$('#btnCreateServicio').on('click', function () {
    $('#inputReadServicio').val(0)
});
