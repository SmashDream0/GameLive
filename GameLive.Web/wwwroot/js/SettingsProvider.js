function settingsProvider(actions){
    var self = this;

    self.settings = undefined;

    self.init = function () {
        self.settings = self.getSettings();
    }
    self.getSettings = function () {
        var result = {};
        AjaxRequest(actions.getSettings, null, false, function (data) { result = data; });
        return result;
    }
}