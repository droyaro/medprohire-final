/// <reference path="../lib/jquery-validation/dist/jquery.validate.js" />
/// <reference path="../lib/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.js" />

$.validator.addMethod('requiredif',
    function (value, element, parameters) {
            var id = parameters['property'];


        var targetvalue = parameters['value'];
        targetvalue =
            (targetvalue == null ? '' : targetvalue).toString();
       
        // get the actual value of the target control
        // note - this probably needs to cater for more 
        // control types, e.g. radios
        if (id.indexOf(',') > -1) {
            var id0 = id.split(',')[0];
            var id1 = id.split(',')[1];
            var targetvalue0 = targetvalue.split(',')[0];
            var targetvalue1 = targetvalue.split(',')[1];
            var control = $("#" + id0);
            var controltype0 = control.attr('type');
            var actualvalue0 =
                controltype0 === 'checkbox' || controltype === 'radio'  ?
                    control.prop('checked').toString() : control.val();
            var control = $("#" + id1);
            var controltype1 = control.attr('type');
            var actualvalue1 = 
                controltype1 === 'checkbox' || controltype === 'radio'  ?
                    control.prop('checked').toString()  : control.val();

            if (targetvalue0 === actualvalue0 && targetvalue1 === actualvalue1) {
                if (value == "" || value == null || value == "0")
                return false;

            }

        }
        else {
            var control = $("#" + id);
            var controltype = control.attr('type');
            var actualvalue =
                controltype === 'checkbox' || controltype === 'radio'  ?
                    control.prop('checked').toString() :
                    control.val();
            if (targetvalue === actualvalue) {
                if (value == "" || value == null || value=="0")
                return false;

            }
                
        }
        return true;
    }
);

$.validator.unobtrusive.adapters.add(
    'requiredif',
    ['property', 'value'], 
    function (options) {
        options.rules['requiredif'] = {
            property: options.params['property'],
            value: options.params['value']
        };
        options.messages['requiredif'] = options.message;
    });
