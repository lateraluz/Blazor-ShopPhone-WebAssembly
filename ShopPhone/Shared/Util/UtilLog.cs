using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Shared.Util;

/// <summary>
/// Wrote By. T. Ramirez
/// Referencias 
/// https://www.c-sharpcorner.com/article/use-log4net-in-asp-net-mvc-application/
/// https://logging.apache.org/log4net/
/// </summary>
public static class UtilLog
{

    /// <summary>
    /// Used to log Errors without return message
    /// </summary>
    /// <param name="message">The object message to log</param>  
    /// <param name="exception">The exception to log, including its stack trace </param>  
    public static string Error(System.Exception exception, MethodBase methodBase)
    {
        string dummy = "";
        // Invoca a Error y se le pasa una dummy string
        
        return Error(exception, methodBase, ref dummy);
    }

    /// <summary>
    /// Used to log Errors with return message
    /// </summary>
    /// <param name="message">The object message to log</param>  
    /// <param name="exception">The exception to log, including its stack trace </param>  
    /// <param name="pMensaje">retorna el error </param>  
    public static string Error(System.Exception exception, MethodBase methodBase, ref string pMensaje)
    {
        StringBuilder msg = new StringBuilder();

        // Es un error a nivel de Entity Framework
        if (exception is DbUpdateException)
        {
            msg.AppendFormat("{0}\n", DbException(exception as DbUpdateException, methodBase, ref pMensaje));          
        }
        else
        {
            // Es un error de SQLException
            if (exception is SqlException)
            {
                msg.AppendFormat("{0}\n", GetDetailsSQLException(exception! as SqlException));
                msg.AppendFormat("{0}\n", GetCustomErrorByNumber(exception! as SqlException));
                pMensaje = GetCustomErrorByNumber(exception! as SqlException);
            }
            else
            {
                // Un error por otro motivo
                msg.AppendFormat("{0}\n", CreateError(methodBase, exception));
                pMensaje = exception.Message;
            }
        }

        return msg.ToString();

    }

    private static string CreateError(MethodBase pMethodBase, Exception pExcepcion)
    {
        StringBuilder msg = new StringBuilder();
        msg.AppendFormat("\n\rClase      :{0}\n", pMethodBase.DeclaringType);
        msg.AppendFormat("Method     :{0}\n", pMethodBase.Name);
        msg.AppendFormat("Message    :{0}\n", pExcepcion.Message);
        msg.AppendFormat("Source     :{0}\n", pExcepcion.Source);
        msg.AppendFormat("StackTrace :{0}\n", pExcepcion.StackTrace);
        msg.AppendFormat("{0}\n", "".PadRight(100, '-'));
        return msg.ToString();
    }

    private static string DbException(DbUpdateException pDbUpdateException,
                                       MethodBase pMethodBase, ref string pMensaje)
    {

        StringBuilder msg = new StringBuilder();

        msg.AppendFormat("\n\r----------------------------------------------------------\n\n");

        msg.AppendFormat("Fecha del Error {0}\n", DateTime.Now.ToString("dd-MM-yy hh:mm:ss"));
        msg.AppendFormat("1- Clase: {0}\n\n", pMethodBase.DeclaringType);
        msg.AppendFormat("2- Method: {0}\n\n", pMethodBase.Name);


        if (pDbUpdateException.InnerException != null)
        {
            UpdateException updateException = (UpdateException)pDbUpdateException.InnerException;

            if (updateException.InnerException != null)
            {
                SqlException sqlException = (SqlException)updateException.InnerException;
                msg.AppendFormat(GetDetailsSQLException(sqlException));
                pMensaje = GetCustomErrorByNumber(sqlException);
            }
            else
            {
                msg.AppendFormat("3- Error Message   {0}\n\n", pDbUpdateException.Message);
                pMensaje = pDbUpdateException.Message;
                msg.AppendFormat("4- StackTrace      {0}\n\n", pDbUpdateException.StackTrace);
                msg.AppendFormat("5- Source          {0}\n\n", pDbUpdateException.Source);
                msg.AppendFormat("6- InnerException  {0}\n\n", pDbUpdateException.InnerException);
                msg.AppendFormat("7- Data            {0}\n\n", pDbUpdateException.Data);
            }
        }
        else
        {
            msg.AppendFormat("3- Error Message   {0}\n\n", pDbUpdateException.Message);
            pMensaje = pDbUpdateException.Message;
            msg.AppendFormat("4- StackTrace      {0}\n\n", pDbUpdateException.StackTrace);
            msg.AppendFormat("5- Source          {0}\n\n", pDbUpdateException.Source);
            msg.AppendFormat("6- InnerException  {0}\n\n", pDbUpdateException.InnerException);
            msg.AppendFormat("7- Data            {0}\n\n", pDbUpdateException.Data);
        }

        msg.AppendFormat("----------------------------------------------------------\n\n");


        return msg.ToString();

    }

