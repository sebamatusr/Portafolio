<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResumenJefeInterno.aspx.cs" Inherits="PortafolioWeb.Reportes.ResumenJefeInterno" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body style="height: 460px">
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField ID="unidad_hidden" runat="server" OnValueChanged="id_unidad_ValueChanged" />
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT VISTAHERMOSA.SOLICITUD.RUT_FUNCIONARIO, VISTAHERMOSA.FUNCIONARIO.NOMBRE, VISTAHERMOSA.FUNCIONARIO.APELLIDO_PATERNO, VISTAHERMOSA.FUNCIONARIO.APELLIDO_MATERNO, COUNT(*) AS CANTIDAD, VISTAHERMOSA.UNIDAD.NOMBRE_UNIDAD, VISTAHERMOSA.ESTADO.DESCRIPCION, VISTAHERMOSA.TIPO_SOLICITUD.DESCRIPCION AS TIPO_SOLICITUD FROM VISTAHERMOSA.SOLICITUD INNER JOIN VISTAHERMOSA.FUNCIONARIO ON VISTAHERMOSA.SOLICITUD.RUT_FUNCIONARIO = VISTAHERMOSA.FUNCIONARIO.RUT INNER JOIN VISTAHERMOSA.TIPO_SOLICITUD ON VISTAHERMOSA.SOLICITUD.ID_TIPOSOLICITUD = VISTAHERMOSA.TIPO_SOLICITUD.ID_TIPOSOLICITUD INNER JOIN VISTAHERMOSA.UNIDAD ON VISTAHERMOSA.FUNCIONARIO.ID_UNIDAD = VISTAHERMOSA.UNIDAD.ID_UNIDAD INNER JOIN VISTAHERMOSA.ESTADO ON VISTAHERMOSA.SOLICITUD.ID_ESTADO = VISTAHERMOSA.ESTADO.ID_ESTADO WHERE (VISTAHERMOSA.UNIDAD.ID_UNIDAD = :IDUNIDAD) GROUP BY VISTAHERMOSA.SOLICITUD.RUT_FUNCIONARIO, VISTAHERMOSA.FUNCIONARIO.NOMBRE, VISTAHERMOSA.FUNCIONARIO.APELLIDO_PATERNO, VISTAHERMOSA.FUNCIONARIO.APELLIDO_MATERNO, VISTAHERMOSA.UNIDAD.NOMBRE_UNIDAD, VISTAHERMOSA.ESTADO.DESCRIPCION, VISTAHERMOSA.TIPO_SOLICITUD.DESCRIPCION">
        <SelectParameters>
                <asp:Parameter Name="IDUNIDAD" Type="Int32" DefaultValue="0" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>


        


        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT        VISTAHERMOSA.UNIDAD.NOMBRE_UNIDAD, derivedtbl_1.EXPR1 AS RECHAZADAS, derivedtbl_2.EXPR1 AS APROBADAS, derivedtbl_3.EXPR1 AS TOTALES
FROM            VISTAHERMOSA.FUNCIONARIO INNER JOIN
                         VISTAHERMOSA.SOLICITUD ON VISTAHERMOSA.FUNCIONARIO.RUT = VISTAHERMOSA.SOLICITUD.RUT_FUNCIONARIO INNER JOIN
                         VISTAHERMOSA.UNIDAD ON VISTAHERMOSA.FUNCIONARIO.ID_UNIDAD = VISTAHERMOSA.UNIDAD.ID_UNIDAD,
                             (SELECT        COUNT(SOLICITUD_1.ID_SOLICITUD) AS EXPR1
                               FROM            VISTAHERMOSA.FUNCIONARIO FUNCIONARIO_1 INNER JOIN
                                                         VISTAHERMOSA.SOLICITUD SOLICITUD_1 ON FUNCIONARIO_1.RUT = SOLICITUD_1.RUT_FUNCIONARIO INNER JOIN
                                                         VISTAHERMOSA.UNIDAD UNIDAD_1 ON FUNCIONARIO_1.ID_UNIDAD = UNIDAD_1.ID_UNIDAD
                               WHERE        (SOLICITUD_1.ID_ESTADO = 12)
                               GROUP BY UNIDAD_1.NOMBRE_UNIDAD) derivedtbl_1,
                             (SELECT        COUNT(SOLICITUD_2.ID_SOLICITUD) AS EXPR1
                               FROM            VISTAHERMOSA.FUNCIONARIO FUNCIONARIO_2 INNER JOIN
                                                         VISTAHERMOSA.SOLICITUD SOLICITUD_2 ON FUNCIONARIO_2.RUT = SOLICITUD_2.RUT_FUNCIONARIO INNER JOIN
                                                         VISTAHERMOSA.UNIDAD UNIDAD_2 ON FUNCIONARIO_2.ID_UNIDAD = UNIDAD_2.ID_UNIDAD
                               WHERE        (SOLICITUD_2.ID_ESTADO = 11)
                               GROUP BY UNIDAD_2.NOMBRE_UNIDAD) derivedtbl_2,
                             (SELECT        COUNT(SOLICITUD_3.ID_SOLICITUD) AS EXPR1
                               FROM            VISTAHERMOSA.FUNCIONARIO FUNCIONARIO_3 INNER JOIN
                                                         VISTAHERMOSA.SOLICITUD SOLICITUD_3 ON FUNCIONARIO_3.RUT = SOLICITUD_3.RUT_FUNCIONARIO INNER JOIN
                                                         VISTAHERMOSA.UNIDAD UNIDAD_3 ON FUNCIONARIO_3.ID_UNIDAD = UNIDAD_3.ID_UNIDAD
                               GROUP BY UNIDAD_3.NOMBRE_UNIDAD) derivedtbl_3
WHERE        (VISTAHERMOSA.UNIDAD.ID_UNIDAD = :IDUNIDAD1)
GROUP BY VISTAHERMOSA.UNIDAD.NOMBRE_UNIDAD, derivedtbl_1.EXPR1, derivedtbl_2.EXPR1, derivedtbl_3.EXPR1">
            <SelectParameters>
                <asp:Parameter Name="IDUNIDAD1" Type="Int32" DefaultValue="0" />
            </SelectParameters>
        </asp:SqlDataSource>


        


        <br />

        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="572px" Height="385px" style="margin-bottom: 1px" SizeToReportContent="True">
            <LocalReport ReportPath="Reportes\ReporteCuantitativo.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="SqlDataSource1" Name="ResumenCuantitativo" />
                    <rsweb:ReportDataSource DataSourceId="SqlDataSource2" Name="DS_ResumenUI" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
    
    </div>
    </form>
</body>
</html>
