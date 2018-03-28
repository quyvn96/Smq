function validationFormLogin() {
    var validatePassword = Common.Validation.ValidateRequired('#UserName', '#validatefieldPassword');
    var validateUserName = Common.Validation.ValidateRequired('#Password', '#validatefieldUserName');
    if (validatePassword || validateUserName) return;
}