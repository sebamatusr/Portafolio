<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResumenCuantitativo.aspx.cs" Inherits="PortafolioWeb.Reportes.ResumenCuantitativo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
   
    <form id="form1" runat="server">
    <div>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
            <LocalReport ReportPath="Reportes\ReporteCuantitativo.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="SqlDataSource1" Name="ResumenCuantitativo" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT        VISTAHERMOSA.SOLICITUD.RUT_FUNCIONARIO, VISTAHERMOSA.FUNCIONARIO.NOMBRE, VISTAHERMOSA.FUNCIONARIO.APELLIDO_PATERNO, VISTAHERMOSA.FUNCIONARIO.APELLIDO_MATERNO, COUNT(*) AS CANTIDAD, 
                         VISTAHERMOSA.UNIDAD.NOMBRE_UNIDAD, VISTAHERMOSA.ESTADO.DESCRIPCION, VISTAHERMOSA.TIPO_SOLICITUD.DESCRIPCION AS TIPO_SOLICITUD
FROM            VISTAHERMOSA.SOLICITUD INNER JOIN
                         VISTAHERMOSA.FUNCIONARIO ON VISTAHERMOSA.SOLICITUD.RUT_FUNCIONARIO = VISTAHERMOSA.FUNCIONARIO.RUT INNER JOIN
                         VISTAHERMOSA.TIPO_SOLICITUD ON VISTAHERMOSA.SOLICITUD.ID_TIPOSOLICITUD = VISTAHERMOSA.TIPO_SOLICITUD.ID_TIPOSOLICITUD INNER JOIN
                         VISTAHERMOSA.UNIDAD ON VISTAHERMOSA.FUNCIONARIO.ID_UNIDAD = VISTAHERMOSA.UNIDAD.ID_UNIDAD INNER JOIN
                         VISTAHERMOSA.ESTADO ON VISTAHERMOSA.SOLICITUD.ID_ESTADO = VISTAHERMOSA.ESTADO.ID_ESTADO
GROUP BY VISTAHERMOSA.SOLICITUD.RUT_FUNCIONARIO, VISTAHERMOSA.FUNCIONARIO.NOMBRE, VISTAHERMOSA.FUNCIONARIO.APELLIDO_PATERNO, VISTAHERMOSA.FUNCIONARIO.APELLIDO_MATERNO, 
                         VISTAHERMOSA.UNIDAD.NOMBRE_UNIDAD, VISTAHERMOSA.ESTADO.DESCRIPCION, VISTAHERMOSA.TIPO_SOLICITUD.DESCRIPCION"></asp:SqlDataSource>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    </div>
    </form>
</body>
</html>
