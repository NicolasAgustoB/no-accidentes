var x = $('#idServiciox').val();
var estado = $('#estadoServiciox').val();
var adicional = $('#adicionalServiciox').val();
var tipoServicio = $('#idTipoServiciox').val();
var cliente = $('#idClientex').val();
var empleado = $('#inputEmpleadoId').val();
var asistentes = $('#asistentes').val();
var materiales = $('#material').val();
var span = document.getElementById('spanHoraInicio');
$(document).ready(function () {
    $('#createServicio').removeClass('invisible');
    tableCliente = $('#tablaClientes').DataTable(
        {
            "dom": 'ftp',
            columns: [
                { data: "Id" },
                { data: "Nombre" },
                { data: "Rut" },
                { data: "Telefono" },
            ]
        }
    );
    $("#inputHoraInicioCreate").prop('disabled', true);
    $("#inputHoraTerminoCreate").prop('disabled', true);
});
$('#chk_Adicional').change(function () {
    if (this.checked) {
        $('#adicionalUpdate').val(1);
    }
    else {
        $('#adicionalUpdate').val(0)
    }
});
$('#adicionalSwitch').change(function () {
    if (this.checked) {
        $('#adicionalCreate').val(1);
    }
    else {
        $('#adicionalCreate').val(0)
    }
});
$('#asistentesCheck').change(function () {
    if (this.checked) {
        $('#inputAsistentesCreate').prop('disabled', false);
    }
    else {
        $('#inputAsistentesCreate').prop('disabled', true);
        $('#inputAsistentesCreate').val("");
    }
});
$('#materialCheck').change(function () {
    if (this.checked) {
        $('#inputMaterialCreate').prop('disabled', false);
    }
    else {
        $('#inputMaterialCreate').prop('disabled', true);
        $('#inputMaterialCreate').val("");
    }
});

$("#comboEmpresaCreate").change(function () {
    $.ajax({
        type: 'POST',
        url: "/Profesional/ReadSucursalDisponible",
        datatype: "application/json",
        data: { IdEmpresa: $("#comboEmpresaCreate").val() },
        success: function (sucursales) {
            $("#comboSucursalCreate").empty();
            $("#comboSucursalCreate").append("<option hidden value=''>Seleccione...</option>")
            $.each(sucursales, function (i, sucursal) {
                $("#comboSucursalCreate").append("<option value='" + sucursal.Id + "'>" + sucursal.Nombre + "</option>")
            });

        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }
    });
    $("#comboSucursalCreate").prop('disabled', false);
    $("#comboSucursalCreate").val(" ");
});
$("#comboSucursalCreate").change(function () {
    $.ajax({
        type: 'POST',
        url: "/Profesional/ReadCliente",
        datatype: "application/json",
        data: { IdSucursal: $("#comboSucursalCreate").val() },
        success: function (clientes) {
            tableCliente.destroy();
            tableCliente = $('#tablaClientes').DataTable({
                "dom": 'ftp',
                data: clientes,
                columns: [
                    { data: "Id" },
                    { data: "Nombre" },
                    { data: "Email" },
                    { data: "Telefono" },
                ]
            });
            $("#tablaClientes").removeAttr("style");
        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }

    });
    $('#divTablaClientes').removeClass('invisible');
});
$('#tablaClientes tbody').on('click', 'tr', function () {

    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
        $('#inputClienteId').val(0);
    } else {
        tableCliente.$('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        var fila = tableCliente.row('.selected').data();
        $('#btnUpdateCliente').removeClass('invisible');
        $('#inputClienteId').val(fila["Id"]);

    }


});

$('#inputFechaCreate').change(function () {
    $("#inputHoraInicioCreate").prop('disabled', false);
    $("#inputHoraTerminoCreate").prop('disabled', true);
    $("#inputHoraInicioCreate").val("");
    $("#inputHoraTerminoCreate").val("");
});

$('#btnCreate').on('click', function () {
    if ($('#inputClienteId').val() == 0) {
        alert("Seleccione un cliente de la tabla");
        return false;
    }
});
$("#inputHoraInicioCreate").change(function () {
    $("#inputHoraTerminoCreate").prop('disabled', false);
});
function callback(mutationsList, observer) {
    console.log('Mutations:', mutationsList)
    console.log('Observer:', observer)
    span.textContent;
    mutationsList.forEach(mutation => {
        if (mutation.attributeName === 'class') {
            if ($('#spanHoraInicio').hasClass('field-validation-error')) {
                $("#inputHoraTerminoCreate").prop('disabled', true);
            }
        }
    })
}


const mutationObserver = new MutationObserver(callback)

mutationObserver.observe(document.getElementById('spanHoraInicio'),
    { attributes: true }
)