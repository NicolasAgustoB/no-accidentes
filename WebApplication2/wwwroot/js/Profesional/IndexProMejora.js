$(document).ready(function () {
    tableMejora = $('#tablaMejoras').DataTable(
        {
            "dom": 'ftp',
            columns: [
                { data: "Id" },
                { data: "Nombre" },
                { data: "Descripcion" },
                { data: "Situacion" },
            ],
            columnDefs: [
                { targets: [0], visible: false },
            ]
        }
    );
    $("#tablaMejoras").removeAttr("style");
});
$('#tablaMejoras tbody').on('click', 'tr', function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
        $('#btnUpgradeServicio').addClass('invisible');
        $('#btnReadServicio').addClass('invisible');
        $('#divCreate').removeClass('invisible');
        $('#divFinish').addClass('invisible');
    } else {
        tableMejora.$('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        var fila = tableMejora.row('.selected').data();

        $('#btnReadServicio').removeClass('invisible');
            $('#btnUpgradeServicio').removeClass('invisible');
            $('#btnReadServicio').removeClass('invisible');
            $('#divCreate').addClass('invisible');
            $('#divFinish').removeClass('invisible');
 
       
    }
    $('#mejoraId').val(fila["Id"])
    $('#inputReadServicio').val(fila["Id"])

});

