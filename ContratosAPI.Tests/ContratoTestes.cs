using System;
using System.Collections.Generic;
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
        public async void GetContratosTests()
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
                    Assert.NotEqual(item.QuantidadeParcelas, 0);
                    Assert.NotEqual(item.Id, 0);
                    Assert.NotNull(item.Prestacoes);
                    Assert.NotEqual(item.ValorFinanciado, 0);
                }
            }  
        } 

        [Fact]
        public async void GetContratoTeste()
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
                Contrato response = await contrato.GetContratoService(1);
                Assert.NotEqual(response.DataContratacao, string.Empty);
                Assert.NotEqual(response.QuantidadeParcelas, 0);
                Assert.NotEqual(response.Id, 0);
                Assert.NotNull(response.Prestacoes);
                Assert.NotEqual(response.ValorFinanciado, 0);
                
            }  
        } 


        private void CriarContrato(DbContextOptions<DataContext> options)
        {   
            var p = new Prestacao();
            p.ContratoId = 1;
            p.DataPagamento = DateTime.Today.Date;
            p.DataVencimento = DateTime.Today.Date;
            p.Status = "Baixada";
            p.Valor = 22;
            var prestacoes = new List<Prestacao>();
            prestacoes.Add(p);
            using (var context = new DataContext(options))
            {
                context.Contratos.Add(new Contrato {DataContratacao = "10/01/2021", QuantidadeParcelas = 2, ValorFinanciado = 4, Prestacoes = prestacoes});
                context.Contratos.Add(new Contrato {DataContratacao = "11/01/2021", QuantidadeParcelas = 3, ValorFinanciado = 5, Prestacoes = prestacoes});
                context.Contratos.Add(new Contrato {DataContratacao = "12/01/2021", QuantidadeParcelas = 4, ValorFinanciado = 6, Prestacoes = prestacoes});
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