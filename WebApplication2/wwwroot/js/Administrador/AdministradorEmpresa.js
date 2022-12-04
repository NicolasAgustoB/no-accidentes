var x = $('#idEmpresax').val();
var estado = $('#estadoEmpresax').val();
$(document).ready(function () {
    if (x == 0) {
        $('#createEmpresa').removeClass('invisible');
    }
    else {
        $('#updateEmpresa').removeClass('invisible');
    }

    if (estado == 0) {
        $('#comboEstadoEmpresa').val(0)
    }
    else {
        $('#comboEstadoEmpresa').val(1)
    }
});

