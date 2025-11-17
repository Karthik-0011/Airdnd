// BuiltYear: must be an integer between min and max
$.validator.addMethod('builtyear', function (value, element, params) {
    if (!value) {
        return true; // Let [Required] handle empty values
    }
    var year = parseInt(value, 10);
    if (isNaN(year)) return false;

    // Get min/max from the 'data-val-builtyear-min/max' attributes
    var min = parseInt(params.min, 10);
    var max = parseInt(params.max, 10);
    
    return year >= min && year <= max;
}, 'Please enter a valid year.');

// Add the adapter to link our 'builtyear' method to the attributes
// from BuiltYearAttribute.cs
$.validator.unobtrusive.adapters.add("builtyear", ["min", "max"], function (options) {
    options.rules["builtyear"] = {
        min: options.params.min,
        max: options.params.max
    };
    options.messages["builtyear"] = options.message;
});


// Bathroom: must be whole number or .5 increments
$.validator.addMethod('bathroom', function (value, element) {
    if (!value) {
        return true; // Let [Required] handle empty values
    }
    var num = parseFloat(value);
    if (isNaN(num)) return false;
    
    // Check if it's a whole number OR a half number
    return (num % 1 === 0) || (num % 1 === 0.5);
}, 'Please enter a valid number of bathrooms (whole or half increments).');

// Add the adapter to link our 'bathroom' method to the 
// 'data-val-bathroom' attribute from BathroomAttribute.cs
$.validator.unobtrusive.adapters.addBool("bathroom");