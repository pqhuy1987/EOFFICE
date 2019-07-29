/*
list function
fncBuilCalendar
fncBuildHeader
fncBuildHeader_Row
fncAddRowToTop
fncBuildHeader_Cell
fncBuildBody
fncBuildBody_Row
fncBuildBody_Cell
fncUpdateSTT
*/

function fncBuilCalendar(table, dataHeader, dataBody) {
    var tableCalendar = table;
    //xoa du lieu trong table
    if (tableCalendar.find('thead').length == 0)
        tableCalendar.append('<thead></thead>');
    if (tableCalendar.find('tbody').length == 0)
        tableCalendar.append('<tbody></tbody>');
    if (tableCalendar.find('tfoot').length == 0)
        tableCalendar.append('<tfoot></tfoot>');
    fncBuildHeader(tableCalendar.find('thead'), dataHeader); //tao header table    
    fncBuildBody(tableCalendar.find('tbody'), dataBody);
}

function fncBuildHeader(tableCalendar, dataHeader) {
    fncBuildHeader_Row(dataHeader, tableCalendar);
}

//tao html table thead
function fncBuildHeader_Row(data, tableCalendar) {
    var tbRow;
    var isTH = true;
    $.each(data, function (index, value) {
        tbRow = $('<tr></tr>').addClass(this.col_class);        
        if (this.col_value.length > 0)
            fncBuildHeader_Cell(this.col_value, tbRow, isTH);
        
        tableCalendar.append(tbRow);
    });    
}

function fncBuilCalendar_header(table, dataHeader, dataBody) {
    var tableCalendar = table;
    //xoa du lieu trong table
    if (tableCalendar.find('thead').length == 0)
        tableCalendar.append('<thead></thead>');
    //if (tableCalendar.find('tbody').length == 0)
    //    tableCalendar.append('<tbody></tbody>');
    //if (tableCalendar.find('tfoot').length == 0)
    //    tableCalendar.append('<tfoot></tfoot>');
    fncBuildHeader(tableCalendar.find('thead'), dataHeader); //tao header table    
    //fncBuildBody(tableCalendar.find('tbody'), dataBody);
}

function fncBuilCalendar_body(table, dataHeader, dataBody) {
    var tableCalendar = table;
    //xoa du lieu trong table

    if (tableCalendar.find('tbody').length == 0)
        tableCalendar.append('<tbody></tbody>');
    if (tableCalendar.find('tfoot').length == 0)
        tableCalendar.append('<tfoot></tfoot>');
    //fncBuildHeader(tableCalendar.find('thead'), dataHeader); //tao header table    
    fncBuildBody(tableCalendar.find('tbody'), dataBody);
}


function fncBuildHeader_Cell(data, tableCalendar, isTH) {
    var tbCell;
    $.each(data, function (index, value) {
        tbCell = $('<th></th>').addClass(this.col_class).html(this.col_value);
        if (!CheckNullOrEmpty(this.colspan))
            if(this.colspan > 1)
                tbCell.attr('colspan', this.colspan);
        if (!CheckNullOrEmpty(this.rowspan))
            if(this.rowspan > 1)
                tbCell.attr('rowspan', this.rowspan);
        tableCalendar.append(tbCell);
    });
}

//tao html table tbody
function fncBuildBody(tableCalendar, data) {
    dataBody = data;
    fncBuildBody_Row(dataBody, tableCalendar);
    //fncUpdateSTT(tableCalendar);
}

function fncBuildBody_Row(dataInput, tableCalendar) {
    var tbRow;
    $.each(dataInput, function (index, value) {
        tbRow = $('<tr></tr>').addClass(this.col_class);
        if (!CheckNullOrEmpty(this.col_id))
            tbRow.attr('codeid', this.col_id);
        if (this.col_value.length > 0 && typeof this.col_value != 'string')
            fncBuildBody_Cell(this.col_value, tbRow);
        else {
            var tdCell = $('<td></td>').attr('colspan', this.colspan).html(this.col_value);
            tbRow.append(tdCell);
        }
        //attribute row
        if (!CheckNullOrEmpty(this.col_attr)) {
            if (this.col_attr.length > 0) {
                $.each(this.col_attr, function () {
                    tbRow.attr(this.name, this.value);
                });
            }
        }
        tableCalendar.append(tbRow);
    });
}


