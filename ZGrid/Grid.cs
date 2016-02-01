﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZGrid.Model;

namespace ZGrid
{
    public class Grid<T>
    {
        private ColumnsManager<T> _columnsManager;
        private DatasourceManager _dataSourceManager;
        private readonly HtmlHelper _helper;
        private string _id = Guid.NewGuid().ToString();
        private readonly string _btnNewId = Guid.NewGuid().ToString();
        // ReSharper disable once StaticMemberInGenericType
        private static Regex ModelRegex { get; } = new Regex(@"(?<model>\{[^}]+\})", RegexOptions.Compiled);

        public Grid(HtmlHelper helper)
        {
            _helper = helper;
        }

        public Grid<T> Id(string id)
        {
            _id = id;
            return this;
        } 
        public MvcHtmlString RenderMarkup()
        {
            var builder = new StringBuilder();

            string tableToolbar = $@"
<div class=""table-toolbar"">
    <div class=""row"">
        <div class=""col-md-6"">
            <div class=""btn-group"">
                <button id = ""{_btnNewId}"" class=""btn green"">
                    Add New<i class=""fa fa-plus""></i>
                </button>
            </div>
        </div>
        <div class=""col-md-6"">
            <div class=""btn-group pull-right"">
                <button class=""btn dropdown-toggle"" data-toggle=""dropdown"">
                    Tools<i class=""fa fa-angle-down""></i>
                </button>
                <ul class=""dropdown-menu pull-right"">
                    <li>
                        <a href = ""javascript:;"" >
                            Print
                        </a>
                    </li>
                    <li>
                        <a href=""javascript:;"">
                            Save as PDF
                        </a>
                    </li>
                    <li>
                        <a href = ""javascript:;"" >
                            Export to Excel
                        </a>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</div>";
            builder.AppendLine(tableToolbar);
            builder.AppendLine($@"
<table id = ""{_id}"" class=""table table-striped table-bordered table-hover dataTable"">
    <thead>
        <tr>
");
            foreach(var column in _columnsManager.Columns)
            {
                builder.AppendLine($@"
<th>
    {column.HtmlHeader}
</th>");
            }
            builder.AppendLine($@"
        </tr>
    </thead>
    <tfoot>
        <tr>
");
            foreach (var column in _columnsManager.Columns)
            {
                builder.AppendLine($@"
<th>
    {column.HtmlHeader}
</th>");
            }
            builder.AppendLine($@"
        </tr>
    </tfoot>
    <tbody></tbody>
</table>
");
            return new MvcHtmlString(builder.ToString());   
        }
        private static string ConvertToJScriptString(string str)
        {
            var strs = str.Split(Environment.NewLine.ToCharArray(),StringSplitOptions.RemoveEmptyEntries);

            return "'"+string.Join(@"\" + Environment.NewLine, strs)+"'";
        }
        private string ReshapeTemplate(string template)
        {
            var coll = ModelRegex.Matches(template);
            var splits = ModelRegex.Split(template);

            var templateBuilder = new StringBuilder();

            for (var i = 0; i < splits.Length; ++i)
            {
                var part = splits[i];

                if (i > 0)
                {
                    templateBuilder.Append("+");
                }

                if (part.StartsWith("{") && part.EndsWith("}"))
                {
                    templateBuilder.Append(part.Substring(1, part.Length - 2));
                }
                else
                {
                    templateBuilder.Append(ConvertToJScriptString(part));
                }
            }
            return templateBuilder.ToString();
        }
        private IList<ColumnModel> GetColumns()
        {
            var columns = new List<ColumnModel>();
            Column column = null;
            
            for (var i = 0; i < _columnsManager.Columns.Count - 1; ++i)
            {
                column = _columnsManager.Columns[i];
                
                columns.Add(new ColumnModel()
                {
                    Name = column.HtmlHeader,
                    Data = column.Name,
                    Visible = !column.Hidden,
                    Type = column?.Type,
                    Render = column.ViewTemplate != null ? new JRaw($@"
                        function(model,type,row){{return {ReshapeTemplate(column.ViewTemplate)};}}") : null
                });
            }

            column = _columnsManager.Columns[_columnsManager.Columns.Count - 1];
            var lastColumnType = column.Type == null ? string.Empty : $@"""type"":""{column.Type}"",";

            columns.Add(new ColumnModel()
            {
                Name = column.HtmlHeader,
                Data = column.Name,
                Visible = true,
                Type = column?.Type,
                Render = new JRaw($@"function(model,type,row){{
                    var vMenu='<div class=""visible-menu"">\
                        <div class=""btn-group btn-group-xs btn-group-solid"">\
                            <button type = ""button"" class=""btn btn-success tbEdit"">Edit<i class=""fa fa-edit""></i></button>\
                            <button type = ""button"" class=""btn btn-danger"">Delete<i class=""fa fa-trash""></i></button>\
                        </div>\
                    </div>';    
                    return vMenu+{(column.ViewTemplate!=null?ReshapeTemplate(column.ViewTemplate):"model")};
                }}")
            });

            return columns;
        } 
        public MvcHtmlString RenderScript()
        {
            var urlHelper = new UrlHelper(_helper.ViewContext.RequestContext);
            var builder = new StringBuilder();
            const string styles =
@"<style type=""text/css"">
        table.dataTable td,
        table.dataTable th {
            -webkit-box-sizing:content-box;
            -moz-box-sizing:content-box;
            box-sizing:content-box;
            position:relative;
            z-index:10;
            }
        .visible-menu {
            padding-top:7px;
            text-align:center;
            z-index:20;
            position:absolute;
            right:0;
            top:0;
            background-color: #eee;
            width:100%;
            height:100%;
            display:none;
        }
</style>";
            var styleInjected = false;
                //_helper.ViewContext.ContainsKey("_style_injected") 
                //&& (bool) _helper.ViewContext.TempData["_style_injected"];

            if (!styleInjected)
            {
                _helper.ViewContext.TempData["_style_injected"] = true;
                builder.AppendLine(styles);
            }
            
            builder.AppendLine(@"<script type=""text/javascript"">");
            builder.AppendLine($@"(function() {{");
            builder.AppendLine("var nEditing = null");
            builder.AppendLine("var nNew = false;");
            builder.AppendLine($@"var table = $('#{_id}');");

            var table=JsonConvert.SerializeObject(new
            {
                serverSide=true,
                ajax=new
                {
                    url= _dataSourceManager.AjaxSource.ReadUrl,
                    type="POST",
                    data=new JRaw(@"function(d) {
                        return JSON.stringify(d);
                    }")
                },
                language=new
                {
                    lengthMenu= "Her sayfa için _MENU_ kayıt göster",//Display _MENU_ records per page
                    zeroRecords= "Kayıt bulunamadı",//Nothing found - sorry
                    info = "_PAGES_ sayfadan _PAGE_. sayfa görüntüleniyor",//Showing page _PAGE_ of _PAGES_
                    infoEmpty = "Herhangi bir kayıt mevcut değil",//No records available
                    infoFiltered = "(Sonuçlar _MAX_ tane kayıttan filtrelendi)",//""(filtered from _MAX_ total records)""
                    paginate =new
                    {
                        previous= "Önceki sayfa",
                        next= "Sonraki sayfası"
                    },
                    search= "Arama"
                },
                pagingType= "full_numbers",//full
                filter= false,
                //lengthChange=false,
                stateSave=true,
                pageLength =_dataSourceManager.AjaxSource.PageLength,
                columns=GetColumns()
            });
            string scriptTable =$@"var oTable = table.dataTable({table});";

            builder.AppendLine(scriptTable);

            string editButtonEventHandler = $@"
            $(""#{_id}"").on({{
                mouseenter: function() {{
                    if (nEditing === null)
                    {{
                            $(this).find("".visible-menu"").show();
                    }}
                }},
                mouseleave: function() {{
                    if (nEditing === null)
                    {{
                            $(this).find("".visible-menu"").hide();
                    }}
                }}
            }}, ""tr""); 
";
            builder.AppendLine(editButtonEventHandler);
            
            string editEventHandler=$@"
            $(""#{_id}"").on({{
                click: function(e) {{e.preventDefault();
                    var nRow = $(this).parents('tr')[0];
                    nEditing = nRow;
                    nNew = false;
                    editRow(oTable, nRow); 
                }}
            }}, "".tbEdit"");
";
            builder.AppendLine(editEventHandler);

            string cancelEventHandler = $@"
            $(""#{_id}"").on({{
                click: function(e) {{
                    e.preventDefault();
                    var nRow = $(this).parents('tr')[0];
                    editRow(oTable, nRow);

                    if (nNew)
                    {{
                        oTable.fnDeleteRow(nRow);
                    }}
                    else {{
                        restoreRow(oTable, nRow);
                    }}
                    nEditing = null;
                    nNew = false;
                }}
            }}, "".tbCancel"");
";
            builder.AppendLine(cancelEventHandler);

            string saveEventHandler = $@"
            $(""#{_id}"").on({{
                click: function(e) {{
                    e.preventDefault();
                    
                    var nRow = $(this).parents('tr')[0];

                    if (nEditing !== null && nEditing != nRow){{
                        restoreRow(oTable, nEditing);
                        editRow(oTable, nRow);
                        nEditing = nRow;
                    }}
                    else if (nEditing == nRow){{
                        saveRow(oTable, nEditing);
                        nEditing = null;
                    }}
                    else {{
                        editRow(oTable, nRow);
                        nEditing = nRow;
                    }}
                }}
            }}, "".tbSave"");
";
            builder.AppendLine(saveEventHandler);

            string fnRestore = $@"
            function restoreRow(oTable, nRow) {{
                //var aData = oTable.fnGetData(nRow);
                //var jqTds = $('>td', nRow);

                //for (var i = 0, iLen = jqTds.length; i < iLen; i++) {{
                    //oTable.fnUpdate(aData[i], nRow, i, false);
                //}}

                oTable.fnDraw(false);
            }}
";
            builder.AppendLine(fnRestore);
            StringBuilder sbEditRow = new StringBuilder();
            sbEditRow.AppendLine(
                $@"
             function editRow(oTable, nRow) {{
                var model = oTable.fnGetData(nRow);

                var jqTds = $('>td', nRow);
                ");
            for (int i = 0,act=0; i < _columnsManager.Columns.Count; ++i)
            {
                if (act > 0)
                {
                    sbEditRow.Append(";");
                }

                var column = _columnsManager.Columns[i];
                if (column.Hidden)
                {
                    continue;
                }

                if (!column.IsReadOnly)
                {
                    if (column.EditTemplate == null)
                    {
                        sbEditRow.Append(
                            $@"jqTds[{act}].innerHTML = '<input type=""text"" class=""form-control input-small"" value=""' + model.{column
                                .Name} + '"">'");
                    }
                    else
                    {
                        sbEditRow.Append(
                            $@"jqTds[{act}].innerHTML = {ReshapeTemplate(column.EditTemplate)}");
                    }
                }

                act++;
            }
            sbEditRow.Remove(sbEditRow.Length - 1, 1);
            sbEditRow.Append(
                $@"\
                <div class=""btn-group btn-group-xs btn-group-solid"">\
                    <button type=""button"" class=""btn btn-success tbSave"">Save <i class=""fa fa-save""></i></button>\
                    <button type=""button"" class=""btn btn-danger tbCancel"">Cancel <i class=""fa fa-undo""></i></button>\
                </div>\
                ';
            }}");


            builder.AppendLine(sbEditRow.ToString());
            StringBuilder sbSaveRow=new StringBuilder();
            sbSaveRow.AppendLine(
                $@"
            function saveRow(oTable, nRow) {{
                var jqInputs = $('input,select', nRow);");

            for (int i = 0, act = 0; i < _columnsManager.Columns.Count; ++i)
            {
                var column = _columnsManager.Columns[i];
                if (column.Hidden)
                {
                    continue;
                }

                if (!column.IsReadOnly)
                {
                    sbSaveRow.AppendLine($"oTable.fnUpdate(jqInputs[{act}].value, nRow, {i}, false);");
                }
                act++;
            }

            sbSaveRow.AppendLine(
                $@"var aData = oTable.fnGetData(nRow);

                $.post(nNew ? ""{_dataSourceManager.AjaxSource
                    .CreateUrl}"" : ""{_dataSourceManager.AjaxSource.UpdateUrl}"", aData,
                    function(result) {{
                        oTable.fnDraw(false);
                    }});");
            builder.AppendLine(sbSaveRow.ToString());
            builder.AppendLine(@"}");
            var newRow = new StringBuilder();
            foreach (var column in this._columnsManager.Columns)
            {
                newRow.AppendLine($@"""{column.Name}"":""""");
                newRow.Append(",");
            }
            newRow.Remove(newRow.Length - 1, 1);

            string fnNewRow = $@"
             $('#{_btnNewId}').click(function(e) {{
                e.preventDefault();

                if (nNew && nEditing) {{
                    if (confirm(""Previous row not saved. Do you want to save it ?"")) {{
                        saveRow(oTable, nEditing); // save
                        $(nEditing).find(""td:first"").html(""Untitled"");
                        nEditing = null;
                        nNew = false;

                    }} else {{
                        oTable.fnDeleteRow(nEditing); // cancel
                        nEditing = null;
                        nNew = false;

                        return;
                    }}
                }}

                var aiNew = oTable.fnAddData({{
                    {newRow.ToString()}
                }}, false);

               
                var nRow = oTable.fnGetNodes(aiNew[0]);
                $(""#{_id} tbody"").prepend(nRow);

                editRow(oTable, nRow);
                nEditing = nRow;
                nNew = true;
            }});
";
            builder.AppendLine(fnNewRow);
            builder.AppendLine($@"}})();");
            builder.AppendLine(@" </script>");
            
            return new MvcHtmlString(builder.ToString());
        }

        public Grid<T> Columns(Action<ColumnsManager<T>> actColumns)
        {
            actColumns(_columnsManager = new ColumnsManager<T>());
            return this;
        }

        public Grid<T> DataSource(Action<DatasourceManager> dataSource)
        {
            var urlHelper = new UrlHelper(_helper.ViewContext.RequestContext);
            dataSource(_dataSourceManager= new DatasourceManager(urlHelper));
            return this;
        }
    }
}