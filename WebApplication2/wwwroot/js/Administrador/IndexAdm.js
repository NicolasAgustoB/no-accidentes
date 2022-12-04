$(document).ready(function () {
    tableServicio = $('#tablaServicios').DataTable(
        {
            "dom": 'ftp',
            columns: [
                { data: "Id" },
                { data: "Actividad" },
                { data: "Profesional" },
                { data: "Cliente" },
                { data: "Empresa" },
                { data: "Sucursal" },
                { data: "Fecha" },
                { data: "Inicio" },
                { data: "Termino" },
                { data: "Estado" }

            ]
        }
    );
    $('#tablaServicios').removeAttr("style");
});
$('#tablaServicios tbody').on('click', 'tr', function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
        $('#btnReadServicio').addClass('invisible');
    } else {
        tableServicio.$('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        var fila = tableServicio.row('.selected').data();
        $('#btnReadServicio').removeClass('invisible');
    }
    $('#inputSelectedServicioRead').val(fila["Id"])
});