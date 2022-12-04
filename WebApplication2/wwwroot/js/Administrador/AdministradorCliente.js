var x = $('#idCliente').val();
var estado = $('#estadoCliente').val();
var sucursalCliente = $('#sucursalIdCliente').val();
var empresaCliente = $('#empresaIdCliente').val();
$(document).ready(function () {
    if (x == 0 && sucursalCliente == 0) {
        $('#createCliente').removeClass('invisible');
    }
    else if (x != 0) {
        $('#updateCliente').removeClass('invisible');
        $("#comboEmpresaUpdate").val(empresaCliente);
        $.ajax({
            type: 'POST',
            url: "/Administrador/ReadSucursal",
            datatype: "application/json",
            data: { IdEmpresa: $("#comboEmpresaUpdate").val() },
            success: function (sucursales) {
                $("#comboSucursalUpdate").empty();
                document.getElementById('comboSucursalUpdate').hidden = false;
                $.each(sucursales, function (i, sucursal) {
                    $("#comboSucursalUpdate").append("<option value='" + sucursal.Id + "'>" + sucursal.Nombre + "</option>")
                });

                $("#comboSucursalUpdate").val(sucursalCliente);
            },

            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }
        });
    }
    else {
        $('#createCliente').removeClass('invisible');
        $("#comboEmpresaCreate").val(empresaCliente);
        $.ajax({
            type: 'POST',
            url: "/Administrador/ReadSucursal",
            datatype: "application/json",
            data: { IdEmpresa: $("#comboEmpresaCreate").val() },
            success: function (sucursales) {
                $("#comboSucursalCreate").empty();
                document.getElementById('comboSucursalCreate').hidden = false;
                $.each(sucursales, function (i, sucursal) {
                    $("#comboSucursalCreate").append("<option value='" + sucursal.Id + "'>" + sucursal.Nombre + "</option>")
                });
                $("#comboSucursalCreate").val(sucursalCliente);
            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }
        });
        
        
    }

    $('#comboEstadoCliente').val(estado);
});

$("#comboEmpresaCreate").change(function () {
    $.ajax({
        type: 'POST',
        url: "/Administrador/ReadSucursal",
        datatype: "application/json",
        data: { IdEmpresa: $("#comboEmpresaCreate").val() },
        success: function (sucursales) {
            $("#comboSucursalCreate").empty();
            document.getElementById('comboSucursalCreate').hidden = false;
            $.each(sucursales, function (i, sucursal) {
                $("#comboSucursalCreate").append("<option value='" + sucursal.Id + "'>" + sucursal.Nombre + "</option>")
            });
        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }
    });
});
$("#comboEmpresaUpdate").change(function () {
    $.ajax({
        type: 'POST',
        url: "/Administrador/ReadSucursal",
        datatype: "application/json",
        data: { IdEmpresa: $("#comboEmpresaUpdate").val() },
        success: function (sucursales) {
            $("#comboSucursalUpdate").empty();
            document.getElementById('comboSucursalUpdate').hidden = false;
            $.each(sucursales, function (i, sucursal) {
                $("#comboSucursalUpdate").append("<option value='" + sucursal.Id + "'>" + sucursal.Nombre + "</option>")
            });
        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }
    });
});