    private static string GetDetailsSQLException(SqlException pExcepcion)
    {
        StringBuilder msg = new StringBuilder();

        msg.AppendFormat("\n");
        msg.AppendFormat("Lista de Errores SQL \n");
        msg.AppendFormat("----------------------------------------------------------\n");
        for (int i = 0; i < pExcepcion.Errors.Count; i++)
        {
            msg.AppendFormat("\t{0} de {1}\n", i + 1, pExcepcion.Errors.Count);
            msg.AppendFormat("\tNumber    : {1}\n", i + 1, pExcepcion.Errors[i].Number);
            msg.AppendFormat("\tLineNumber: {1}\n", i + 1, pExcepcion.Errors[i].LineNumber);
            msg.AppendFormat("\tMessage   : {1}\n", i + 1, pExcepcion.Errors[i].Message);

        }
        msg.AppendFormat("\n");
        msg.AppendFormat("StackTrace     {0}\n\n", pExcepcion.StackTrace);
        msg.AppendFormat(@"Info de errores para investigar mas https://technet.microsoft.com/en-us/library/cc645603(v=sql.105).aspx \n");
        msg.AppendFormat("---------------------------------------------------------\n");
        return msg.ToString();
    }


    private static string GetCustomErrorByNumber(SqlException pSqlExcepcion)
    {
        string msg = "\n";

        switch (pSqlExcepcion.Number)
        {
            case 17:
                msg = "Error el servidor '" + pSqlExcepcion.Server + "' no existe, por favor verifique el nombre";
                break;
            case 53:
                msg = "Error no se logra conectar al servidor, puede ser porque estén los servicios no activos o el nombre del servidor esté mal escrito.";
                break;
            case 109:
                msg = "Error hay más columnas en el INSERT que los parámetros que se envían.";
                break;
            case 110:
                msg = "Error hay menos columnas en el INSERT que los parámetros que se envían.";
                break;
            case 113:
                msg = "Error con comentarios.";
                break;
            case 137:
                msg = $"Error al declarar variables que se pasan al SQL. Mensaje de la BD {pSqlExcepcion.Message}";
                break;
            case 156:
                msg = "Error de sintaxis del SQL";
                break;
            case 207:
                msg = "Error un campo de la tabla NO existe";
                break;
            case 208:    //Invalid object name '%ls'."
                msg = "Error el nombre del objeto de la base de datos NO correcto o no EXISTE ";
                break;
            case 547:
                msg = "Error de Integridad, intentó  borrar o salvar un registro que tiene un dato Padre referenciado";
                break;
            case 2627:    //Login failed for user '%ls'. Reason: Not associated with a trusted SQL Server connection.
                msg = "Error el código que está intentando salvar YA EXISTE";
                break;
            case 2812:
                msg = "Error esta intentando invocar un procedimiento almacenado que NO EXISTE";
                break;
            case 4060:    //Cannot open database requested in login '%.*ls'. Login fails.
                msg = "No se puede acceder la base de datos, (estará en línea, bien configurada?)";
                break;
            case 18452:    //Login failed for user '%ls'. Reason: Not associated with a trusted SQL Server connection.
                msg = "Error especifique un usuario y password";
                break;
            case 18456: //Login failed for user '%ls'.
                msg = "Error el usuario o password es incorrecto";
                break;

            default:
                msg = pSqlExcepcion.Message;
                break;
        }

        return msg;

    }
}

