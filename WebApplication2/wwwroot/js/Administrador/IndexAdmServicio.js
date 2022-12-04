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
        $('#btnUpdateServicio').addClass('invisible');
        $('#btnReadServicio').addClass('invisible');
    } else {
        tableServicio.$('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        var fila = tableServicio.row('.selected').data();
        $('#btnUpdateServicio').removeClass('invisible');
        $('#btnReadServicio').removeClass('invisible');
    }
    $('#inputUpdateServicio').val(fila[0])
    $('#inputReadServicio').val(fila[0])
});
$('#btnCreateServicio').on('click', function () {
    $('.inputCreateServicio').val(0)
});
