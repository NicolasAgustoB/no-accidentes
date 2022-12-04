$(document).ready(function () {
    tableCliente = $('#tablaClientes').DataTable(
        {
            "dom": 'ftp'
        }
    );
    $("#tablaClientes").removeAttr("style");
});
$('#tablaClientes tbody').on('click', 'tr', function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
    } else {
        tableCliente.$('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        var fila = tableCliente.row('.selected').data();
    }
});