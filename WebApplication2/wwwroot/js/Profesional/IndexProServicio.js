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
                { data: "Estado" },
                { data: "Adicional" }
            ],
            columnDefs: [
                { targets: [0], visible: false }
            ]
        }
    );
    $("#tablaServicios").removeAttr("style");
});
$('#tablaServicios tbody').on('click', 'tr', function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
        $('#btnUpgradeServicio').addClass('invisible');
        $('#btnReadServicio').addClass('invisible');
        $('#btnFinishServicio').addClass('invisible');
    } else {
        tableServicio.$('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        var fila = tableServicio.row('.selected').data();

        $('#btnReadServicio').removeClass('invisible');
            $('#btnUpgradeServicio').removeClass('invisible');
            $('#btnReadServicio').removeClass('invisible');
            $('#btnFinishServicio').removeClass('invisible');
       
    }
    $('#inputCreateServicio').val(fila["Id"])
    $('#inputFinishServicio').val(fila["Id"])
    $('#inputReadServicio').val(fila["Id"])
    $('#inputUpgradeServicio').val(fila["Id"])
    
});
$('#btnCreateServicio').on('click', function () {
    $('.inputCreateServicio').val(0)
});
