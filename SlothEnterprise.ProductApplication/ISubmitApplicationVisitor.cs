using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Products;

namespace SlothEnterprise.ProductApplication
{
    public interface ISubmitApplicationVisitor
    {
        int SubmitApplication(ISellerApplication application, SelectiveInvoiceDiscount product);
        int SubmitApplication(ISellerApplication application, ConfidentialInvoiceDiscount product);
        int SubmitApplication(ISellerApplication application, BusinessLoans product);
    }
}