function fncAddRowToTop(tableCalendar, data) {
    var tbRow;
    $.each(data, function (index, value) {
        tbRow = $('<tr></tr>').addClass(this.col_class);
        if (!CheckNullOrEmpty(this.col_id))
            tbRow.attr('codeid', this.col_id);
        if (this.col_value.length > 0 && typeof this.col_value != 'string')
            fncBuildBody_Cell(this.col_value, tbRow);
        else {
            var tdCell = $('<td></td>').attr('colspan', this.colspan).html(this.col_value);
            tbRow.append(tdCell);
        }
        //attribute row
        if (!CheckNullOrEmpty(this.col_attr)) {
            if (this.col_attr.length > 0) {
                $.each(this.col_attr, function () {
                    tbRow.attr(this.name, this.value);
                });
            }
        }
        tableCalendar.find('.header-box').after(tbRow);
    });
}

function fncAddRowTopBottom(tableCalendar, data) {
    var tbRow;
    $.each(data, function (index, value) {
        tbRow = $('<tr></tr>').addClass(this.col_class);
        if (!CheckNullOrEmpty(this.col_id))
            tbRow.attr('codeid', this.col_id);
        if (this.col_value.length > 0 && typeof this.col_value != 'string')
            fncBuildBody_Cell(this.col_value, tbRow);
        else {
            var tdCell = $('<td></td>').attr('colspan', this.colspan).html(this.col_value);
            tbRow.append(tdCell);
        }
        //attribute row
        if (!CheckNullOrEmpty(this.col_attr)) {
            if (this.col_attr.length > 0) {
                $.each(this.col_attr, function () {
                    tbRow.attr(this.name, this.value);
                });
            }
        }
        tableCalendar.append(tbRow);
    });
}

function fncAddRowToRowBefore(tableCalendar, data, idRowBefore) {
    var tbRow;
    $.each(data, function (index, value) {
        tbRow = $('<tr></tr>').addClass(this.col_class);
        if (!CheckNullOrEmpty(this.col_id))
            tbRow.attr('codeid', this.col_id);
        if (this.col_value.length > 0 && typeof this.col_value != 'string')
            fncBuildBody_Cell(this.col_value, tbRow);
        else {
            var tdCell = $('<td></td>').attr('colspan', this.colspan).html(this.col_value);
            tbRow.append(tdCell);
        }
        //attribute row
        if (!CheckNullOrEmpty(this.col_attr)) {
            if (this.col_attr.length > 0) {
                $.each(this.col_attr, function () {
                    tbRow.attr(this.name, this.value);
                });
            }
        }
        if (isInt(idRowBefore)) {
            tableCalendar.find('.rows-box[codeid="' + idRowBefore + '"]').before(tbRow);
        }
        else
        {
            tableCalendar.find(idRowBefore).before(tbRow);
        }
    });
}

function fncAddRowAfterRow(tableCalendar, data, idRowAfter, rowObject) {
    var tbRow;
    var isFirst = false;
    $.each(data, function (index, value) {
        tbRow = $('<tr></tr>').addClass(this.col_class);
        if (!CheckNullOrEmpty(this.col_id))
            tbRow.attr('codeid', this.col_id);
        if (this.col_value.length > 0 && typeof this.col_value != 'string')
            fncBuildBody_Cell(this.col_value, tbRow);
        else {
            var tdCell = $('<td></td>').attr('colspan', this.colspan).html(this.col_value);
            tbRow.append(tdCell);
        }
        //attribute row
        if (!CheckNullOrEmpty(this.col_attr)) {
            if (this.col_attr.length > 0) {
                $.each(this.col_attr, function () {
                    tbRow.attr(this.name, this.value);
                });
            }
        }
        if (CheckNullOrEmpty(idRowAfter))
            tableCalendar.find(rowObject).after(tbRow);
        else {
            if (!isFirst) {
                tableCalendar.find('.rows-box[codeid="' + idRowAfter + '"]').after(tbRow);
                isFirst = true;
            }
            else
            {
                var rowAfter = tableCalendar.find('.rows-box[subparent="' + idRowAfter + '"]');
                rowAfter.last().after(tbRow);
            }
        }
    });
}

function fncBuildBody_Cell(data, tableCalendar) {
    var tbCell;
    $.each(data, function (index, value) {
        tbCell = $('<td></td>').addClass(this.col_class).html(this.col_value);
        if (!CheckNullOrEmpty(this.title))
        {
            tbCell.attr('title', Encoder.htmlDecode(this.title));
        }
        //colspan cell
        if(!CheckNullOrEmpty(this.colspan))
            if (parseInt(this.colspan) > 1)
                tbCell.attr('colspan', this.colspan);
        //attribute cell
        if (!CheckNullOrEmpty(this.col_attr))
            if (this.col_attr.length > 0) {
                $.each(this.col_attr, function () {
                    tbCell.attr(this.name, this.value);
                });
            }
        tableCalendar.append(tbCell);
    });
}

function fncUpdateSTT(table) {
    var colSTTs = table.find('tr td.stt');
    $.each(colSTTs, function (stt) {
        $(this).html(stt+1);
    });
}