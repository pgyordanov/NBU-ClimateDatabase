(function (climateDbApp, $, undefined) {

    //Public Property
    climateDbApp.climateStationsConfig = {
        insertStationFormId: '',
        insertStationFormSubmitButtonId: '',
    };

    //Public Method
    climateDbApp.init = function () {
        $.validator.setDefaults({ ignore: ':disabled' });

        initInsertFormEvents();
    };

    //Private Method
    function initInsertFormEvents() {
        var formId = climateDbApp.climateStationsConfig.insertStationFormId;
        var submitButtonId = climateDbApp.climateStationsConfig.insertStationFormSubmitButtonId;

        $(submitButtonId).on('click', function (e) {
            $(formId).submit();
        });
    }

}(window.climateDbApp = window.climateDbApp || {}, jQuery));