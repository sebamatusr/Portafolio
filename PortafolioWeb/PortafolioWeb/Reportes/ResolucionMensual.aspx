<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResolucionMensual.aspx.cs" Inherits="PortafolioWeb.Reportes.ResolucionMensual" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT        VISTAHERMOSA.SOLICITUD.RUT_FUNCIONARIO, VISTAHERMOSA.FUNCIONARIO.NOMBRE, VISTAHERMOSA.FUNCIONARIO.APELLIDO_PATERNO, VISTAHERMOSA.FUNCIONARIO.APELLIDO_MATERNO, COUNT(*) AS CANTIDAD, 
                         VISTAHERMOSA.UNIDAD.NOMBRE_UNIDAD, VISTAHERMOSA.ESTADO.DESCRIPCION, VISTAHERMOSA.TIPO_SOLICITUD.DESCRIPCION AS TIPO_SOLICITUD
FROM            VISTAHERMOSA.SOLICITUD INNER JOIN
                         VISTAHERMOSA.FUNCIONARIO ON VISTAHERMOSA.SOLICITUD.RUT_FUNCIONARIO = VISTAHERMOSA.FUNCIONARIO.RUT INNER JOIN
                         VISTAHERMOSA.TIPO_SOLICITUD ON VISTAHERMOSA.SOLICITUD.ID_TIPOSOLICITUD = VISTAHERMOSA.TIPO_SOLICITUD.ID_TIPOSOLICITUD INNER JOIN
                         VISTAHERMOSA.UNIDAD ON VISTAHERMOSA.FUNCIONARIO.ID_UNIDAD = VISTAHERMOSA.UNIDAD.ID_UNIDAD INNER JOIN
                         VISTAHERMOSA.ESTADO ON VISTAHERMOSA.SOLICITUD.ID_ESTADO = VISTAHERMOSA.ESTADO.ID_ESTADO
                         WHERE VISTAHERMOSA.UNIDAD.ID_UNIDAD = :IDUNIDAD AND VISTAHERMOSA.SOLICITUD.ID_ESTADO != 10 AND VISTAHERMOSA.SOLICITUD.ESTADO = 1
GROUP BY VISTAHERMOSA.SOLICITUD.RUT_FUNCIONARIO, VISTAHERMOSA.FUNCIONARIO.NOMBRE, VISTAHERMOSA.FUNCIONARIO.APELLIDO_PATERNO, VISTAHERMOSA.FUNCIONARIO.APELLIDO_MATERNO, 
                         VISTAHERMOSA.UNIDAD.NOMBRE_UNIDAD, VISTAHERMOSA.ESTADO.DESCRIPCION, VISTAHERMOSA.TIPO_SOLICITUD.DESCRIPCION">
            <SelectParameters>
                <asp:Parameter Name="IDUNIDAD" Type="Int32" DefaultValue="0" />
            </SelectParameters>
        </asp:SqlDataSource>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" SizeToReportContent="true">
            <LocalReport ReportPath="Reportes\ResolucionMensual.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="SqlDataSource1" Name="DS_ResolucionMensual" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
    
    </div>
    </form>
</body>
</html>
