var user = {
    init: function () {
        user.registerEvent();
    },
    registerEvent: function () {
        $('#update-userinfomation').off('click').on('click', function () {
            user.initUpdateInfomation();
        });
        $('#change-password').off('click').on('click', function () {
            user.changePassword();
        });
        $('.close').off('click').on('click', function () {
            $("#currentpassword").attr("style", "border: 1px solid #cccccc");
            $("#validatecurrentpassword").html("");
            $("#newpassword").attr("style", "border: 1px solid #cccccc");
            $("#validatenewpassword").html("");
            $("#confimpassword").attr("style", "border: 1px solid #cccccc");
            $("#validateconfimpassword").html("");
            $(".validate-confimpassword-newpassword").html("");
            $("#currentpassword").val("");
            $("#newpassword").val("");
            $("#confimpassword").val("");

        });
        $('#close-changepassword').off('click').on('click', function () {
            $("#currentpassword").attr("style", "border: 1px solid #cccccc");
            $("#validatecurrentpassword").html("");
            $("#newpassword").attr("style", "border: 1px solid #cccccc");
            $("#validatenewpassword").html("");
            $("#confimpassword").attr("style", "border: 1px solid #cccccc");
            $("#validateconfimpassword").html("");
            $(".validate-confimpassword-newpassword").html("");
            $("#currentpassword").val("");
            $("#newpassword").val("");
            $("#confimpassword").val("");
        });
        $("#address-info").focusin(function() {
            $(this).attr("style", "border: 1px solid #cccccc");
            $("#validateaddress").html("");
        });
        $("#fullname-info").focusin(function () {
            $(this).attr("style", "border: 1px solid #cccccc");
            $("#validatefullname").html("");
        });
        $("#email-info").focusin(function () {
            $(this).attr("style", "border: 1px solid #cccccc");
            $("#validateemail").html("");
        });
        $("#phonenumber-info").focusin(function () {
            $(this).attr("style", "border: 1px solid #cccccc");
            $("#validatephonenumber").html("");
        });
        $("#birthday-info").focusin(function () {
            $(this).attr("style", "border: 1px solid #cccccc");
            $("#validatebirthday").html("");
        });

        $("#currentpassword").focusin(function () {
            $(this).attr("style", "border: 1px solid #cccccc");
            $("#validatecurrentpassword").html("");
            $("#checkcurrentpassword").html("");
        });
        $("#newpassword").focusin(function () {
            $(this).attr("style", "border: 1px solid #cccccc");
            $("#validatenewpassword").html("");
            $(".validate-confimpassword-newpassword").html("");
        });
        $("#confimpassword").focusin(function () {
            $(this).attr("style", "border: 1px solid #cccccc");
            $("#validateconfimpassword").html("");
            $(".validate-confimpassword-newpassword").html("");
        });
    },
    initUpdateInfomation: function () {
        var validatefullName = Common.Validation.ValidateRequired('#fullname-info', '#validatefullname');
        var validateemail = Common.Validation.ValidateRequiredEmail('#email-info', '#validateemail');
        var validatephoneNumber = Common.Validation.ValidateNumberFormat('#phonenumber-info', '#validatephonenumber');
        var validateaddress = Common.Validation.ValidateRequired('#address-info', '#validateaddress');
        var validatebirthday = Common.Validation.ValidateRequired('#birthday-info', '#validatebirthday');
        if (validatefullName || validateemail || validatephoneNumber || validateaddress || validatebirthday) return;
        var id = $("#userId-info").val();
        var fullName = $("#fullname-info").val();
        var email = $("#email-info").val();
        var birthDay = $("#birthday-info").val();
        var phoneNumber = $("#phonenumber-info").val();
        var address = $("#address-info").val();
        $.ajax({
            url: '/Account/YourInformation',
            type: 'POST',
            data: JSON.stringify({
                id: id,
                fullName: fullName,
                address:address,
                birthDay: birthDay,
                email: email,
                phoneNumber: phoneNumber            
            }),
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    alert("Cập nhật thành công!");
                }
                else {
                    alert("Cập nhật thất bại!");
                }
            }
        });
    },
    changePassword: function () {
        var validateCurrentPassword = Common.Validation.ValidatePassword('#currentpassword', '#validatecurrentpassword',6);
        var validateNewPassword = Common.Validation.ValidatePassword('#newpassword', '#validatenewpassword',6);
        var validateConfimPassword = Common.Validation.ValidatePassword('#confimpassword', '#validateconfimpassword',6);
        if (validateNewPassword || validateConfimPassword || validateConfimPassword) return;
        var currentPassword = $("#currentpassword").val();
        var newPassword = $("#newpassword").val();
        var confimPassword = $("#confimpassword").val();
        if (newPassword !== confimPassword) {
            $(".validate-confimpassword-newpassword").html("Xác nhận sai mật khẩu!");
            return;
        }
        $.ajax({
            url: '/Account/ChangePassword',
            type: 'POST',
            data: JSON.stringify({
                currentPassword: currentPassword,
                newPassword: newPassword
            }),
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    alert("Thay đổi thành công!");
                    $('#change-password').prop("data-dismiss", "modal");               
                }
                else {
                    $("#checkcurrentpassword").html("Sai mật khẩu hiện tại !");
                    alert("Thay đổi thất bại!");
                }
            }
        });
    }
}
user.init();