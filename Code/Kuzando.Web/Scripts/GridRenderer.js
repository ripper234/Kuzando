$(document).ready(function() {
    updateCards();
});

function getFromDateStr() {
    return $('form > input#fromDate')[0]["value"];
}
function getFromDate() {
    return new Date(Date.parse(getFromDateStr()));
}

function updateCards() {
    var fromDateStr = getFromDateStr();
    var fromDate = getFromDate();
    var toDateStr = $('form > input#toDate')[0]["value"];

    $.get("/Tasks/Get", { from: fromDateStr, to: toDateStr }, function(data) {
        var tasksAssignedToDay
        data.every(function(task) {
            var cell = findCell(task, fromDate);
            
            return true;
        });

        $(".sticky").draggable({ revert: true , revertDuration:0});
    }, "json");
}

function findCell(task, fromDate) {
    dueDate = eval(task["DueDate"].replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
    row = $('table.tasksgrid tr')[task["PriorityInDay"] + 1];

    var millisInDay = 1000 * 3600 * 24;
    column = Math.floor((dueDate - fromDate) / millisInDay);
    var cell = row.querySelectorAll('td')[column];
    cell.innerHTML = '<div class="sticky" id="' + task["Id"] + '"><div class="text">' + task["Title"] + '</div></div>';
}

$(document).ready(function() {
    $(".taskcell").droppable({
        drop: function(event, ui) {
            try {
                var taskId = getTaskId(event.originalTarget);
                var newDate = findNewDate(event.target);
                var newPriorityInDay = findPriorityInDay(event.target);
                var dateStr = dateToString(newDate);

                $.post("/Tasks/UpdateTask", { taskId: taskId, newDate: dateStr, newPriorityInDay: newPriorityInDay }, function(data) {
                }, "json");
                $(event.originalTarget).removeClass('ui-dragable-dragging');
                $(event.target).append(event.originalTarget);
            }
            catch (e) {
                alert(e);
            }
        }
    });
});

//Get the 'task ID' from the task DOM
function getTaskId(task) {
    return task.id;
}

function findNewDate(newCell) {
    var fromDate = getFromDate();
    var col = jQuery.inArray(newCell, newCell.parentNode.children);
//    
//    var col = 0;
//    $(newCell.parentNode).children().each(function() {
//        if (this == newCell) {
//            return false;
//        }
//        col++;
//    });
    if (col < 0)
        throw "Can't find cell";

    var newDate = new Date();
    newDate.setDate(fromDate.getDate() + col);
    return newDate;
}

function findPriorityInDay(newCell) {
    var row = newCell.parentNode;
    return jQuery.inArray(row, row.parentNode.children) - 1;
}

function dateToString(date) {
    return (date.getMonth() + 1) + "/" + date.getDate() + "/" + date.getFullYear() + " 12:00:00 AM";
}