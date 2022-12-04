var x = $('#idContratox').val();
var estado = $('#estadoContratox').val();
var sucursal = $('#idContratoSucursalx').val();
var cuerpo = $('#cuerpo').val();
var valor = $('#valor').val();
$(document).ready(function () {
    if (x == 0) {
        $('#createContrato').removeClass('invisible');
    }
    else {
        
        $('#comboUpdateSucursalId').val(sucursal);
        $('#comboEstadoContrato').val(estado);
        $('#updateContrato').removeClass('invisible');
        $('#inputValorUpdate').val(valor);
        $('#inputCuerpoUpdate').val(cuerpo);
        $('#comboSucursalUpdate').val(sucursal);
    }

});
$('#inputFechaCreate').click(function () {
});