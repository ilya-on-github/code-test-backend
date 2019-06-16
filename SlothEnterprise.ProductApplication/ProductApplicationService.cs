using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Products;

namespace SlothEnterprise.ProductApplication
{
    public class ProductApplicationService : ISubmitApplicationVisitor
    {
        private readonly ISelectInvoiceService _selectInvoiceService;
        private readonly IConfidentialInvoiceService _confidentialInvoiceWebService;
        private readonly IBusinessLoansService _businessLoansService;

        public ProductApplicationService(ISelectInvoiceService selectInvoiceService, IConfidentialInvoiceService confidentialInvoiceWebService, IBusinessLoansService businessLoansService)
        {
            _selectInvoiceService = selectInvoiceService;
            _confidentialInvoiceWebService = confidentialInvoiceWebService;
            _businessLoansService = businessLoansService;
        }

        /// <summary>
        /// Submits an application.
        /// </summary>
        /// <param name="application"></param>
        /// <returns>An application ID (or -1 in case of failure).</returns>
        public int SubmitApplicationFor(ISellerApplication application)
        {
            return application.Product.Visit(this, application);
        }

        public int SubmitApplication(ISellerApplication application, SelectiveInvoiceDiscount product)
        {
            return _selectInvoiceService.SubmitApplicationFor(application.CompanyData.Number.ToString(),
                product.InvoiceAmount, product.AdvancePercentage);
        }

        public int SubmitApplication(ISellerApplication application, ConfidentialInvoiceDiscount product)
        {
            var result = _confidentialInvoiceWebService.SubmitApplicationFor(
                new CompanyDataRequest
                {
                    CompanyFounded = application.CompanyData.Founded,
                    CompanyNumber = application.CompanyData.Number,
                    CompanyName = application.CompanyData.Name,
                    DirectorName = application.CompanyData.DirectorName
                }, product.TotalLedgerNetworth, product.AdvancePercentage, product.VatRate);

            return result.Success ? result.ApplicationId ?? -1 : -1;
        }

        public int SubmitApplication(ISellerApplication application, BusinessLoans product)
        {
            var result = _businessLoansService.SubmitApplicationFor(new CompanyDataRequest
            {
                CompanyFounded = application.CompanyData.Founded,
                CompanyNumber = application.CompanyData.Number,
                CompanyName = application.CompanyData.Name,
                DirectorName = application.CompanyData.DirectorName
            }, new LoansRequest
            {
                InterestRatePerAnnum = product.InterestRatePerAnnum,
                LoanAmount = product.LoanAmount
            });
            return result.Success ? result.ApplicationId ?? -1 : -1;
        }
    }
}
