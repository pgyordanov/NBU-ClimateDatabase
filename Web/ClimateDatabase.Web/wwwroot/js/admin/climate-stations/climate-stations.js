(function (climateDbApp, $, undefined) {

    //Public Property
    climateDbApp.climateStationsConfig = {
        insertStationFormId: '',
        insertStationFormSubmitButtonId: '',
    };

    //Public Method
    climateDbApp.init = function () {
        $.validator.setDefaults({ ignore: ':disabled' });

        //typeahead_initialize();
        initInsertFormEvents();
    };

    //Private Method
    //Private Method
    function typeahead_initialize() {
        var stations = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: climateDbApp.climateStationsConfig.getStationsUrl,
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
        var formId = climateDbApp.climateStationsConfig.insertStationFormId;
        var submitButtonId = climateDbApp.climateStationsConfig.insertStationFormSubmitButtonId;

        $(submitButtonId).on('click', function (e) {
            $(formId).submit();
        });
    }

}(window.climateDbApp = window.climateDbApp || {}, jQuery));