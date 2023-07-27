using iTextSharp.text.pdf;
using iTextSharp.text;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopPhone.Services.Implementations;
using ShopPhone.Shared.Response;
using System.Reflection;
using System.Reflection.Metadata;
using static iTextSharp.text.pdf.AcroFields;


namespace ShopPhone.Server.Controllers;

[Route("api/[controller]")]
[ApiController]

public class VentaController : ControllerBase
{
    private IVentaService _VentaService;
    private ILog _Logger;

    public VentaController(IVentaService service, ILog logger)
    {
        _VentaService = service;
        _Logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Post(FacturaDTO request)
    {
        try
        {
            var response = await _VentaService.AddAsync(request);

            return response.Success ? Ok(response) : NotFound(response);
        }
        catch (Exception ex)
        {
            _Logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }

    [HttpGet("List")]
    public async Task<IActionResult> ListAsync()
    {
        try
        {
            var response = await _VentaService.ListAsync();

            return response.Success ? Ok(response) : NotFound(response);
        }
        catch (Exception ex)
        {
            _Logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }

    }


    [HttpGet("FindById")]
    public async Task<IActionResult> FindByIdAsync(int id)
    {
        try
        {
            var response = await _VentaService.FindByIdAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }
        catch (Exception ex)
        {
            _Logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }

    [HttpGet("myfactura")]
    public async Task<IActionResult> GetFactura()
    {
        try
        {

            //await Task.FromResult(1);
            GeneratePdf();

            // var response = await _VentaService.ListAsync();

            // return response.Success ? Ok(response) : NotFound(response);

            var file = System.IO.File.ReadAllBytes(@"c:\temp\images\pdf\archivo.pdf");

            HttpContext.Response.Headers.ContentDisposition = "inline;filename=1.pdf";


            return File(file, "application/pdf", "1.pdf");
            // return Ok("HOLA");
        }
        catch (Exception ex)
        {
            _Logger.Error($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            throw;
        }
    }



    private void GeneratePdf()
    {
        using (MemoryStream myMemoryStream = new MemoryStream())
        {

            FacturaDTO factura = new();
            factura.IdFactura = 123;
            factura.FechaVenta = DateTime.Now;
            // Cliente
            factura._Cliente.Nombre = "Juan";
            factura._Cliente.Apellidos = "Gonzalez";
            factura._Cliente.IdCliente = 123;


            List<FacturaDetalleDTO> detalle = new List<FacturaDetalleDTO>();

            detalle.Add(new FacturaDetalleDTO() { IdProducto = 1,
                Cantidad = 1,
                Descripcion = "Azus",
                PrecioUnitario = 154123,
                Impuesto = 25, Secuencia = 0 });
            detalle.Add(new FacturaDetalleDTO()
            {
                IdProducto = 2,
                Cantidad = 2,
                Descripcion = "Samsun",
                PrecioUnitario = 868,
                Impuesto = 4,
                Secuencia = 1
            });


            factura._FacturaDetalle = detalle;
            //*******************************************************************//

            iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.Letter.Rotate(), 5, 5, 15, 15);
            PdfWriter myPDFWriter = PdfWriter.GetInstance(document, myMemoryStream);
            document.Open();             

            // Add to content to your PDF here..
            iTextSharp.text.Font courier = new iTextSharp.text.Font(iTextSharp.text.Font.COURIER, 7f);
            PdfPTable table = new PdfPTable(1);
            table.SpacingAfter = 10;
            table.SpacingBefore = 10;
            Font fuente = FontFactory.GetFont("Times New Roman", 20, Font.BOLD);
            Font fuente2 = FontFactory.GetFont("Times New Roman", 12, Font.BOLD);

            PdfPCell header = new PdfPCell(new Phrase("Factura", fuente));
            header.Colspan = 2;
            header.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            
            table.AddCell(header);
            PdfPCell item1 = new PdfPCell(new Phrase("Factura -" + factura.IdFactura));
            item1.Border = 0;
            PdfPCell item2 = new PdfPCell(new Phrase("Cliente - " + factura._Cliente.Nombre + " " + factura._Cliente.Apellidos));
            item2.Border = 0;
            PdfPCell item3 = new PdfPCell(new Phrase("Fecha - " + DateTime.Now.ToShortDateString()));
            item3.Border = 0;

            table.AddCell(item1);
            table.AddCell(item2);
            table.AddCell(item3);
            table.SpacingBefore = 10f;
            table.SpacingAfter = 10f;
            document.Add(table);

            //**********************************************
            PdfPTable table2 = new PdfPTable(7);
            float[] widths = new float[] { 5f, 10f,20f,10f,10f,10f, 10f};
            table2.SetWidths(widths);
            PdfPCell headerNo = new PdfPCell(new Phrase("No", fuente2));            
            table2.AddCell(headerNo);

            PdfPCell headerCodigo = new PdfPCell(new Phrase("Código", fuente2));          
            table2.AddCell(headerCodigo);

            PdfPCell headerDescripcion = new PdfPCell(new Phrase("Descripción", fuente2));            
            table2.AddCell(headerDescripcion);

            PdfPCell headerCantidad = new PdfPCell(new Phrase("Cantidad", fuente2));
            headerCantidad.HorizontalAlignment = Element.ALIGN_RIGHT;
            table2.AddCell(headerCantidad);
            PdfPCell headerPrecio = new PdfPCell(new Phrase("Precio", fuente2));
            headerPrecio.HorizontalAlignment = Element.ALIGN_RIGHT;
            table2.AddCell(headerPrecio);
            PdfPCell headerImpuesto = new PdfPCell(new Phrase("Impuesto", fuente2));
            headerImpuesto.HorizontalAlignment = Element.ALIGN_RIGHT;
            table2.AddCell(headerImpuesto);
            PdfPCell headerTotal = new PdfPCell(new Phrase("Total", fuente2));
            headerTotal.HorizontalAlignment = Element.ALIGN_RIGHT;
            table2.AddCell(headerTotal);
            
            //table.WidthPercentage = 100;
            foreach (var item in factura._FacturaDetalle)
            {
                table2.AddCell(item.Secuencia.ToString());
                table2.AddCell(item.IdProducto.ToString());
                table2.AddCell(item.Descripcion);
               
                PdfPCell cellCantidad = new PdfPCell(new Phrase(item.Cantidad.ToString()));
                cellCantidad.HorizontalAlignment = Element.ALIGN_RIGHT;                
                table2.AddCell(cellCantidad);

                PdfPCell cellPrecio = new PdfPCell(new Phrase(item.PrecioUnitario.ToString("N")));
                cellPrecio.HorizontalAlignment = Element.ALIGN_RIGHT;
                table2.AddCell(cellPrecio);

                PdfPCell cellImpuesto = new PdfPCell(new Phrase(item.Impuesto.ToString("N")));
                cellImpuesto.HorizontalAlignment = Element.ALIGN_RIGHT;
                table2.AddCell(cellImpuesto);
                double total = item.Cantidad * item.PrecioUnitario;
                total = total * .13;

                PdfPCell cellTotal = new PdfPCell(new Phrase(total.ToString("N")));
                cellTotal.HorizontalAlignment = Element.ALIGN_RIGHT;
                table2.AddCell(cellTotal);
            }

            // Last Row 
            PdfPCell dummyCell = new PdfPCell(new Phrase(""));
            dummyCell.Border = 0;
            table2.AddCell(dummyCell);
            table2.AddCell(dummyCell);
            table2.AddCell(dummyCell);

            PdfPCell cellCantidad2 = new PdfPCell(new Phrase(""));
            cellCantidad2.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellCantidad2.Border = 0;
            table2.AddCell(cellCantidad2);

            PdfPCell cellPrecio2 = new PdfPCell(new Phrase(""));
            cellPrecio2.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellPrecio2.Border = 0;
            table2.AddCell(cellPrecio2);

            PdfPCell cellImpuesto2 = new PdfPCell(new Phrase("Gran Total"));
            cellImpuesto2.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellImpuesto2.Border = 0;
            table2.AddCell(cellImpuesto2);

            double granTotal = factura._FacturaDetalle.Sum(x => x.PrecioUnitario * x.Cantidad + x.Impuesto ); 

            PdfPCell cellTotal2 = new PdfPCell(new Phrase(granTotal.ToString("N"),fuente2));
            cellTotal2.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellTotal2.Border = 0;
            table2.AddCell(cellTotal2); 

            document.Add(table2);           
            document.Close();


            byte[] content = myMemoryStream.ToArray();

            // Write out PDF from memory stream.
            using (FileStream fs = System.IO.File.Create(@"c:\temp\images\pdf\archivo.pdf"))
            {
                fs.Write(content, 0, (int)content.Length);
            }
        }



    }
}


