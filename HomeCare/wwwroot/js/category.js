﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/category/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "50%" },
            { "data": "displayOrder", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center"> 
                                <a href="/Admin/category/Upsert/${data}" class='btn btn-success text-white' style='cursor:pointer; width:120px;' >
                                    <i class='far fa-edit'></i> Edit
                                </a>
                                &nbsp;
                                <a class='btn btn-danger text-white' style='cursor:pointer; width:120px;' onclick=Delete('/admin/category/Delete/'+${data})>
                                   <i class='far fa-trash-alt'></i> Delete
                                </a>
                            </div>
                            `;
                }, "width": "30%"
            }
        ],
        "language": {
            "emptyTable": "No records found."
        },
        "width": "100%"
    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to delete this category?",
        text: "This action will permanently delete the category. ",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes",
        closeOnconfirm: true
    }, function () {
        $.ajax({
            type: 'DELETE',
            url: url,
            success: function (data) {
                if (data.success) {
                    toastr.options = {
                        "positionClass": "toast-bottom-right",
                    };
                    toastr.success(data.message);
                    dataTable.ajax.reload();
                }
                else {
                    toastr.options = {
                        "positionClass": "toast-bottom-right",
                    };
                    toastr.error(data.message);
                }
            }
        });
    });
}