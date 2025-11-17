// This is the client-side logic from the Chapter 11 Registration example
jQuery.validator.addMethod("minimumage",
    function (value, element, param) {
        if (value === '') return false; // Handled by [Required] but good to have

        var dateToCheck = new Date(value);
        if (dateToCheck.toString() === "Invalid Date") return false;

        var minYears = Number(param);
        dateToCheck.setFullYear(dateToCheck.getFullYear() + minYears);

        var today = new Date();
        return (dateToCheck <= today);
    });

jQuery.validator.unobtrusive.adapters.add("minimumage", ["years"], function (options) {
    options.rules["minimumage"] = options.params.years;
    options.messages["minimumage"] = options.message;
});