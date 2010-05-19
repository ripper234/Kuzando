$(document).ready(function() {
    $.ajaxSetup({
        // Disable caching of AJAX responses */
        cache: false
    });

    colorToday();
    updateCards();
    doActionIcons();
    $('img.checked').live('click', handle_checking_unchecking);
    $('img.unchecked').live('click', handle_checking_unchecking);
});

function getFromDateDays() {
    return parseInt($('form > fieldset input#fromDate')[0]["value"], 10);
}

function getFromDate() {
    var daysFrom1970Str = getFromDateDays();
    var date = new Date(parseInt(daysFrom1970Str, 10) * 24 * 3600 * 1000);
    return date;
}

function updateCards() {
    var fromDate = getFromDateDays();

    $.get("/Tasks/Get", { fromDate: fromDate }, function(data) {
        try {
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
                        var date = findDateDays(droppable);
                        var newPriorityInDay = findPriorityInDay(droppable);

                        updateTaskDatePriority(taskId, date, newPriorityInDay);

                        $(draggable).removeClass('ui-dragable-dragging');
                        $(draggable).attr('style', 'position:relative');
                        $(droppable).append(draggable);
                        recalcOverdue(draggable);
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
        }
        catch (e) {
            alert(e);
        }
    }, "json");
}

function handle_checking_unchecking() {
    var img = $(this);
    var sticky = img.parentsUntil(".taskcell").last();
    var currentlyChecked;
    if (sticky.hasClass('done')) {
        currentlyChecked = true;
    } else {
        currentlyChecked = false;
    }
     
    var newImgSrc;
    if (currentlyChecked) {
        newImgSrc = uncheckedImgSrc;
        sticky.removeClass("done");
    }
    else {
        newImgSrc = checkedImgSrc;
        sticky.addClass("done");
    }
    img.attr("src", newImgSrc);
    recalcOverdue(sticky);
    var taskId = getTaskId(sticky);
    $.post("/Tasks/UpdateTaskDoneStatus", { taskId: taskId, newDoneStatus: !currentlyChecked },
            function(data) { }, "json");
}

function updateTaskDatePriority(taskId, dateStr, newPriorityInDay) {
    $.post("/Tasks/UpdateTaskDatePriority", { taskId: taskId, newDate: dateStr, newPriorityInDay: newPriorityInDay },
                        function(data) { }, "json");
}

function createNewTaskInCell(cell) {
    var priority = findPriorityInDay(cell);
    var dueDate = findDateDays(cell);

    $.post("/Tasks/CreateNewTask", { priority: priority, dueDate: dueDate }, function(task) {
        newSticky = createCardFromTask(task);
        newSticky.children(".edit").click();
    }, "json");
}

checkedImgSrc = "/Content/Images/checked-icon.png";
uncheckedImgSrc = "/Content/Images/unchecked-icon.png";

function createCardFromTask(task) {
    try {
        var fromDate = getFromDateDays();
        dueDateDays = parseInt(task["DueDateInDays"], 10);
        row = $('table.tasksgrid > tbody > tr')[task["PriorityInDay"] + 1];

        column = dueDateDays - fromDate;
        if (column < 0) {
            throw "Negative column. dueDate=" + dueDateDays + ", fromDate= " + fromDate;
        }
        var cell = row.querySelectorAll('td.taskcell')[column];
        if (cell == null)
            throw ("Failed to fine taskcell in row for priority " + task["PriorityInDay"]);
            
        var taskId = task["Id"];
        var text = task["Text"];
        //var converter = new Showdown.converter();

        // todo - we need to do this without storing the html in the database (just for display).
        // var html = converter.makeHtml(text);


        var newSticky = $('.sticky-template').clone().show();
        newSticky.removeClass('sticky-template');
        newSticky.addClass('sticky');
        if (task["Done"])
            newSticky.addClass("done");
        newSticky.attr('id', 'note' + taskId);
        var img;
        if (task["Done"]) {
            img = $('<img width="20" height="20" class="checked" alt="done" src="' + checkedImgSrc + '" />');
            // img.click(handle_unchecking);
        }
        else {
            img = $('<img width="20" height="20" class="unchecked" alt="done" src="' + uncheckedImgSrc + '" />');
            // img.click(handle_checking);
        }
        newSticky.find("div.checked-cell").append(img);
        edit = newSticky.find('.edit');
        edit.attr('id', 'text' + taskId);
        edit.append(text);
        
        $(cell).append(newSticky);
        recalcOverdue(newSticky);
        
        edit.editable(function(value, settings) {
            try {
                var taskId = getTaskId(this);

                $.post("/Tasks/UpdateTaskText", { taskId: taskId, newText: value },
                function(data) { }, "json");

                return value;
            }
            catch (e) {
                alert(e);
            }
        }, {
            // indicator: 'Saving...',
            tooltip: 'Click to edit',
            type: 'textarea',
            onblur: 'submit'
            // submit: 'Save',
            // cancel: 'Cancel'
        });
        newSticky.draggable({ revert: 'invalid',
            revertDuration: 200,
            scroll: false
        });

        return newSticky;
    }
    catch (e) {
        alert(e);
    }
    
}

function recalcOverdue(sticky) {
    sticky = $(sticky);
    var cell = sticky.parents('.taskcell').first();
    if (!sticky.hasClass('done') && findDateDays(cell) < getTodayInDays()) {
        sticky.addClass("overdue");
    } else {
        sticky.removeClass("overdue");
    }
}

function getTodayInDays() {
    var millisInDay = 1000 * 3600 * 24;
    var today = new Date();
    var daysSinceEpoch = Math.floor(today.getTime() / millisInDay);
    return daysSinceEpoch;
}

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
                var toDateDays = getFromDateDays();
                if (prefix == "prev")
                    toDateDays -= 7;
                else if (prefix == "next")
                    toDateDays += 7
                else throw "Unexpected prefix '" + prefix + "'";
                
                var newPriorityInDay = findPriorityInDay(draggable.parentNode);

                updateTaskDatePriority(taskId, toDateDays, newPriorityInDay);                                
                    
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
    var task2 = $(task).parentsUntil('.taskcell').last(); // get the child of taskcell
    if (task2 != null && task2.size() > 0)
        task = task2;
    else
        task = $(task);
        
    // eg 'note18'
    var regex = /note(\d+)/;
    var match = regex.exec(task.attr("id"));
    return match[1];
}

function findDateDays(cell) {
    var fromDate = getFromDateDays();
    cell = $(cell);
    var col = cell.index();
    //var col = jQuery.inArray(cell, cell.parentNode.children);
    if (col < 0)
        throw "Can't find cell";

    return fromDate + col;
}

function findPriorityInDay(cell) {
    var row = cell.parentNode;
    return jQuery.inArray(row, row.parentNode.children) - 1;
}

function dateToString(date) {
    return (date.getMonth() + 1) + "/" + date.getDate() + "/" + date.getFullYear();  //+ " 12:00:00 AM";
}
