function GameViewModel(actions, settings, leftMouseButtonOnlyDown) {
    var self = this;

    self.live = [];
    self.settings = settings;
    self.actions = actions;
    self.isGameCreated = ko.observable();
    self.isGameContinued = ko.observable();
    self.isGameEnded = ko.observable();
    self.rows = ko.observable([]);
    self.stopReasonText = ko.observable('');
    self.iterationNumber = ko.observable(0);
    self.leftMouseButtonOnlyDown = leftMouseButtonOnlyDown;

    self.isGameCreated.subscribe(function (data) {
        console.log('isGameCreated = ' + data);
    });
    self.isGameContinued.subscribe(function (data) {
        console.log('isGameContinued = ' + data);
    });
    self.isGameEnded.subscribe(function (data) {
        console.log('isGameEnded = ' + data);
    });

    self.isGameCreated(false);
    self.isGameContinued(false);
    self.isGameEnded(true);

    self.createWorld = function () {

        var rows = [];

        for (var i = 0; i < self.settings.worldSize.height; i++) {
            var rowItem = { index: i, cells: [] };

            for (var j = 0; j < self.settings.worldSize.width; j++) {
                var cell = { index: j, isAlive: ko.observable() };

                rowItem.cells.push(cell);
            }

            rows.push(rowItem);
        }

        self.rows(rows);

        self.cleanWorld();

        self.isGameEnded(true);
        self.isGameCreated(true);
    }


    self.changeCellState = function (row, cell, isClicked) {
        if (!self.isGameContinued()) {
            var incomeHash = self.getHash(cell.index, row.index);

            if (self.leftMouseButtonOnlyDown() || isClicked) {
                if (self.live[incomeHash] === undefined) {
                    self.live[incomeHash] = { x: cell.index, y: row.index };
                    cell.isAlive(true);
                }
                else {
                    delete self.live[incomeHash];
                    cell.isAlive(false);
                }
            }
        }
    }

    self.getHash = function (x, y) {
        var hash = self.settings.worldSize.width * y + x;

        return hash;
    }

    self.cleanWorld = function () {
        var rows = self.rows();
        for (seekRow in rows) {
            var row = rows[seekRow];
            for (seekColumn in row.cells) {
                var cell = row.cells[seekColumn];

                cell.isAlive(false);
            }
        }
        self.live = [];
    }

    self.setGameState = function (live) {
        self.cleanWorld();

        for (seek in live) {
            var point = live[seek];
            var cell = self.rows()[point.y].cells[point.x];

            cell.isAlive(true)

            var hash = self.getHash(point.x, point.y);
            self.live[hash] = point;
        }
    }

    self.Init = function () {
    }

    self.updateWorld = function () {
        AjaxRequest(self.actions.getGameState, {}, true, function (result) {
            self.setGameState(result);
        });
    }

    self.startGame = function() {

        if (self.connection === undefined) {
            self.connect();
        }
        var points = self.getPoints(self.live);
        AjaxRequest(actions.createGame, { liveJson: JSON.stringify(points) }, false, function (data) {
            self.isGameCreated(true);
        });

        self.continueGame();
    }
    self.continueGame = function () {
        AjaxRequest(actions.continueGame, null , true, function () {
            self.isGameContinued(true);
            self.isGameEnded(false);
            self.stopReasonText('');
        });
    }
    self.stopGame = function (userCause) {
        self.isGameContinued(false);
        if (userCause) {
            AjaxRequest(actions.stopGame, null, true);
            self.stopReasonText('Пауза');
        }
        else {
            self.stopReasonText('Игра завершена');
        }
    }
    self.getPoints = function (dictionary) {
        var result = [];
        for (var key in dictionary) {
            result.push(dictionary[key]);
        }
        return result;
    }
    self.connect = function() {
        self.connection = new signalR.HubConnectionBuilder()
            .withUrl('/worldUpdateHub')
            //.configureLogging(LogLevel.Information)
            .build();
        self.connection.onclose(self.connect);
        self.connection.start().then(() => {
            console.log("Reconnect Successful");
        }).catch((error) => {
            console.log("Reconnect Failed", error == null ? error : "Unknown", "Retrying");
            setTimeout(self.connect, 5000);
        });
        self.connection.on("WorldUpdate", function (result) {
            self.iterationNumber(result.iterationCount);
            self.setGameState(result.live);
            if (result.isStoped) {
                self.stopGame(false, result.iterationCount);
                self.isGameEnded(true);
            }
        });
    }
}