function populateDataTable(columnNames, entries) {
    const columns = columnNames.map(name => { return { title: name } });
    columns.push({ title: "", orderable: false });

    const data = entries.map(({ values, actions }) => {
        const row = [...values];

        row.push(
            `<td class="text-nowrap">
                        <a href="${actions.details}" class="btn icon-btn-sm btn-outline-primary m-1">
                            <i class="fas fa-info"></i>
                        </a>

                        <a href="${actions.edit}" class="btn icon-btn-sm btn-outline-primary m-1">
                            <i class="fas fa-edit"></i>
                        </a>

                        <a href="${actions.delete}" class="btn icon-btn-sm btn-outline-danger m-1">
                            <i class="fas fa-trash"></i>
                        </a>
                    </td>
                    `
        );

        return row;
    });

    var t = $('#datatable')
        .DataTable({
            language: {
                url: "/areas/admin/plugins/datatables/locale/ar.json"
            },
            paging: true,
            lengthChange: false,
            searching: true,
            ordering: true,
            autoWidth: false,
            responsive: true,
            serverSide: false,
            initComplete: function () {
                const api = this.api();

                const addButtons = buttons => {
                    new $.fn.dataTable.Buttons(api, { buttons });
                    api.buttons().container().appendTo('#tableButtons');
                }

                addButtons([
                    {
                        extend: "colvis",
                        columns: "th:not(:last-child)"
                    }
                ]);
            },
            columns,
            data,
        });


    $("#search").on("input", (e) => {
        const q = e.target.value;
        t.column(0).search(q).draw();
    });
}
