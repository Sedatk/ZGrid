# ZGrid

Sample Usage for cshtml view

```cshtml
@using ZGrid
@{
    ViewBag.Title = "Sample";
}
<div class="row">
@{
    var grid = Html.GridFor<Grid.Shared.Entities.Person>()
        .Id("E94A4AFA-C7A2-425F-A886-34C432C8A821")
        .Columns(columns =>
        {
            columns.For(c => c.Id).Hidden();
            columns.For(c => c.Name);
            columns.For(c => c.BirthDate)
                .Title("Birth Date")
                .ViewTemplate(@"<span class=""label label-sm label-primary"">{model}</span>")
                .EditTemplate(@"<input data-date-format=""yyyy-mm-dd 00:00:00"" data-provide=""datepicker"" class=""form-control input-small"" value=""{model.BirthDate}""></input>")
                .Type("date");
            columns.For(c => c.IsInternal)
                .ViewTemplate(@"<span class=""label label-sm {(model?'label-success':'label-danger')}"">{(model?'Evet':'Hayır')}</span>");
            columns.For(c => c.Company)
                .ViewTemplate(@"<span class=""label label-sm label-success"">{model}</span>")
                .EditTemplate(
                    @"<select class=""form-control"" value=""{model.Company}"">
                        <option value=""microsoft"" {(model.Company=='microsoft'?'selected':'')}>Microsoft</option>
                        <option value=""google"" {(model.Company=='google'?'selected':'')}>Google</option>
                        <option value=""facebook"" {(model.Company=='facebook'?'selected':'')}>Facebook</option>
                    </select>");
        }).DataSource(source =>
            source.Ajax()
                .Read(url => url.Action("ListPersons"))
                .Create(url => url.Action("InsertNewPerson"))
                .Update(url => url.Action("UpdatePerson"))
                .SetPageLength(4)
        );

    @grid.RenderMarkup()
}
</div>

@section scripts
{
    <link href="~/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.css" rel="stylesheet" />
    <link href="~/assets/global/plugins/bootstrap-datepicker/css/bootstrap-datepicker3.min.css" rel="stylesheet" />
    <link href="~/assets/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css" rel="stylesheet" />


    <script src="~/assets/global/plugins/datatables/media/js/jquery.dataTables.min.js"></script>
    <script src="~/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js"></script>
    <script src="~/assets/global/scripts/datatable.js"></script>
    <script src="~/assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.min.js"></script>

    @grid.RenderScript()
}
```
