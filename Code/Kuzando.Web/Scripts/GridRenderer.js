function getFromDateStr() {
    return $('form > input#fromDate')[0]["value"];
}
function getFromDate() {
    return new Date(Date.parse(getFromDateStr()));
}

function updateCards() {
    var fromDateStr = getFromDateStr();
    var toDateStr = $('form > input#toDate')[0]["value"];

    $.get("/Tasks/Get", { from: fromDateStr, to: toDateStr }, function(data) {
        var tasksAssignedToDay
        data.every(function(task) {
            createCardFromTask(task);

            return true;
        });

        $(".taskcell").droppable({
            drop: function(event, ui) {
                try {
                    var draggable = ui.draggable[0];
                    var droppable = $(this)[0];

                    // an ugly hack - this happened some times
                    // todo
                    if (draggable.className == 'edit')
                        draggable = draggable.parentNode;

                    var taskId = getTaskId(draggable);
                    var dateStr = findDate(droppable);
                    var newPriorityInDay = findPriorityInDay(droppable);


                    $.post("/Tasks/UpdateTaskDatePriority", { taskId: taskId, newDate: dateStr, newPriorityInDay: newPriorityInDay },
                        function(data) { }, "json");

                    $(draggable).removeClass('ui-dragable-dragging');
                    $(draggable).attr('style', 'position:relative');
                    $(droppable).append(draggable);
                }
                catch (e) {
                    alert(e);
                }
            }
        });

        $(".taskcell").dblclick(function() {
            if ($(this).children().size() > 0)
                return;

            // create new task
            var priority = findPriorityInDay(this);
            var dueDate = findDate(this);

            $.post("/Tasks/CreateNewTask", { priority: priority, dueDate: dueDate }, function(task) {
                newSticky = createCardFromTask(task);
                newSticky.children(".edit").click();
            }, "json");
        });
    }, "json");
}

function createCardFromTask(task) {
    var fromDate = getFromDate();
    dueDate = eval(task["DueDate"].replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
    row = $('table.tasksgrid tr')[task["PriorityInDay"] + 1];

    var millisInDay = 1000 * 3600 * 24;
    column = Math.floor((dueDate - fromDate) / millisInDay);
    var cell = row.querySelectorAll('td')[column];
    var taskId = task["Id"];
    var text = task["Text"];
    var converter = new Showdown.converter();
    
    // todo - we need to do this without storing the html in the database (just for display).
    // var html = converter.makeHtml(text);
    var newSticky = $('<div class="sticky" id="note' + taskId + '"></div>');
    var newText = $('<div class="edit" id="text' + taskId + '">' + text + '</div>');
    newSticky.append(newText);
    newText.editable(function(value, settings) {
        var task = this.parentNode;
        var taskId = getTaskId(task);

        $.post("/Tasks/UpdateTaskText", { taskId: taskId, newText: value },
                function(data) { }, "json");

        return value;
    }, {
        indicator: 'Saving...',
        tooltip: 'Click to edit',
        type: 'textarea',
        submit: 'Save',
        cancel: 'Cancel'
    });
    newSticky.draggable({ revert: 'invalid', revertDuration: 200 });

    $(cell).append(newSticky);
    return newSticky;
}

$(document).ready(function() {
    updateCards();
    $('#trash').droppable({
        drop: function(event, ui) {
            try {
                var draggable = ui.draggable[0];

                // an ugly hack - this happened some times
                // todo
                if (draggable.className == 'edit')
                    draggable = draggable.parentNode;

                var taskId = getTaskId(draggable);

                $.post("/Tasks/Delete", { taskId: taskId },
                        function(data) { }, "json");

                $(draggable).remove();
            }
            catch (e) {
                alert(e);
            }
        }
    });
});

//Get the 'task ID' from the task DOM
function getTaskId(task) {
    // eg 'note18'
    var regex = /note(\d+)/;
    var match = regex.exec(task.id);
    return match[1];
}

function findDate(cell) {
    var fromDate = getFromDate();
    var col = jQuery.inArray(cell, cell.parentNode.children);
    if (col < 0)
        throw "Can't find cell";

    var date = new Date();
    date.setDate(fromDate.getDate() + col);
    return dateToString(date);
}

function findPriorityInDay(cell) {
    var row = cell.parentNode;
    return jQuery.inArray(row, row.parentNode.children) - 1;
}

function dateToString(date) {
    return (date.getMonth() + 1) + "/" + date.getDate() + "/" + date.getFullYear() + " 12:00:00 AM";
}
