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
        
        // Testa o resultado da busca de todos os contratos
        [Fact]
        public async void GetContratosTeste()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "Database")
            .Options;

            CriarMultiplosContratos(options);

            // Use a clean instance of the context to run the test
            using (var context = new DataContext(options))
            {
                ContratoService contrato = new ContratoService(context);
                var resultado = await contrato.GetContratosService();
                foreach (var item in resultado)
                {
                    Assert.NotEqual(item.DataContratacao, string.Empty);
                    Assert.NotEqual(0, item.QuantidadeParcelas);
                    Assert.NotEqual(0, item.Id);
                    Assert.NotEqual(0, item.ValorFinanciado);
                    Assert.NotNull(item.Prestacoes);
                }
            }  
        } 

        // Testa o resultado da busca de um contrato específico
        [Fact]
        public async void GetContratoTeste()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "Database")
            .Options;

            CriarMultiplosContratos(options);

            // Use a clean instance of the context to run the test
            using (var context = new DataContext(options))
            {
                ContratoService contrato = new ContratoService(context);
                Contrato resultado = await contrato.GetContratoService(2);
                Assert.NotEqual(resultado.DataContratacao, string.Empty);
                Assert.NotEqual(0, resultado.QuantidadeParcelas);
                Assert.NotEqual(0, resultado.Id);
                Assert.NotEqual(0, resultado.ValorFinanciado);
                Assert.NotNull(resultado.Prestacoes);
            }  
        } 

        // Testa a criação de contratos e suas respectivas parcelas
        [Fact]
        public async void PostContratoTeste()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "Database")
            .Options;

            // Use a clean instance of the context to run the test
            using (var context = new DataContext(options))
            {
                var novoContrato = new Contrato {
                    DataContratacao = "10/01/2021", 
                    QuantidadeParcelas = 2, 
                    ValorFinanciado = 4};

                ContratoService contrato = new ContratoService(context);
                Contrato resultado = await contrato.PostContratoService(novoContrato);
                Assert.NotEqual(resultado.DataContratacao, string.Empty);
                Assert.NotEqual(0, resultado.QuantidadeParcelas);
                Assert.NotEqual(0, resultado.Id);
                Assert.NotEqual(0, resultado.ValorFinanciado);
                Assert.Equal(2, resultado.Prestacoes.Count);   
            }  
        } 

        // Testa a edição de contratos e suas respectivas parcelas
        [Fact]
        public async void PutContratoTeste()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "Database")
            .Options;

            CriarMultiplosContratos(options);

            // Use a clean instance of the context to run the test
            using (var context = new DataContext(options))
            {
                var contratoEditado = new Contrato {
                    DataContratacao = "15/01/2021", 
                    QuantidadeParcelas = 10, 
                    ValorFinanciado = 200};

                ContratoService contrato = new ContratoService(context);
                Contrato resultado = await contrato.PutContratoService(2, contratoEditado);
                Assert.NotEqual(resultado.DataContratacao, string.Empty);
                Assert.NotEqual(0, resultado.QuantidadeParcelas);
                Assert.NotEqual(0, resultado.Id);
                Assert.NotNull(resultado.Prestacoes);
                Assert.Equal(200, resultado.ValorFinanciado);
            }  
        } 

        [Fact]
        public async void DeleteContratoTeste()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "Database")
            .Options;

            CriarMultiplosContratos(options);

            // Use a clean instance of the context to run the test
            using (var context = new DataContext(options))
            {
                ContratoService contrato = new ContratoService(context);
                await contrato.DeleteContratoService(1);
                await Assert.ThrowsAsync<ArgumentNullException>(
                    async () => await contrato.GetContratoService(1)
                );
            }  
        } 
        
        private void CriarMultiplosContratos(DbContextOptions<DataContext> options)
        {   
            var prestacao = new Prestacao();
            prestacao.ContratoId = 1;
            prestacao.DataPagamento = DateTime.Today.Date;
            prestacao.DataVencimento = DateTime.Today.Date;
            prestacao.Status = "Baixada";
            prestacao.Valor = 22;
            var prestacoes = new List<Prestacao>();
            prestacoes.Add(prestacao);
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