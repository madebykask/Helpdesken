$(document).on('click', '#btnHomeAddress, #btnEmergencyAddress', function (e) {
    e.preventDefault();

    var prefix = $(this).attr('data-prefix');
    document.getElementById(prefix + "Addressline1").value = document.getElementById('PermanentAddressline1').value;
    document.getElementById(prefix + "Addressline2").value = document.getElementById('PermanentAddressline2').value;
    document.getElementById(prefix + "Addressline3").value = document.getElementById('PermanentAddressline3').value;
    document.getElementById(prefix + "Addressline4").value = document.getElementById('PermanentAddressline4').value;

    document.getElementById(prefix + "PostalCode").value = document.getElementById('PermanentPostalcode').value;
    document.getElementById(prefix + "City").value = document.getElementById('PermanentCity').value;
    document.getElementById(prefix + "Country").value = document.getElementById('PermanentCountry').value;

});

validate.run($('#actionState').val());

$(document).on('change', '#actionState', function (e) {
    var val = $(this).val();
    validate.run(val);
});