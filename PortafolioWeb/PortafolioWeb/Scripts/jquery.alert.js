$(document).ready(function () {
    dialog.alert({
        title: "Credenciales inválidas",
        message: "Compruebe su rut y contraseña antes de volver a intentarlo",
        button: "Aceptar",
        animation: "fade",
        callback: function (value) {
            console.log(value);
        }
    });
    return false;
});