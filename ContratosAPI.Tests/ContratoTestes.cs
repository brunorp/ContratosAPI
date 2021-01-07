using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ContratosAPI.Data;
using ContratosAPI.Models;
using ContratosAPI.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Protected;
using Xunit;

namespace ContratosAPI.Tests
{
    public class ContratoTestes
    {
        private static Mock<HttpMessageHandler> _mock = new Mock<HttpMessageHandler>();
        
        [Fact]
        public async void ResponseTests()
        {
            MockSetup("../../../Data/MockData.json", HttpStatusCode.OK);

            var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "Database")
            .Options;

            CriarContrato(options);

            // Use a clean instance of the context to run the test
            using (var context = new DataContext(options))
            {
                ContratoService contrato = new ContratoService(context);
                var response = await contrato.GetContratosService();
                foreach (var item in response)
                {
                    Assert.NotEqual(item.DataContratacao, string.Empty);
                }
            }  
        } 
        
        private void CriarContrato(DbContextOptions<DataContext> options)
        {
            using (var context = new DataContext(options))
            {
                context.Contratos.Add(new Contrato {DataContratacao = "10/01/2021", QuantidadeParcelas = 2, ValorFinanciado = 4, Prestacoes = 1});
                context.Contratos.Add(new Contrato {DataContratacao = "11/01/2021", QuantidadeParcelas = 3, ValorFinanciado = 5, Prestacoes = 2});
                context.Contratos.Add(new Contrato {DataContratacao = "12/01/2021", QuantidadeParcelas = 4, ValorFinanciado = 6, Prestacoes = 3});
                context.SaveChanges();
            }
        }

        private void MockSetup(string dir, HttpStatusCode statusCode)
        {   
            var mockData = new StringContent(File.ReadAllText(dir));
            _mock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = mockData
            });
        }
    }
}