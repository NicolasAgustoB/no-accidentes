$(document).ready(function () {
    tableEmpresa = $('#tablaEmpresas').DataTable(
        {
            "dom": 'ftp',
            columns: [
                { data: "Id" },
                { data: "Nombre" },
                { data: "Rubro" },
                { data: "Direccion" },
                { data: "Telefono" },
                { data: "Estado" }
            ]

        }
    );
    tableSucursal = $('#tablaSucursales').DataTable(
        {
            "dom": 'ftp',
            columns: [
                { data: "Id" },
                { data: "Nombre" },
                { data: "Trabajadores" },
                { data: "Direccion" },
                { data: "Telefono" },
                { data: "Estado" }
            ]
        }
    );
    tableCliente = $('#tablaClientes').DataTable(
        {
            "dom": 'ftp',
            columns: [
                { data: "Id" },
                { data: "Nombre" },
                { data: "Rut" },
                { data: "Email" },
                { data: "Telefono" },
            ]
        }
    );
    $('#inputCreateSucursal').val(0);
    $('#inputCreateCliente').val(0);
    $('#inputCreateEmpresa').val(0);
});
$('#tablaEmpresas tbody').on('click', 'tr', function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
        $.ajax({
            type: 'POST',
            url: "/Administrador/ReadSucursales",
            datatype: "application/json",
            data: { },
            success: function (sucursales) {
                tableSucursal.destroy();
                tableSucursal = $('#tablaSucursales').DataTable({
                    "dom": 'ftp',
                    data: sucursales,
                    columns: [
                        { data: "Id" },
                        { data: "Nombre" },
                        { data: "Trabajadores" },
                        { data: "Direccion" },
                        { data: "Telefono" },
                        { data: "Estado" }

                    ]
                });
                $("#tablaSucursales").removeAttr("style");
            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }

        });
        $('#inputCreateSucursal').val(0);
        $('#btnUpdateEmpresa').addClass('invisible');
    } else {
        tableEmpresa.$('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        var fila = tableEmpresa.row('.selected').data();
        $.ajax({
            type: 'POST',
            url: "/Administrador/ReadSucursal",
            datatype: "application/json",
            data: { IdEmpresa: fila["Id"] },
            success: function (sucursales) {
                tableSucursal.destroy();
                tableSucursal = $('#tablaSucursales').DataTable({
                    "dom": 'ftp',
                    data: sucursales,
                    columns: [
                        { data: "Id" },
                        { data: "Nombre" },
                        { data: "Trabajadores" },
                        { data: "Direccion" },
                        { data: "Telefono" },
                        { data: "Estado" }
                    ]
                });
                $("#tablaSucursales").removeAttr("style");
            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }

        });
        var fila = tableEmpresa.row('.selected').data();
        $('#btnUpdateEmpresa').removeClass('invisible');
        $('#inputUpdateEmpresa').val(fila["Id"]);
        $('#inputCreateSucursal').val(fila["Id"]);



    }

});

$('#tablaSucursales tbody').on('click', 'tr', function () {

    if ($(this).hasClass('selected')) {
        $.ajax({
            type: 'POST',
            url: "/Administrador/ReadClientes",
            datatype: "application/json",
            data: {},
            success: function (clientes) {
                tableCliente.destroy();
                tableCliente = $('#tablaClientes').DataTable({
                    "dom": 'ftp',
                    data: clientes,
                    columns: [
                        { data: "Id" },
                        { data: "Nombre" },
                        { data: "Rut" },
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
        $(this).removeClass('selected');
        $('#inputCreateCliente').val(0);
        $('#btnUpdateSucursal').addClass('invisible');
    } else {
        tableSucursal.$('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        var fila = tableSucursal.row('.selected').data();
        $.ajax({
            type: 'POST',
            url: "/Administrador/ReadCliente",
            datatype: "application/json",
            data: { IdSucursal: fila["Id"] },
            success: function (clientes) {
                tableCliente.destroy();
                tableCliente = $('#tablaClientes').DataTable({
                    "dom": 'ftp',
                    data: clientes,
                    columns: [
                        { data: "Id" },
                        { data: "Nombre" },
                        { data: "Rut" },
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
        var fila = tableSucursal.row('.selected').data();
        $('#btnUpdateSucursal').removeClass('invisible');
        $('#inputUpdateSucursal').val(fila["Id"]);
        $('#inputCreateCliente').val(fila["Id"]);

    }


});
$('#tablaClientes tbody').on('click', 'tr', function () {

    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
        $('#btnUpdateCliente').addClass('invisible');
    } else {
        tableCliente.$('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        var fila = tableCliente.row('.selected').data();
        $('#btnUpdateCliente').removeClass('invisible');
        $('#inputUpdateCliente').val(fila["Id"]);

    }


});


