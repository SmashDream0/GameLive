function CellViewModel(columnIndex, rowIndex) {
    var self = this;

    self.columnIndex = columnIndex;
    self.rowIndex = rowIndex;

    self.isAlive = ko.observable(false);
}