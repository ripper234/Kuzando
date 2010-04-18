$(document).ready(function() {
    updateCards();
});

function updateCards() {
    var fromDate = $('form > input#fromDate')[0]["value"];
    var toDate = $('form > input#toDate')[0]["value"];

    $.get("/Tasks/Get", { from: fromDate, to: toDate }, function(data) {
        var tasksAssignedToDay
        data.every(function(task) {
            var cell = findCell(task, fromDate);
            
            return true;
        });
    }, "json");
}

function findCell(task, fromDate) {
    fromDate = new Date(Date.parse(fromDate))
    dueDate = eval(task["DueDate"].replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
    row = $('table.tasksgrid tr')[task["PriorityInDay"] + 1];

    var millisInDay = 1000 * 3600 * 24;
    column = Math.floor((dueDate - fromDate) / millisInDay);
    cell = row.querySelectorAll('td')[column];
    cell.attributes['class'] = cell.attributes['class'] + " withimg";
    cell.innerHTML = 'Foobar';  //'<img src="/Content/postit.png" /> <div class="hidden"><p>' + task["Title"] + '</p>';
    cell = cell;
}