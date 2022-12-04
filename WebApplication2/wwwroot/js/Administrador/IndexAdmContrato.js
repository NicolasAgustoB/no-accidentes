$(document).ready(function () {
    tableContrato = $('#tablaContratos').DataTable(
        {
            "dom": 'ftp',
            columns: [
                { data: "Id" },
                { data: "Estado" },
                { data: "Sucursal" },
                { data: "Empresa" },
                { data: "Fecha Inicio" },
                { data: "Fecha Termino" }
            ],
            columnDefs: [
                { targets: [0], visible: false },
            ]
        }
    );
});
$('#tablaContratos tbody').on('click', 'tr', function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
        $('#btnFinalizeContrato').addClass('invisible');
        $('#btnUpdateContrato').addClass('invisible');
    } else {
        tableContrato.$('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        var fila = tableContrato.row('.selected').data();
        $('#btnUpdateContrato').removeClass('invisible');
        $('#inputUpdateServicio').val(fila["Id"])  
        if (fila["Estado"] == "Activo") {
            $('#btnFinalizeContrato').removeClass('invisible');
            $('#inputContratoFinalize').val(fila["Id"]);
        }
    }

});
$('#btnCreateContrato').on('click', function () {
    $('#inputCreateServicio').val(0);
});
