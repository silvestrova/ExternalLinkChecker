<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="ExternalLinksChecker.Dialogs.Report" %>

<%@ Register Assembly="Sitecore.Kernel" Namespace="Sitecore.Web.UI.HtmlControls" TagPrefix="sc" %>
<%@ Register Assembly="Sitecore.Kernel" Namespace="Sitecore.Web.UI.WebControls" TagPrefix="sc" %>
<%@ Register Assembly="Sitecore.Kernel" Namespace="Sitecore.Web.UI.WebControls.Ribbons" TagPrefix="sc" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ca" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
    <head id="Head1" runat="server">
        <title>Sitecore</title>
        <sc:stylesheet src="Content Manager.css" devicedependant="true" runat="server" />
        <sc:stylesheet src="Ribbon.css" devicedependant="true" runat="server" />
        <sc:stylesheet src="Grid.css" devicedependant="true" runat="server" />
        <sc:script src="/sitecore/shell/Controls/InternetExplorer.js" runat="server" />
        <sc:script src="/sitecore/shell/Controls/Sitecore.js" runat="server" />
        <sc:script src="/sitecore/shell/Controls/SitecoreObjects.js" runat="server" />
        <sc:script src="/sitecore/shell/Applications/Content Manager/Content Editor.js" runat="server" />
        <style type="text/css">
            html body { overflow: hidden; }
        </style>

        <script type="text/javascript" language="javascript">

            function OnResize() {
                var doc = $(document.documentElement);
                var ribbon = $("RibbonContainer");
                var grid = $("GridContainer");

                grid.style.height = doc.getHeight() - ribbon.getHeight() + 'px';
                grid.style.width = doc.getWidth() + 'px';

                Results.render();


              //  setTimeout(".render()", 150);
            }

            function refresh() {
                Results.scHandler.refresh();
            }

            function searchByCode(string) {

                var x = Results.search(string, false);
                if (!x) {
                    Results.setProperty('EmptyGridText', '<b>No Matches</b>');
                    Results.render();
                }


            }

            function SelectSearchElement(id) {

                if ($(id)) {

                    $(id).className = "scRibbonToolbarSmallButtonDown";
                }
            }

            function UnSelectSearchElements() {

                var elements = document.getElementsByClassName('scRibbonToolbarSmallButtonDown');
                for (var i = 0; i < elements.length; ++i) {
                    var item = elements[i];
                    item.className = "scRibbonToolbarSmallButton";
                }
            }

            function search(code, id) {
                console.log(code);
                console.log(id);
                searchByCode(code);
                UnSelectSearchElements();
                SelectSearchElement(id);
            }

            function OnLoad() {
                OnResize();
            }

            window.onresize = OnResize;
        </script>

    </head>
    <body style="background: transparent; height: 100%" id="PageBody" runat="server">
        <form id="ExternalLinksCheckerReport" runat="server">
            <sc:continuationmanager runat="server" />
            <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td id="RibbonContainer">
                        <sc:ribbon runat="server" id="Ribbon" />
                    </td>
                </tr>

                <tr>
                    <td id="GridCell" height="100%" style="background: #e9e9e9; padding: 0px;">
                        <sc:border runat="server" id="GridContainer">
                            <ca:Grid id="Results" 
                                     AutoFocusSearchBox="false"
                                     RunningMode="Callback" 
                                     CssClass="Grid" 
                                     ShowHeader="true"
                                     HeaderCssClass="GridHeader" 

                                     FillContainer="true"
                                     FooterCssClass="GridFooter" 

                                     GroupByText = ""
                                     GroupingNotificationText = ""
                                     GroupByCssClass="GroupByCell"
                                     GroupByTextCssClass="GroupByText"
                                     GroupBySortAscendingImageUrl="group_asc.gif"
                                     GroupBySortDescendingImageUrl="group_desc.gif"
                                     GroupBySortImageWidth="10"
                                     GroupBySortImageHeight="10"
                                     GroupingMode="ConstantRecords"

                                     GroupingNotificationTextCssClass="GridHeaderText"
                                     GroupingPageSize="20"
                                     EmptyGridText = "No one link was checked"
                                     PageSize="30" 
                                     PagerStyle="Slider"
                                     PagerTextCssClass="GridFooterText"
                                     PagerButtonWidth="41"
                                     PagerButtonHeight="22"
                                     PagerImagesFolderUrl="/sitecore/shell/themes/standard/componentart/grid/pager/"
            
                                     RenderSearchEngineStamp="True"
                                     ShowSearchBox="true"
                                     SearchTextCssClass="GridHeaderText"
                                     SearchBoxCssClass="SearchBox"
      	    
                                     SliderHeight="20"
                                     SliderWidth="150" 
                                     SliderGripWidth="9" 
                                     SliderPopupOffsetX="20"
            
                                     TreeLineImagesFolderUrl="/sitecore/shell/themes/standard/componentart/grid/lines/" 
                                     TreeLineImageWidth="22" 
                                     TreeLineImageHeight="19" 
            
                                     PreExpandOnGroup="false"
                                     ImagesBaseUrl="/sitecore/shell/themes/standard/componentart/grid/" 
                                     IndentCellWidth="22" 

                                     LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                     LoadingPanelPosition="MiddleCenter"
            
                                     runat="server">
            
                                <Levels>
                                    <ca:GridLevel 
                                        DataKeyField="scGridID"
                                        ShowTableHeading="false" 
                                        ShowSelectorCells="false" 
                                        RowCssClass="Row" 
                                        ColumnReorderIndicatorImageUrl="reorder.gif"
                                        DataCellCssClass="DataCell" 
                                        HeadingCellCssClass="HeadingCell" 
                                        HeadingCellHoverCssClass="HeadingCellHover" 
                                        HeadingCellActiveCssClass="HeadingCellActive" 
                                        HeadingRowCssClass="HeadingRow" 
                                        HeadingTextCssClass="HeadingCellText"
                                        GroupHeadingClientTemplateId="GroupHeadingTemplate"
                                        SelectedRowCssClass="SelectedRow"
                                        GroupHeadingCssClass="GroupHeading" 
                                        SortAscendingImageUrl="asc.gif" 
                                        SortDescendingImageUrl="desc.gif" 
                                        SortImageWidth="13" 
                                        SortImageHeight="13">
                                        <Columns>
                                            <ca:GridColumn DataField="scGridID" Visible="false" IsSearchable="false" />
                                            <ca:GridColumn DataField="Description" Visible="false" IsSearchable="false" />
                                            <ca:GridColumn DataField="ItemLink" Visible="false" IsSearchable="false" />
                                            <ca:GridColumn DataField="FieldHTML" Visible="false" IsSearchable="false" />
                                            <ca:GridColumn DataField="Code" AllowSorting="true" IsSearchable="true" AllowGrouping="false" SortedDataCellCssClass="SortedDataCell" HeadingText="Http Status Code" AllowHtmlContent="false" DataCellClientTemplateId="CodeTemplate" />
                                            <ca:GridColumn DataField="FieldName" AllowSorting="true" IsSearchable="true" AllowGrouping="false" SortedDataCellCssClass="SortedDataCell" HeadingText="Field Name(Type)" AllowHtmlContent="False" DataCellClientTemplateId="FieldTemplate"  />
                                            <ca:GridColumn DataField="LinkText" AllowSorting="false" IsSearchable="true" AllowGrouping="false" SortedDataCellCssClass="SortedDataCell" HeadingText="Link Text" AllowHtmlContent="False" DataCellClientTemplateId="LinkTemplate" />                  
                                            <ca:GridColumn DataField="ContentItem" AllowSorting="true" IsSearchable="true" AllowGrouping="false" SortedDataCellCssClass="SortedDataCell" HeadingText="Content Item" AllowHtmlContent="False" DataCellClientTemplateId="ItemTemplate" />

                                        </Columns>
                                    </ca:GridLevel>
                                </Levels>

                                <ClientTemplates>

                                    <ca:ClientTemplate Id="CodeTemplate">
                                        <a href="#"onclick=" window.alert('## DataItem.GetMember('Description').Value ##') ">  ## DataItem.GetMember('Code').Value ##</a>
                                    </ca:ClientTemplate>              
             
                                    <ca:ClientTemplate Id="LinkTemplate">
                                        <a href="#" onclick=" window.open('## DataItem.GetMember('LinkText').Value##', 'uri_## DataItem.GetMember('LinkText').Value##', 'height=700,width=800,left=10,top=10,resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no,status=yes') ">  ## DataItem.GetMember('LinkText').Value ##</a>
                                    </ca:ClientTemplate>
              
                                    <ca:ClientTemplate Id="FieldTemplate">
                                        <a href="#"onclick=" window.alert('## DataItem.GetMember('FieldHTML').Value ##') ">  ## DataItem.GetMember('FieldName').Value ##</a>
                                    </ca:ClientTemplate>  

                                    <ca:ClientTemplate Id="ItemTemplate">
                                        <a href="#"onclick=" window.open('## DataItem.GetMember('ItemLink').Value##', 'uri_## DataItem.GetMember('ItemLink').Value##', 'height=700,width=800,left=10,top=10,resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no,status=yes') ">  ## DataItem.GetMember('ContentItem').Value ##</a>
                                    </ca:ClientTemplate>                        
                                    <ca:ClientTemplate Id="LoadingFeedbackTemplate">
                                        <table cellspacing="0" cellpadding="0" border="0">
                                            <tr>
                                                <td style="font-size: 10px;"><sc:Literal Text="Loading..." runat="server" />;</td>
                                                <td><img src="/sitecore/shell/themes/standard/componentart/grid/spinner.gif" width="16" height="16" border="0"></td>
                                            </tr>
                                        </table>
                                    </ca:ClientTemplate>
             
                                </ClientTemplates>
                            </ca:Grid>
                        </sc:border>
                    </td>
                </tr>
            </table>
        </form>
    </body>
</html>