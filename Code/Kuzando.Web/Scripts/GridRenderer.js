function removeHoursFromDate(dateWithHours) {
    var dateRegex = /(\d+\/\d+\/\d+)/;
    var match = dateRegex.exec(dateWithHours);
    return match[1];
}

function getFromDateStr() {
    return removeHoursFromDate($('form > fieldset input#fromDate')[0]["value"]);
}
function getFromDate() {
    return new Date(Date.parse(getFromDateStr()));
}

function updateCards() {
    var fromDateStr = getFromDateStr();
    var toDateStr = removeHoursFromDate($('form > fieldset input#toDate')[0]["value"]);

    $.get("/Tasks/Get", { from: fromDateStr, to: toDateStr }, function(data) {
        $.each(data, function() {
            createCardFromTask(this);
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

                    if (draggable.id == 'newsticky') {
                        createNewTaskInCell(droppable);
                        return;
                    }

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
            createNewTaskInCell(this);
        });
    }, "json");
}

function createNewTaskInCell(cell) {
    var priority = findPriorityInDay(cell);
    var dueDate = findDate(cell);

    $.post("/Tasks/CreateNewTask", { priority: priority, dueDate: dueDate }, function(task) {
        newSticky = createCardFromTask(task);
        newSticky.children(".edit").click();
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
    newSticky.draggable({   revert: 'invalid', 
                            revertDuration: 200,
                            scroll: false
                        });

    $(cell).append(newSticky);
    return newSticky;
}

$(document).ready(function() {
    $.ajaxSetup({
        // Disable caching of AJAX responses */
        cache: false
    });
    
    updateCards();
    doActionIcons();
});

function doActionIcons() {
    $('#trash').droppable({
        drop: function(event, ui) {
            try {
                var draggable = ui.draggable[0];

                // an ugly hack - this happened some times
                // todo
                if (draggable.className == 'edit')
                    draggable = draggable.parentNode;

                if (!$(draggable).hasClass('sticky')) {
                    return;
                }

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

    $('#newsticky').draggable({
        revert: 'invalid',
        revertDuration: 200,
        cursor: 'move',
        cursorAt: { top: 40, left: 40 },
        helper: function(event) {
            return $('<img src="/Content/Images/sticky.png" alt="" width="64" height="64"/>');
        },
        //containment: '#main',
        scroll: false
    });
}

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

    var date = fromDate;
    date.setDate(fromDate.getDate() + col);
    return dateToString(date);
}

function findPriorityInDay(cell) {
    var row = cell.parentNode;
    return jQuery.inArray(row, row.parentNode.children) - 1;
}

function dateToString(date) {
    return (date.getMonth() + 1) + "/" + date.getDate() + "/" + date.getFullYear();  //+ " 12:00:00 AM";
}
