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

        $(".sticky").draggable({ revert: 'invalid', revertDuration: 200 });

        $(".taskcell").droppable({
            drop: function(event, ui) {
                try {
                    var draggable = ui.draggable[0];
                    var droppable = $(this)[0];

                    // an ugly hack - this happened some times
                    if (draggable.className == 'edit')
                        draggable = draggable.parentNode;

                    var taskId = getTaskId(draggable);
                    var newDate = findNewDate(droppable);
                    var newPriorityInDay = findPriorityInDay(droppable);
                    var dateStr = dateToString(newDate);

                    $.post("/Tasks/UpdateTaskDatePriority", { taskId: taskId, newDate: dateStr, newPriorityInDay: newPriorityInDay }, 
                        function(data) {}, "json");

                    $(draggable).removeClass('ui-dragable-dragging');
                    $(draggable).attr('style', 'position:relative');
                    //$(droppable).append(draggable);
                    $(droppable).append(draggable);
                }
                catch (e) {
                    alert(e);
                }
            }
        });

        $(".edit").editable(function(value, settings) {
            var task = this.parentNode;
            var taskId = getTaskId(task);
            
            $.post("/Tasks/UpdateTaskText", { taskId: taskId, newText: value},
                function(data) { }, "json");
                
            return value;
        }, {
            indicator: 'Saving...',
            tooltip: 'Click to edit',
            type: 'textarea',
            submit: 'Save',
            cancel: 'Cancel'
        });
    }, "json");
}

function findCell(task, fromDate) {
    dueDate = eval(task["DueDate"].replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
    row = $('table.tasksgrid tr')[task["PriorityInDay"] + 1];

    var millisInDay = 1000 * 3600 * 24;
    column = Math.floor((dueDate - fromDate) / millisInDay);
    var cell = row.querySelectorAll('td')[column];
    var taskId = task["Id"];
    cell.innerHTML = '<div class="sticky" id="note' + taskId + '"><div class="edit" id="text' + taskId + '">' + task["Text"] + '</div></div>';
}

$(document).ready(function() {
    updateCards();
});

//Get the 'task ID' from the task DOM
function getTaskId(task) {
    // eg 'note18'
    var regex = /note(\d+)/;
    var match = regex.exec(task.id);
    return match[1];
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
