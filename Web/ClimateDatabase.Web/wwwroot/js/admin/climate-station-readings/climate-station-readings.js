(function (climateDbApp, $, undefined) {

    //Public Property
    climateDbApp.climateStationReadingsConfig = {
        getStationsUrl: '',
        insertReadingFormId: '',
        insertReadingFormSubmitButtonId: '',
    };

    //Public Method
    climateDbApp.init = function () {
        $.validator.setDefaults({ ignore: ':disabled' });

        typeahead_initialize();

        $('form').on('typeahead:selected typeahead:autocomplete', '.typeahead', function (e, datum) {
            var $input = $(e.target);
            var $stationContainer = $input.closest('.station');

            console.log($stationContainer);

            //fire .focusout() so the validator can update
            $stationContainer.find('#ClimateStationId').attr('value', datum.id).focusout();
        });
        
        initInsertFormEvents();
    };

    //Private Method
    function typeahead_initialize() {
        var stations = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: climateDbApp.climateStationReadingsConfig.getStationsUrl,
                wildcard: 'QUERY'
            }
        });

        $('.typeahead').typeahead(null, {
            limit: 10,
            name: 'stations',
            source: stations,
            display: 'name',
            value: 'id',
            templates: {
                empty: [
                    '<div class="empty-message">',
                    'Nothing found',
                    '</div>'
                ].join('\n'),
            }
        });
    }

    function initInsertFormEvents() {
        var formId = climateDbApp.climateStationReadingsConfig.insertReadingFormId;
        var submitButtonId = climateDbApp.climateStationReadingsConfig.insertReadingFormSubmitButtonId;

        $(submitButtonId).on('click', function (e) {
            $(formId).submit();
        });
    }

}(window.climateDbApp = window.climateDbApp || {}, jQuery));