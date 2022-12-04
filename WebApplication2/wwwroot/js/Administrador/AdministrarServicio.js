var x = $('#idServiciox').val();
var estado = $('#estadoServiciox').val();
var adicional = $('#adicionalServiciox').val();
var tipoServicio = $('#idTipoServiciox').val();
var cliente = $('#idClientex').val();
var empleado = $('#idEmpleadox').val();
var asistentes = $('#asistentes').val();
var materiales = $('#material').val();
$(document).ready(function () {

    if (x == 0) {
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
    }
    else {
        $('#updateServicio').removeClass('invisible');
        $('#comboTipoServicioId').val(tipoServicio);
        $('#comboEstadoServicio').val(estado);
        $('#comboEmpleadoId').val(empleado);
        $('#comboClienteId').val(cliente);
        if (asistentes > 0) {
            document.getElementById("asistentesCheckUpdate").checked = true;
            $('#inputAsistentesUpdate').prop('disabled', false);
            $('#inputAsistentesUpdate').val(asistentes);
        }
        else {
            document.getElementById("asistentesCheckUpdate").checked = false;
        }
        if (materiales == "N/A" || materiales == null) {
            document.getElementById("materialCheckUpdate").checked = false;
        }
        else {
            document.getElementById("materialCheckUpdate").checked = true;
            $('#inputMaterialUpdate').prop('disabled', false);
            $('#inputMaterialUpdate').val(materiales);
        }
        if (adicional == 1) {
            document.getElementById("chk_Adicional").checked = true;
            $('#adicionalUpdate').val(1);

        }
        else {
            document.getElementById("chk_Adicional").checked = false;
            $('#adicionalUpdate').val(0);
        }
    }
    $("#inputHoraInicioCreate").prop('disabled', true);
    $("#inputHoraTerminoCreate").prop('disabled', true);
    $("#comboEmpleadoIdCreate").prop('disabled', true);
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
$('#asistentesCheckUpdate').change(function () {
    if (this.checked) {
        $('#inputAsistentesUpdate').prop('disabled', false);
        $('#inputAsistentesUpdate').val(asistentes);
    }
    else {
        $('#inputAsistentesUpdate').prop('disabled', true);
        $('#inputAsistentesUpdate').val("");
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
$('#materialCheckUpdate').change(function () {
    if (this.checked) {
        $('#inputMaterialUpdate').prop('disabled', false);
        $('#inputMaterialUpdate').val(materiales);
    }
    else {
        $('#inputMaterialUpdate').prop('disabled', true);
        $('#inputMaterialUpdate').val("");
    }
});

$("#comboEmpresaCreate").change(function () {
    $.ajax({
        type: 'POST',
        url: "/Administrador/ReadSucursalDisponible",
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
        url: "/Administrador/ReadCliente",
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
    $("#comboEmpleadoIdCreate").prop('disabled', true);
    $("#inputHoraInicioCreate").val("");
    $("#inputHoraTerminoCreate").val("");
});
$('#inputFechaUpdate').change(function () {
    $("#inputHoraInicioUpdate").prop('disabled', false);
    $("#inputHoraTerminoUpdate").prop('disabled', true);
    $("#comboEmpleadoIdUpdate").empty();
    $("#comboEmpleadoIdUpdate").prop('disabled', true);
    $("#inputHoraInicioUpdate").val("");
    $("#inputHoraTerminoUpdate").val("");
});
$('#inputHoraInicioCreate').change(function () {
    $("#inputHoraTerminoCreate").prop('disabled', false);
    $("#comboEmpleadoIdCreate").prop('disabled', false);
    $.ajax({
        type: 'POST',
        url: "/Administrador/ReadEmpleado",
        datatype: "application/json",
        data: { Fecha: $('#inputFechaCreate').val(), HoraInicio: $('#inputHoraInicioCreate').val() },
        success: function (usuarios) {
            $("#comboEmpleadoIdCreate").empty();
            $("#comboEmpleadoIdCreate").append("<option hidden value=''>Seleccione...</option>")
            $.each(usuarios, function (i, usuario) {
                $("#comboEmpleadoIdCreate").append("<option value='" + usuario.Id + "'>" + usuario.Nombre + "</option>")
            });
        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }
    });
});
$('#inputHoraInicioUpdate').change(function () {
    $("#inputHoraTerminoUpdate").prop('disabled', false);
    $("#comboEmpleadoIdUpdate").prop('disabled', false);
    $.ajax({
        type: 'POST',
        url: "/Administrador/ReadEmpleado",
        datatype: "application/json",
        data: { Fecha: $('#inputFechaUpdate').val(), HoraInicio: $('#inputHoraInicioUpdate').val() },
        success: function (usuarios) {
            $("#comboEmpleadoIdUpdate").empty();
            $("#comboEmpleadoIdUpdate").append("<option hidden value=''>Seleccione...</option>")
            $.each(usuarios, function (i, usuario) {
                $("#comboEmpleadoIdUpdate").append("<option value='" + usuario.Id + "'>" + usuario.Nombre + "</option>")
            });
        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }
    });
});
$('#btnCreate').on('click', function () {
    if ($('#inputClienteId').val() == 0) {
        alert("Seleccione un cliente de la tabla");
        return false;
    }
});