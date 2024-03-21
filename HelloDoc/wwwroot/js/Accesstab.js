$('#createaccessbtn').on('click', function () {
    $.ajax({
        url: '/AdminArea/AccessTab/CreateRolePage',
        success: function (response) {
            $('#nav-tabContent').html(response);
        }
    });
});

function menus(accounttype) {
    $.ajax({
        url: '/AdminArea/AccessTab/GetMenus',
        data: { accounttype },
        type: 'POST',
        success: function (data) {
            console.log(data);
            $('#allmenus').html("");
            $.each(data, function (index, menu) {
                $('#allmenus').append('<div class="col-xl-3 col-md-4 col-sm-6 col-6 d-flex gap-2 align-items-center"><input type = "checkbox" name = "menuitem" class= "form-check-input mt-0" id = "menu_' + menu.menuid + '" value = "' + menu.menuid + '" /><label for="menu_' + menu.menuid + '">' + menu.name + '</label></div > ');
            });
        },
    });
}
menus(0);

$('#accounttypedropdown').on('change', function () {
    menus($(this).val());
});

var nameregex = /^[a-zA-Z]+$/i
var Role_Name = $('#Role_Name').val();
function validateforrolecreation() {
    console.log("acs");
    if (!nameregex.test(Role_Name)) {
        $('#Role_Name').closest('.form-group').css("border-color", "red");
        $('#Error_for_name').html('Enter Valid Name for Role');
        return false;
    }
}
$('#Role_Save').on('click', function () {

    console.log("clicked");
    validateforrolecreation();
    

});


