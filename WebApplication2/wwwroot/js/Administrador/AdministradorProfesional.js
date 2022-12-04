var id = $('#idProfesional').val();
var estado = $('#estadoProfesional').val();
$(document).ready(function () {
    if (id == 0) {
        $('#createProfesional').removeClass('invisible');
    }
    else {
        $('#comboEstadoProfesional').val(estado);
        $('#updateProfesional').removeClass('invisible');
    }
});
