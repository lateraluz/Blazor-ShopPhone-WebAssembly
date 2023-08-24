using AutoMapper;
using Azure;
using log4net;
using ShopPhone.DataAccess;
using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static iTextSharp.text.pdf.AcroFields;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using ShopPhone.Shared.Entities;
using ShopPhone.Repositories.Interfaces;
using ShopPhone.Services.Interfaces;

namespace ShopPhone.Services.Implementations
{
    public class VentaService : IVentaService
    {
        private IVentaRepository _ventaRepository;
        private readonly IMapper _mapper;
        private ILog _logger;
        private readonly IOptions<AppConfig> _options;


        public VentaService(IVentaRepository repository, IMapper mapper, ILog logger, IOptions<AppConfig> options)
        {
            _ventaRepository = repository;
            _mapper = mapper;
            _logger = logger;
            _options = options;
        }

        public async Task<BaseResponseGeneric<int>> AddAsync(FacturaDTO identity)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                // Map
                var factura = _mapper.Map<FacturaEncabezado>(identity);

                // Get No Receipt and assign Receipt Number
                factura.IdFactura = _ventaRepository.GetNoReceipt();

                identity._FacturaDetalle.ForEach(
                    x => x.IdFactura = identity.IdFactura
                    );

                var baseResponse = await _ventaRepository.AddAsync(factura);
                response.Success = true;
                response.Data = factura.IdFactura;//identity.IdFactura;

                var facturaProcesada = await _ventaRepository.FindAsync(factura.IdFactura);

                // Crear Factura
                GeneratePdf(facturaProcesada!);
                SendEmail(identity._Cliente.CorreoElectronico);

                _logger.Info($"Venta realizada con exito");
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = $"Error al salvar el factura ";
                _logger.Error($"{response.ErrorMessage}  en {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                return response;
            }
        }

        private async void SendEmail(string email)
        {
            try
            {

                if (_options.Value.SmtpConfiguration == null)
                {
                    _logger.Error($"No se encuentra configurado ningun valor para SMPT en {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}");
                    return;
                }

                // force sending email to a specific account
                if (string.IsNullOrEmpty(_options.Value.SmtpConfiguration.DummyRecipient) == false)
                {
                    email = _options.Value.SmtpConfiguration.DummyRecipient;
                }
                var mailMessage = new MailMessage(
                    new MailAddress(_options.Value.SmtpConfiguration.UserName, _options.Value.SmtpConfiguration.FromName),
                    new MailAddress(email))
                {
                    Subject = "Factura Electrónica para " + email,
                    Body = "Adjunto Factura Electronica de ShopPhone",
                    IsBodyHtml = true
                };

                Attachment attachment = new Attachment(@"c:\\temp\\images\\pdf\\archivo.pdf");
                mailMessage.Attachments.Add(attachment);

                using var smtpClient = new SmtpClient(_options.Value.SmtpConfiguration.Server,
                                                       _options.Value.SmtpConfiguration.PortNumber)
                {
                    Credentials = new NetworkCredential(
                                                                _options.Value.SmtpConfiguration.UserName,
                                                                _options.Value.SmtpConfiguration.Password),
                    EnableSsl = _options.Value.SmtpConfiguration.EnableSsl,
                };

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Silent Error 
                _logger.Error($"Error al enviar el correo  en {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
            }
        }

        public async Task<BaseResponseGeneric<ICollection<FacturaDTO>>> FindByIdAsync(int id)
        {
            BaseResponseGeneric<ICollection<FacturaDTO>> response = new BaseResponseGeneric<ICollection<FacturaDTO>>();
            try
            {
                var facturaProcesada = await _ventaRepository.FindAsync(id);
                var @object = _mapper.Map<FacturaDTO>(facturaProcesada);
                List<FacturaDTO> lista = new List<FacturaDTO>();
                lista.Add(@object);
                response.Success = true;
                response.Data = lista;
                return response;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
        }

        public Task<BaseResponse> UpdateAsync(int id, FacturaDTO request)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponseGeneric<ICollection<FacturaDTO>>> ListAsync()
        {
            BaseResponseGeneric<ICollection<FacturaDTO>> response = new BaseResponseGeneric<ICollection<FacturaDTO>>();

            try
            {

                var collection = await _ventaRepository.ListAsync();
                response.Success = true;
                response.Data = _mapper.Map<ICollection<FacturaDTO>>(collection);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = $"Error al consultar las facturas ";
                _logger.Error($"{response.ErrorMessage}  en {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                return response;
            }
        }
        private void GeneratePdf(FacturaEncabezado factura)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {

                //*******************************************************************//

                iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.Letter.Rotate(), 5, 5, 15, 15);
                PdfWriter myPDFWriter = PdfWriter.GetInstance(document, myMemoryStream);
                document.Open();

                // Add to content to your PDF here..
                iTextSharp.text.Font courier = new iTextSharp.text.Font(iTextSharp.text.Font.COURIER, 7f);
                PdfPTable table = new PdfPTable(1);
                table.SpacingAfter = 10;
                table.SpacingBefore = 10;
                iTextSharp.text.Font fuente = FontFactory.GetFont("Times New Roman", 20, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font fuente2 = FontFactory.GetFont("Times New Roman", 12, iTextSharp.text.Font.BOLD);

                PdfPCell header = new PdfPCell(new Phrase("Factura", fuente));
                header.Colspan = 2;
                header.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                table.AddCell(header);
                PdfPCell item1 = new PdfPCell(new Phrase("Factura -" + factura.IdFactura));
                item1.Border = 0;
                PdfPCell item2 = new PdfPCell(new Phrase("Cliente - " + factura.IdClienteNavigation.Nombre.Trim() + " " + factura.IdClienteNavigation.Apellidos.Trim()));
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
                float[] widths = new float[] { 5f, 10f, 20f, 10f, 10f, 10f, 10f };
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
                foreach (var item in factura.FacturaDetalles)
                {
                    table2.AddCell(item.Secuencia.ToString());
                    table2.AddCell(item.IdProducto.ToString());
                    table2.AddCell(item.IdProductoNavigation.Descripcion.Trim());

                    PdfPCell cellCantidad = new PdfPCell(new Phrase(item.Cantidad.ToString()));
                    cellCantidad.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table2.AddCell(cellCantidad);

                    PdfPCell cellPrecio = new PdfPCell(new Phrase(item.PrecioUnitario.ToString("N")));
                    cellPrecio.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table2.AddCell(cellPrecio);

                    PdfPCell cellImpuesto = new PdfPCell(new Phrase(item.Impuesto.ToString("N")));
                    cellImpuesto.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table2.AddCell(cellImpuesto);
                    decimal total = item.Cantidad * item.PrecioUnitario;
                    total = total * (decimal)0.13;

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

                double granTotal = (double)factura.FacturaDetalles.ToList().Sum(x => x.PrecioUnitario * x.Cantidad + x.Impuesto);

                PdfPCell cellTotal2 = new PdfPCell(new Phrase(granTotal.ToString("N"), fuente2));
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

}
