$(function() {
    if ($('#daterange').length) {
        
        $('#daterange').daterangepicker({
            autoUpdateInput: false,
            minDate: moment().startOf('day'), // Block past dates
            locale: {
                cancelLabel: 'Clear' 
            }
        });

        $('#daterange').on('apply.daterangepicker', function(ev, picker) {
            $(this).val(picker.startDate.format('MM/DD/YYYY') + ' - ' + picker.endDate.format('MM/DD/YYYY'));
        });

        $('#daterange').on('cancel.daterangepicker', function(ev, picker) {
            $(this).val('All');
        });

        var initialDates = $('#daterange').data('initial-dates');
        if (initialDates && initialDates !== 'All') {
            $('#daterange').val(initialDates);
        } else {
            $('#daterange').val('All');
        }
    }
});

$(function () {
    $('select[data-val="true"]').on('change', function () {
        $(this).valid();
    });
});