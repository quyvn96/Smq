var Common = Common || {}

Common.Validation = {
    CheckPositiveInterger: function (x) {
        var year = /^\+?\d+$/;
        return year.test(x);
    },

    ValidateRequired: function (element, errElement) {
        var value = $(element).val().trim().replace(/\$|\,/g, '');
        if (value === "") {
            $(element).attr("style", "border: 1px solid red");
            $(errElement).html("Trường bắt buộc.");
            return true;
        } else {
            $(element).attr("style", "border: 1px solid #cccccc");
            $(errElement).html("");
            return false;
        }
    },

    ValidateNumberFormat: function (element, errElement) {
        var value = $(element).val().trim().replace(/\$|\,/g, '');
        if (!$.isNumeric(value)) {
            $(element).attr("style", "border: 1px solid red");
            $(errElement).html("Trường này phải nhập số.");
            return true;
        } else {
            var valueData = parseFloat(value);
            if (valueData < 0) {
                $(element).attr("style", "border: 1px solid red");
                $(errElement).html("This field must be greater than 0.");
                return true;
            }
            $(element).attr("style", "border: 1px solid #cccccc");
            $(errElement).html("");
            return false;
        }
    },

    ValidateIntNumberInRange: function (element, errElement, max) {
        var value = $(element).val().trim().replace(/\$|\,/g, '');
        var valueData = parseFloat(value);
        if (valueData - parseInt(value) !== 0) {
            $(element).attr("style", "border: 1px solid red");
            $(errElement).html("This field must an integer");
            return true;
        } else {
            $(element).attr("style", "border: 1px solid #cccccc");
            $(errElement).html("");
        }
        if (valueData > max) {
            $(element).attr("style", "border: 1px solid red");
            $(errElement).html("This field must be less than or equal to " + max);
            return true;
        } else {
            $(element).attr("style", "border: 1px solid #cccccc");
            $(errElement).html("");
            return false;
        }
    },

    ValidateRequiredDropDown: function (element, errElement) {
        var value = $(element).val();
        if (value == "" || value == 0) {
            $(element).attr("style", "border: 1px solid red");
            $(errElement).html("This field is required.");
            return true;
        } else {
            $(element).attr("style", "border: 1px solid #cccccc");
            $(errElement).html("");
            return false;
        }
    },

    ValidateRequiredKendoMultiSelect: function (element, errElement) {
        var partner = $(element).data("kendoMultiSelect").value();
        if (partner.length == 0) {
            $(element).parents('.k-multiselect').attr("style", "border: 1px solid red");
            $(errElement).html("This field is required.");
            return true;
        } else {
            $(element).parents('.k-multiselect').removeAttr("style");
            $(errElement).html("");
            return false;
        }
    },

    ValidateRequiredSelectedBox: function (element, errElement) {
        var value = $(element).attr('value');
        if (typeof (value) == "undefined") {
            $(errElement).parent(".form-group-initiative").find(".list-label").attr("style", "border: 1px solid red");
            $(errElement).html("This field is required.");
            return true;
        } else {
            $(element).parents('.list-label').removeAttr('style');
            $(errElement).html("");
            return false;
        }
    },
    ValidateRequiredEmail: function (element, errElement) {
        var emailRegex = new RegExp(/^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$/i);
        var emailAddress =  $(element).val();
        var valid = emailRegex.test(emailAddress);
        if (!valid) {
            $(element).attr("style", "border: 1px solid red");
            $(errElement).html("Làm ơn nhập một địa chỉ email.");
            return true;
        } else {
            $(element).attr("style", "border: 1px solid #cccccc");
            $(errElement).html("");
            return false;
        }        
    },
    ValidateRequiredDate:function(element,errElement){
        var dateOut = $(element).val().split("/");
        var dateToSend = new Date(dateOut[0] + "/" + dateOut[1] + "/" + dateOut[2]);
        if (!dateToSend) {
            $(element).attr("style", "border: 1px solid red");
            $(errElement).html("Làm ơn nhập ngày tháng năm sinh.");
            return true;
        }
        else {
            $(element).attr("style", "border: 1px solid #cccccc");
            $(errElement).html("");
            return false;
        }
    },
    ValidatePassword: function (element, errElement, max) {
        var value = $(element).val().trim().replace(/\$|\,/g, '');
        var valueCount = value.length;
        if (valueCount < max) {
            $(element).attr("style", "border: 1px solid red");
            $(errElement).html("Mật khẩu tối thiểu "+max+" ký tự ");
            return true;
        } else {
            $(element).attr("style", "border: 1px solid #cccccc");
            $(errElement).html("");
            return false;
        }
    }
}