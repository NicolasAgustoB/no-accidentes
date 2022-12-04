var x = $('#idSucursal').val();
var estado = $('#estadoSucursal').val();
var empresa = $('#empresaId').val();
$(document).ready(function () {
    if (x == 0) {
        $('#createSucursal').removeClass('invisible');
    }
    else {
        $('#updateSucursal').removeClass('invisible');
        $('#comboEmpresaUpdate').val(empresa);
        $('#comboEstadoSucursal').val(estado);
    }

    
});