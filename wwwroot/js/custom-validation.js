$.validator.addMethod('builtyear', function (value, element, params) {
    if (!value) {
        return true;
    }
    var year = parseInt(value, 10);
    if (isNaN(year)) return false;

    var min = parseInt(params.min, 10);
    var max = parseInt(params.max, 10);
    
    return year >= min && year <= max;
}, 'Please enter a valid year.');

$.validator.unobtrusive.adapters.add("builtyear", ["min", "max"], function (options) {
    options.rules["builtyear"] = {
        min: options.params.min,
        max: options.params.max
    };
    options.messages["builtyear"] = options.message;
});

$.validator.addMethod('bathroom', function (value, element) {
    if (!value) {
        return true;
    }
    var num = parseFloat(value);
    if (isNaN(num)) return false;
    
    return (num % 1 === 0) || (num % 1 === 0.5);
}, 'Please enter a valid number of bathrooms (whole or half increments).');

$.validator.unobtrusive.adapters.addBool("bathroom");