$(document).ready(function () {
    tableServicio = $('#tablaServicios').DataTable(
        {
            "dom": 'ftp'
        }
    );
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
    $('#inputSelectedServicioRead').val(fila[0])
});