$(document).ready(function () {
    tableProfesional = $('#tablaProfesionales').DataTable(
        {
            "dom": 'ftp'
        }
    );
});
$('#tablaProfesionales tbody').on('click', 'tr', function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
        $('#btnUpdateProfesional').addClass('invisible');
    } else {
        tableProfesional.$('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        var fila = tableProfesional.row('.selected').data();
        $('#btnUpdateProfesional').removeClass('invisible');
    }

    $('#inputUpdateProfesional').val(fila[0]);
});
$('#btnCreateProfesional').on('click', function () {
    $('#inputCreateProfesional').val(fila[0]);
});