$(document).ready(function () {
    tableServicio = $('#tablaPagos').DataTable(
        {
            "dom": 'ftp',
            columns: [
                { data: "Id" },
                { data: "NombreEmpresa" },
                { data: "NombreSucursal" },
                { data: "FechaCreación" },
                { data: "FechaPago" },
                { data: "FechaLimite" },
                { data: "MontoTotal" },
                { data: "Estado" }
            ]
        }
    );
});
$('#tablaPagos tbody').on('click', 'tr', function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
        $('#btnReadPago').addClass('invisible');
        $('#btnAtraso').addClass('invisible');
    } else {
        tableServicio.$('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        var fila = tableServicio.row('.selected').data();
        $('#btnReadPago').removeClass('invisible');

        if (fila["Estado"] == "Atrasado") {
            $('#btnAtraso').removeClass('invisible');
        }
        else {
            $('#btnAtraso').addClass('invisible');
        }
    }
    
    $('#inputSelectedContratoId').val(fila["Id"]);
    $('#inputSelectedPagoRead').val(fila["Id"]);
    $('#inputSelectedPago').val(fila["Id"]);
});

$('#btnConfirmarPago').on('click', function () {
    var metodo;
    metodo = prompt("Metodo de Pago: ");
    $('#inputMetodoPago').val(metodo);
});