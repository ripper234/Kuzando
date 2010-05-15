﻿function removeHoursFromDate(dateWithHours) {
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
                    var draggable = getDraggedTask(ui);
                    var droppable = $(this)[0];

                    if (draggable.id == 'newsticky') {
                        createNewTaskInCell(droppable);
                        return;
                    }

                    var taskId = getTaskId(draggable);
                    var dateStr = findDate(droppable);
                    var newPriorityInDay = findPriorityInDay(droppable);

                    updateTaskDatePriority(taskId, dateStr, newPriorityInDay);
                    
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

function updateTaskDatePriority(taskId, dateStr, newPriorityInDay) {
    $.post("/Tasks/UpdateTaskDatePriority", { taskId: taskId, newDate: dateStr, newPriorityInDay: newPriorityInDay },
                        function(data) { }, "json");
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
        onblur: 'submit',
        // submit: 'Save',
        // cancel: 'Cancel'
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

    colorToday();
    updateCards();
    doActionIcons();
});

function colorToday() {
    var today = new Date();
    var date = getFromDate();
    $('.headercell div').each(function(i, cell) {
        if (date.getDate() == today.getDate()) {
            $(cell).addClass("today");
        }
        date.setDate(date.getDate() + 1);
    });
}

function doActionIcons() {
    $('#trash').droppable({
        drop: function(event, ui) {
            try {
                var draggable = getDraggedTask(ui);
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

    $('#nextWeek').droppable(createNextPrevWeekDrag("next"));
    $('#prevWeek').droppable(createNextPrevWeekDrag("prev"));
    
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

function createNextPrevWeekDrag(prefix){
    return {
        drop: function(event, ui) {
            try {
                var draggable = getDraggedTask(ui);
                if (!$(draggable).hasClass('sticky')) {
                    return;
                }

                var taskId = getTaskId(draggable);
                var toDate = getFromDate();
                if (prefix == "prev")
                    toDate.setDate(toDate.getDate() - 7);
                else if (prefix == "next")
                    toDate.setDate(toDate.getDate() + 7);
                else throw "Unexpected prefix '" + prefix + "'";
                
                var toDateStr = dateToString(toDate);
                var newPriorityInDay = findPriorityInDay(draggable.parentNode);

                updateTaskDatePriority(taskId, toDateStr, newPriorityInDay);
                
                
                    
                $(draggable).remove();
                alert("Dragged task to " + prefix + " week");
                //$.post("/Tasks/Delete", { taskId: taskId },
                  //      function(data) { }, "json");

                //$(draggable).remove();
            }
            catch (e) {
                alert(e);
            }
        }
    }
}

function getDraggedTask(ui){
    var draggable = ui.draggable[0];

    // an ugly hack - this happened some times
    // todo
    if (draggable.className == 'edit')
        draggable = draggable.parentNode;
    return draggable;
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
