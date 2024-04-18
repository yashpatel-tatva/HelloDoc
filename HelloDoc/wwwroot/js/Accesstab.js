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
var bool = false;
function validateforrolename() {
    if (!nameregex.test(Role_Name)) {
        $('#Role_Name').css("border", "1px solid red");
        $('#Error_for_name').html('Enter Valid Name for Role');
        return false;
    }
    if (nameregex.test(Role_Name)) {
        $('#Role_Name').css("border", "none");
        $('#Error_for_name').html('');
        $.ajax({
            url: '/AdminArea/AccessTab/IsThisRoleExist',
            data: { Role_Name },
            type: 'POST',
            success: function (data) {
                if (data == true) {
                    $('#Role_Name').css("border", "1px solid red");
                    $('#Error_for_name').html('Role Name already taken');
                    bool = false;
                }
                else {
                    $('#Role_Name').css("border", "none");
                    $('#Error_for_name').html('');
                    bool = true;
                }
            }
        })
        return bool;
    }
}
function validatemenu() {
    if ($('input[name="menuitem"]:checked').length === 0) {
        Swal.fire("Select Atleast One Menu for this role!");
        return false;
    }
    return true;
}
$('#Role_Name').on('input', function () {
    Role_Name = $('#Role_Name').val();
    validateforrolename();
});

$('#Role_Save').on('click', function () {
    Role_Name = $('#Role_Name').val();
    var formData = new FormData();
    formData.append('Rolename', Role_Name);
    $('input[name="menuitem"]:checked').each(function () {
        formData.append('menuitems', $(this).val());
    });

    formData.append('accounttype', $('#accounttypedropdown').val());
    console.log(validateforrolename())
    console.log(validatemenu())
    if (validateforrolename() && validatemenu()) {
        $.ajax({
            url: '/AdminArea/AccessTab/AddThisRole',
            data: formData,
            type: 'POST',
            processData: false,
            contentType: false,
            success: function (response) {
                location.reload();
            }
        });
    }
});

$('.roledeletebtn').on('click', function () {
    var roleid = $(this).data('id');
    $.ajax({
        url: '/AdminArea/AccessTab/DeleteThisRole',
        type: 'POST',
        data: { roleid },
        success: function (response) {
            location.reload();
        }
    });
});

$('.roleeditbtn').on('click', function () {
    var roleid = $(this).data('id');
    $.ajax({
        url: '/AdminArea/AccessTab/EditRolePage',
        type: 'POST',
        data: { roleid },
        success: function (response) {
            $('#nav-tabContent').html(response);
        }
    });
});