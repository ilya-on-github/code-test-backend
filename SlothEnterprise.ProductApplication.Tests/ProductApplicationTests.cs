using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Products;

namespace SlothEnterprise.ProductApplication.Tests
{
    [TestFixture]
    public class ProductApplicationTests
    {
        private Fixture _fixture;
        private Mock<ISelectInvoiceService> _mockSelectInvoiceService;
        private Mock<IConfidentialInvoiceService> _mockConfidentialInvoiceService;
        private Mock<IBusinessLoansService> _mockBusinessLoansService;

        [SetUp]
        public void SetUp()
        {
            _mockSelectInvoiceService = new Mock<ISelectInvoiceService>();
            _mockConfidentialInvoiceService = new Mock<IConfidentialInvoiceService>();
            _mockBusinessLoansService = new Mock<IBusinessLoansService>();

            _fixture = new Fixture();

            _fixture.Register(() => _mockSelectInvoiceService.Object);
            _fixture.Register(() => _mockConfidentialInvoiceService.Object);
            _fixture.Register(() => _mockBusinessLoansService.Object);
            _fixture.Register<ISellerCompanyData>(() => _fixture.Create<SellerCompanyData>());
        }


        [Test]
        public void SubmitApplicationFor_ForSellerInvoiceDiscount_ShouldCallSelectInvoiceService()
        {
            // arrange
            var product = _fixture.Create<SelectiveInvoiceDiscount>();
            var application = _fixture.Build<SellerApplication>()
                .With(x => x.Product, product)
                .Create();

            var service = _fixture.Create<ProductApplicationService>();

            // act
            service.SubmitApplicationFor(application);

            // assert
            _mockSelectInvoiceService.Verify(x => x.SubmitApplicationFor(application.CompanyData.Number.ToString(),
                product.InvoiceAmount, product.AdvancePercentage), Times.Once);
        }


        [Test]
        public void SubmitApplicationFor_ForSellerInvoiceDiscount_ShouldReturnServiceCallResult()
        {
            // arrange
            var product = _fixture.Create<SelectiveInvoiceDiscount>();
            var application = _fixture.Build<SellerApplication>()
                .With(x => x.Product, product)
                .Create();
            var applicationId = _fixture.Create<int>();
            _mockSelectInvoiceService.Setup(x =>
                    x.SubmitApplicationFor(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>()))
                .Returns(applicationId);

            var service = _fixture.Create<ProductApplicationService>();

            // act
            var result = service.SubmitApplicationFor(application);

            // assert
            result.Should().Be(applicationId);
        }

        // ...tests for other products
        //
        // as far as the ProductApplicationService is a facade calling external dependencies
        // we should first of all check that calls are correct
        // and then cover the rest conditions
    }
}