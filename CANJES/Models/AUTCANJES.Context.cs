

//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------


namespace CANJES.Models
{

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using System.Data.Entity.Core.Objects;
using System.Linq;


public partial class AUTCANJESEntities : DbContext
{
    public AUTCANJESEntities()
        : base("name=AUTCANJESEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<TBL_Anio> TBL_Anio { get; set; }

    public virtual DbSet<TBL_Casa_Proveedor_SAP> TBL_Casa_Proveedor_SAP { get; set; }

    public virtual DbSet<TBL_Centros> TBL_Centros { get; set; }

    public virtual DbSet<TBL_Dias> TBL_Dias { get; set; }

    public virtual DbSet<TBL_EncabezadoSAP> TBL_EncabezadoSAP { get; set; }

    public virtual DbSet<TBL_MaestrosCanjes> TBL_MaestrosCanjes { get; set; }

    public virtual DbSet<TBL_Meses> TBL_Meses { get; set; }

    public virtual DbSet<TBL_Sociedades> TBL_Sociedades { get; set; }

    public virtual DbSet<TBL_UsuariosCanjes> TBL_UsuariosCanjes { get; set; }

    public virtual DbSet<TBL_UsuariosCanjes_Estado> TBL_UsuariosCanjes_Estado { get; set; }

    public virtual DbSet<TBL_UsuariosCanjes_Roles> TBL_UsuariosCanjes_Roles { get; set; }

    public virtual DbSet<VSP_Autorizaciones_Detalle> VSP_Autorizaciones_Detalle { get; set; }

    public virtual DbSet<VSP_Autorizaciones_Encabezado> VSP_Autorizaciones_Encabezado { get; set; }

    public virtual DbSet<VSP_Casa_ClientePrv_Sociedad> VSP_Casa_ClientePrv_Sociedad { get; set; }

    public virtual DbSet<VSP_Casa_Proveedor_SAP> VSP_Casa_Proveedor_SAP { get; set; }

    public virtual DbSet<VSP_CASA_SAP> VSP_CASA_SAP { get; set; }

    public virtual DbSet<VSP_DatosUsuarios> VSP_DatosUsuarios { get; set; }

    public virtual DbSet<VSP_MATERIALES_CASAS> VSP_MATERIALES_CASAS { get; set; }

    public virtual DbSet<VSP_PROVEEDORES_SAP> VSP_PROVEEDORES_SAP { get; set; }

    public virtual DbSet<VSP_ReporteLogs1> VSP_ReporteLogs1 { get; set; }

    public virtual DbSet<VSP_ReporteLogsCanjes> VSP_ReporteLogsCanjes { get; set; }

    public virtual DbSet<TBL_DetalleSAP> TBL_DetalleSAP { get; set; }

    public virtual DbSet<VSP_ReporteLogs> VSP_ReporteLogs { get; set; }

    public virtual DbSet<TBL_LOGERROR> TBL_LOGERROR { get; set; }

    public virtual DbSet<TBL_MaterialesSAP> TBL_MaterialesSAP { get; set; }

    public virtual DbSet<VSP_CLIENTE_X_SOCIEDAD_SAP> VSP_CLIENTE_X_SOCIEDAD_SAP { get; set; }


    public virtual ObjectResult<Nullable<System.DateTime>> sp_convertir_fecha(string fecha_entrada)
    {

        var fecha_entradaParameter = fecha_entrada != null ?
            new ObjectParameter("fecha_entrada", fecha_entrada) :
            new ObjectParameter("fecha_entrada", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.DateTime>>("sp_convertir_fecha", fecha_entradaParameter);
    }


    public virtual ObjectResult<string> sp_TBL_DetalleSAP(Nullable<int> documento, string cod_prod_sap, string nombre_segun_proveedor, Nullable<decimal> cantidad_solicitada)
    {

        var documentoParameter = documento.HasValue ?
            new ObjectParameter("documento", documento) :
            new ObjectParameter("documento", typeof(int));


        var cod_prod_sapParameter = cod_prod_sap != null ?
            new ObjectParameter("cod_prod_sap", cod_prod_sap) :
            new ObjectParameter("cod_prod_sap", typeof(string));


        var nombre_segun_proveedorParameter = nombre_segun_proveedor != null ?
            new ObjectParameter("nombre_segun_proveedor", nombre_segun_proveedor) :
            new ObjectParameter("nombre_segun_proveedor", typeof(string));


        var cantidad_solicitadaParameter = cantidad_solicitada.HasValue ?
            new ObjectParameter("cantidad_solicitada", cantidad_solicitada) :
            new ObjectParameter("cantidad_solicitada", typeof(decimal));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TBL_DetalleSAP", documentoParameter, cod_prod_sapParameter, nombre_segun_proveedorParameter, cantidad_solicitadaParameter);
    }


    public virtual ObjectResult<string> sp_TBL_DetalleSAP_Delete(Nullable<int> documento)
    {

        var documentoParameter = documento.HasValue ?
            new ObjectParameter("documento", documento) :
            new ObjectParameter("documento", typeof(int));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TBL_DetalleSAP_Delete", documentoParameter);
    }


    public virtual ObjectResult<string> sp_TBL_DetalleSAP_delete2(Nullable<int> documento, string material)
    {

        var documentoParameter = documento.HasValue ?
            new ObjectParameter("documento", documento) :
            new ObjectParameter("documento", typeof(int));


        var materialParameter = material != null ?
            new ObjectParameter("material", material) :
            new ObjectParameter("material", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TBL_DetalleSAP_delete2", documentoParameter, materialParameter);
    }


    public virtual ObjectResult<string> sp_TBL_DetalleSAP_update(Nullable<int> documento, string material, Nullable<decimal> cantidad_Solicitada)
    {

        var documentoParameter = documento.HasValue ?
            new ObjectParameter("documento", documento) :
            new ObjectParameter("documento", typeof(int));


        var materialParameter = material != null ?
            new ObjectParameter("material", material) :
            new ObjectParameter("material", typeof(string));


        var cantidad_SolicitadaParameter = cantidad_Solicitada.HasValue ?
            new ObjectParameter("Cantidad_Solicitada", cantidad_Solicitada) :
            new ObjectParameter("Cantidad_Solicitada", typeof(decimal));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TBL_DetalleSAP_update", documentoParameter, materialParameter, cantidad_SolicitadaParameter);
    }


    public virtual ObjectResult<string> sp_TBL_DetalleSAP_update1(Nullable<int> documento, string material, string mensaje)
    {

        var documentoParameter = documento.HasValue ?
            new ObjectParameter("documento", documento) :
            new ObjectParameter("documento", typeof(int));


        var materialParameter = material != null ?
            new ObjectParameter("material", material) :
            new ObjectParameter("material", typeof(string));


        var mensajeParameter = mensaje != null ?
            new ObjectParameter("mensaje", mensaje) :
            new ObjectParameter("mensaje", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TBL_DetalleSAP_update1", documentoParameter, materialParameter, mensajeParameter);
    }


    public virtual ObjectResult<string> sp_TBL_DetalleSAP_update2(Nullable<int> documento, string material, string mensaje)
    {

        var documentoParameter = documento.HasValue ?
            new ObjectParameter("documento", documento) :
            new ObjectParameter("documento", typeof(int));


        var materialParameter = material != null ?
            new ObjectParameter("material", material) :
            new ObjectParameter("material", typeof(string));


        var mensajeParameter = mensaje != null ?
            new ObjectParameter("mensaje", mensaje) :
            new ObjectParameter("mensaje", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TBL_DetalleSAP_update2", documentoParameter, materialParameter, mensajeParameter);
    }


    public virtual ObjectResult<string> sp_TBL_DetalleSAP_update2_1(Nullable<int> documento, string material, Nullable<decimal> cantidad, string centro, string almacen, string lote, string fecha_cad, string unidad, string mensaje)
    {

        var documentoParameter = documento.HasValue ?
            new ObjectParameter("documento", documento) :
            new ObjectParameter("documento", typeof(int));


        var materialParameter = material != null ?
            new ObjectParameter("material", material) :
            new ObjectParameter("material", typeof(string));


        var cantidadParameter = cantidad.HasValue ?
            new ObjectParameter("cantidad", cantidad) :
            new ObjectParameter("cantidad", typeof(decimal));


        var centroParameter = centro != null ?
            new ObjectParameter("centro", centro) :
            new ObjectParameter("centro", typeof(string));


        var almacenParameter = almacen != null ?
            new ObjectParameter("almacen", almacen) :
            new ObjectParameter("almacen", typeof(string));


        var loteParameter = lote != null ?
            new ObjectParameter("lote", lote) :
            new ObjectParameter("lote", typeof(string));


        var fecha_cadParameter = fecha_cad != null ?
            new ObjectParameter("fecha_cad", fecha_cad) :
            new ObjectParameter("fecha_cad", typeof(string));


        var unidadParameter = unidad != null ?
            new ObjectParameter("unidad", unidad) :
            new ObjectParameter("unidad", typeof(string));


        var mensajeParameter = mensaje != null ?
            new ObjectParameter("mensaje", mensaje) :
            new ObjectParameter("mensaje", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TBL_DetalleSAP_update2_1", documentoParameter, materialParameter, cantidadParameter, centroParameter, almacenParameter, loteParameter, fecha_cadParameter, unidadParameter, mensajeParameter);
    }


    public virtual ObjectResult<string> sp_TBL_DetalleSAP_update2_2(Nullable<int> documento, string document_sap, string document_year, string message, string type)
    {

        var documentoParameter = documento.HasValue ?
            new ObjectParameter("documento", documento) :
            new ObjectParameter("documento", typeof(int));


        var document_sapParameter = document_sap != null ?
            new ObjectParameter("document_sap", document_sap) :
            new ObjectParameter("document_sap", typeof(string));


        var document_yearParameter = document_year != null ?
            new ObjectParameter("document_year", document_year) :
            new ObjectParameter("document_year", typeof(string));


        var messageParameter = message != null ?
            new ObjectParameter("message", message) :
            new ObjectParameter("message", typeof(string));


        var typeParameter = type != null ?
            new ObjectParameter("type", type) :
            new ObjectParameter("type", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TBL_DetalleSAP_update2_2", documentoParameter, document_sapParameter, document_yearParameter, messageParameter, typeParameter);
    }


    public virtual ObjectResult<string> sp_TBL_DetalleSAP_update3(Nullable<int> documento, string material, string mensaje)
    {

        var documentoParameter = documento.HasValue ?
            new ObjectParameter("documento", documento) :
            new ObjectParameter("documento", typeof(int));


        var materialParameter = material != null ?
            new ObjectParameter("material", material) :
            new ObjectParameter("material", typeof(string));


        var mensajeParameter = mensaje != null ?
            new ObjectParameter("mensaje", mensaje) :
            new ObjectParameter("mensaje", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TBL_DetalleSAP_update3", documentoParameter, materialParameter, mensajeParameter);
    }


    public virtual ObjectResult<string> sp_TBL_DetalleSAP_update4(Nullable<int> documento, string material, string mensaje)
    {

        var documentoParameter = documento.HasValue ?
            new ObjectParameter("documento", documento) :
            new ObjectParameter("documento", typeof(int));


        var materialParameter = material != null ?
            new ObjectParameter("material", material) :
            new ObjectParameter("material", typeof(string));


        var mensajeParameter = mensaje != null ?
            new ObjectParameter("mensaje", mensaje) :
            new ObjectParameter("mensaje", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TBL_DetalleSAP_update4", documentoParameter, materialParameter, mensajeParameter);
    }


    public virtual ObjectResult<string> sp_TBL_EncabezadoSAP(string cod_casa_sap, string cod_prov_sap, string cod_cliente_sap, string sociedad, Nullable<int> centro, string cliente, string observaciones, Nullable<int> idusuario)
    {

        var cod_casa_sapParameter = cod_casa_sap != null ?
            new ObjectParameter("cod_casa_sap", cod_casa_sap) :
            new ObjectParameter("cod_casa_sap", typeof(string));


        var cod_prov_sapParameter = cod_prov_sap != null ?
            new ObjectParameter("cod_prov_sap", cod_prov_sap) :
            new ObjectParameter("cod_prov_sap", typeof(string));


        var cod_cliente_sapParameter = cod_cliente_sap != null ?
            new ObjectParameter("cod_cliente_sap", cod_cliente_sap) :
            new ObjectParameter("cod_cliente_sap", typeof(string));


        var sociedadParameter = sociedad != null ?
            new ObjectParameter("sociedad", sociedad) :
            new ObjectParameter("sociedad", typeof(string));


        var centroParameter = centro.HasValue ?
            new ObjectParameter("centro", centro) :
            new ObjectParameter("centro", typeof(int));


        var clienteParameter = cliente != null ?
            new ObjectParameter("cliente", cliente) :
            new ObjectParameter("cliente", typeof(string));


        var observacionesParameter = observaciones != null ?
            new ObjectParameter("observaciones", observaciones) :
            new ObjectParameter("observaciones", typeof(string));


        var idusuarioParameter = idusuario.HasValue ?
            new ObjectParameter("idusuario", idusuario) :
            new ObjectParameter("idusuario", typeof(int));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TBL_EncabezadoSAP", cod_casa_sapParameter, cod_prov_sapParameter, cod_cliente_sapParameter, sociedadParameter, centroParameter, clienteParameter, observacionesParameter, idusuarioParameter);
    }


    public virtual ObjectResult<string> sp_TBL_EncabezadoSAP_cambio_status1(Nullable<int> documento, string status)
    {

        var documentoParameter = documento.HasValue ?
            new ObjectParameter("documento", documento) :
            new ObjectParameter("documento", typeof(int));


        var statusParameter = status != null ?
            new ObjectParameter("status", status) :
            new ObjectParameter("status", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TBL_EncabezadoSAP_cambio_status1", documentoParameter, statusParameter);
    }


    public virtual ObjectResult<string> sp_TBL_EncabezadoSAP_cambio_status2(Nullable<int> documento, string status)
    {

        var documentoParameter = documento.HasValue ?
            new ObjectParameter("documento", documento) :
            new ObjectParameter("documento", typeof(int));


        var statusParameter = status != null ?
            new ObjectParameter("status", status) :
            new ObjectParameter("status", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TBL_EncabezadoSAP_cambio_status2", documentoParameter, statusParameter);
    }


    public virtual ObjectResult<string> sp_TBL_EncabezadoSAP_cambio_status3(Nullable<int> documento, string status)
    {

        var documentoParameter = documento.HasValue ?
            new ObjectParameter("documento", documento) :
            new ObjectParameter("documento", typeof(int));


        var statusParameter = status != null ?
            new ObjectParameter("status", status) :
            new ObjectParameter("status", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TBL_EncabezadoSAP_cambio_status3", documentoParameter, statusParameter);
    }


    public virtual ObjectResult<string> sp_TBL_EncabezadoSAP_Delete(Nullable<int> documento)
    {

        var documentoParameter = documento.HasValue ?
            new ObjectParameter("documento", documento) :
            new ObjectParameter("documento", typeof(int));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TBL_EncabezadoSAP_Delete", documentoParameter);
    }


    public virtual ObjectResult<string> sp_TBL_EncabezadoSAP_update(Nullable<int> documento, string cliente, string observaciones)
    {

        var documentoParameter = documento.HasValue ?
            new ObjectParameter("documento", documento) :
            new ObjectParameter("documento", typeof(int));


        var clienteParameter = cliente != null ?
            new ObjectParameter("cliente", cliente) :
            new ObjectParameter("cliente", typeof(string));


        var observacionesParameter = observaciones != null ?
            new ObjectParameter("observaciones", observaciones) :
            new ObjectParameter("observaciones", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TBL_EncabezadoSAP_update", documentoParameter, clienteParameter, observacionesParameter);
    }


    public virtual ObjectResult<string> sp_TBL_MaestrosCanjes_create(string matkl, string kunnr, string sociedad, string matnr, string matlab, string nsproveedor)
    {

        var matklParameter = matkl != null ?
            new ObjectParameter("matkl", matkl) :
            new ObjectParameter("matkl", typeof(string));


        var kunnrParameter = kunnr != null ?
            new ObjectParameter("kunnr", kunnr) :
            new ObjectParameter("kunnr", typeof(string));


        var sociedadParameter = sociedad != null ?
            new ObjectParameter("sociedad", sociedad) :
            new ObjectParameter("sociedad", typeof(string));


        var matnrParameter = matnr != null ?
            new ObjectParameter("matnr", matnr) :
            new ObjectParameter("matnr", typeof(string));


        var matlabParameter = matlab != null ?
            new ObjectParameter("matlab", matlab) :
            new ObjectParameter("matlab", typeof(string));


        var nsproveedorParameter = nsproveedor != null ?
            new ObjectParameter("nsproveedor", nsproveedor) :
            new ObjectParameter("nsproveedor", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TBL_MaestrosCanjes_create", matklParameter, kunnrParameter, sociedadParameter, matnrParameter, matlabParameter, nsproveedorParameter);
    }


    public virtual ObjectResult<string> sp_TBL_MaestrosCanjes_EliminarMasivo(string codcasa, string codprov, string sociedad)
    {

        var codcasaParameter = codcasa != null ?
            new ObjectParameter("codcasa", codcasa) :
            new ObjectParameter("codcasa", typeof(string));


        var codprovParameter = codprov != null ?
            new ObjectParameter("codprov", codprov) :
            new ObjectParameter("codprov", typeof(string));


        var sociedadParameter = sociedad != null ?
            new ObjectParameter("sociedad", sociedad) :
            new ObjectParameter("sociedad", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TBL_MaestrosCanjes_EliminarMasivo", codcasaParameter, codprovParameter, sociedadParameter);
    }


    public virtual ObjectResult<string> sp_TBL_DetalleSAP2(Nullable<int> documento, string cod_prod_sap, Nullable<decimal> cantidad_enviar, string almacen, string centro, string lote, string fecha_cad, string unidad, string mensaje3)
    {

        var documentoParameter = documento.HasValue ?
            new ObjectParameter("documento", documento) :
            new ObjectParameter("documento", typeof(int));


        var cod_prod_sapParameter = cod_prod_sap != null ?
            new ObjectParameter("cod_prod_sap", cod_prod_sap) :
            new ObjectParameter("cod_prod_sap", typeof(string));


        var cantidad_enviarParameter = cantidad_enviar.HasValue ?
            new ObjectParameter("cantidad_enviar", cantidad_enviar) :
            new ObjectParameter("cantidad_enviar", typeof(decimal));


        var almacenParameter = almacen != null ?
            new ObjectParameter("almacen", almacen) :
            new ObjectParameter("almacen", typeof(string));


        var centroParameter = centro != null ?
            new ObjectParameter("centro", centro) :
            new ObjectParameter("centro", typeof(string));


        var loteParameter = lote != null ?
            new ObjectParameter("lote", lote) :
            new ObjectParameter("lote", typeof(string));


        var fecha_cadParameter = fecha_cad != null ?
            new ObjectParameter("fecha_cad", fecha_cad) :
            new ObjectParameter("fecha_cad", typeof(string));


        var unidadParameter = unidad != null ?
            new ObjectParameter("unidad", unidad) :
            new ObjectParameter("unidad", typeof(string));


        var mensaje3Parameter = mensaje3 != null ?
            new ObjectParameter("mensaje3", mensaje3) :
            new ObjectParameter("mensaje3", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TBL_DetalleSAP2", documentoParameter, cod_prod_sapParameter, cantidad_enviarParameter, almacenParameter, centroParameter, loteParameter, fecha_cadParameter, unidadParameter, mensaje3Parameter);
    }


    public virtual ObjectResult<string> sp_TBL_MaestroCanjes_cambio_nombreSAP(string mATNR, string mAKTX)
    {

        var mATNRParameter = mATNR != null ?
            new ObjectParameter("MATNR", mATNR) :
            new ObjectParameter("MATNR", typeof(string));


        var mAKTXParameter = mAKTX != null ?
            new ObjectParameter("MAKTX", mAKTX) :
            new ObjectParameter("MAKTX", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TBL_MaestroCanjes_cambio_nombreSAP", mATNRParameter, mAKTXParameter);
    }


    public virtual ObjectResult<string> sp_LOGERROR(Nullable<int> documento, string mensaje, string type)
    {

        var documentoParameter = documento.HasValue ?
            new ObjectParameter("documento", documento) :
            new ObjectParameter("documento", typeof(int));


        var mensajeParameter = mensaje != null ?
            new ObjectParameter("mensaje", mensaje) :
            new ObjectParameter("mensaje", typeof(string));


        var typeParameter = type != null ?
            new ObjectParameter("type", type) :
            new ObjectParameter("type", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_LOGERROR", documentoParameter, mensajeParameter, typeParameter);
    }


    public virtual ObjectResult<string> sp_TBL_MaterialesSAP_Insert1()
    {

        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TBL_MaterialesSAP_Insert1");
    }


    public virtual ObjectResult<string> sp_TBL_MaterialesSAP_Update1(string matkl, string matnr, string matlab)
    {

        var matklParameter = matkl != null ?
            new ObjectParameter("matkl", matkl) :
            new ObjectParameter("matkl", typeof(string));


        var matnrParameter = matnr != null ?
            new ObjectParameter("matnr", matnr) :
            new ObjectParameter("matnr", typeof(string));


        var matlabParameter = matlab != null ?
            new ObjectParameter("matlab", matlab) :
            new ObjectParameter("matlab", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TBL_MaterialesSAP_Update1", matklParameter, matnrParameter, matlabParameter);
    }

}

}

